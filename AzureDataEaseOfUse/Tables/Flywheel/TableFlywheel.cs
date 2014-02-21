using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables.Async
{
    /// <summary>
    /// Used to process a flow of table batches.
    /// Abstracting the user from the batch creation mechanics.
    /// Executing batches asynchronously when they fill up (100) or when Flush() is called.
    /// Order of execution is not guaranteed.
    /// </summary>
    public class TableFlywheel<T> where T : TableEntity, IAzureStorageTable
    {
        public TableFlywheel(CloudTable table)
        {
            this.Table = table;
            this.Retrieved = new List<T>();
            this.Errors = new List<FlywheelError<T>>();
        }

        public readonly CloudTable Table;

        Dictionary<string, List<TableBatch<T>>> Pending = new Dictionary<string, List<TableBatch<T>>>();

        List<FlywheelResult<T>> Executing = new List<FlywheelResult<T>>();

        public readonly List<T> Retrieved;

        public readonly List<FlywheelError<T>> Errors;

        public long SuccessCount { get; private set; }

        /// <summary>
        /// Count of Batches Pending Transmission
        /// </summary>
        public int PendingCount()
        {
            int count = 0;

            foreach (var partition in Pending)
                count += partition.Value.Count;

            return count;
        }

        /// <summary>
        /// Count of all the executing change/retrieves (not count of batches)
        /// </summary>
        public int ExecutingCount()
        { 
            int count = 0;

            foreach (var item in Executing)
                count += item.Batch.Count;

            return count;
        }


        #region CRUD Operations

        public TableFlywheel<T> Insert(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.Insert));
        }

        public TableFlywheel<T> Replace(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.Replace));
        }

        public TableFlywheel<T> Delete(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.Delete));
        }

        public TableFlywheel<T> Retrieve(string partitionKey, string rowKey)
        {
            return Append(new FlywheelOperation<T>(partitionKey, rowKey));
        }

        public TableFlywheel<T> InsertOrReplace(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.InsertOrReplace));
        }

        public TableFlywheel<T> Merge(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.Merge));
        }

        public TableFlywheel<T> InsertOrMerge(T item)
        {
            return Append(new FlywheelOperation<T>(item, TableOperationType.InsertOrMerge));
        }

        #endregion

        #region Batch Directory Management

        /// <summary>
        /// Appends operation to the flywheel to be processed
        /// </summary>
        private TableFlywheel<T> Append(FlywheelOperation<T> operation)
        {
            var partition = GetFlywheelPartition(operation);

            var batch = partition.FirstOrDefault(q => q.CanInclude(operation));

            if (batch == null)
            {
                batch = new TableBatch<T>(operation);
                partition.Add(batch);
            }
            else
            {
                batch.Include(operation);
            }

            FlushIfFull(batch);

            return this;
        }

        #endregion

        #region Partitions

        private List<TableBatch<T>> GetFlywheelPartition(FlywheelOperation<T> request)
        {
            var key = GetFlywheelPartitionKey(request.PartitionKey, request.IsChange);

            return GetFlywheelPartition(key);
        }

        private List<TableBatch<T>> GetFlywheelPartition(TableBatch<T> batch)
        {
            var key = GetFlywheelPartitionKey(batch.PartitionKey, batch.IsChange);

            return GetFlywheelPartition(key);
        }

        private List<TableBatch<T>> GetFlywheelPartition(string key)
        {
            if (Pending.ContainsKey(key) == false)
                Pending.Add(key, new List<TableBatch<T>>());

            return Pending[key];
        }

        private string GetFlywheelPartitionKey(string partitionKey, bool isChange)
        {
            return partitionKey + "-" + isChange.ToString();
        }


        #endregion

        #region Flush (aka execute)

        private void FlushIfFull(TableBatch<T> batch)
        {
            if (batch.IsFull())
            {
                Flush(batch);
                ProcessExecuting();
            }
        }
        /// <summary>
        /// Executes all pending batches (async) and places the outcome into the Results with the batch that kicked it off
        /// </summary>
        public void Flush()
        {
            var batches = new List<TableBatch<T>>();

            foreach (var partition in Pending.Values)
                batches.AddRange(partition);

            batches.ForEach(batch => Flush(batch));

            ProcessExecuting(waitAll: true);
        }

        /// <summary>
        /// Executes the batch (async) and places the outcome into the Results with the batch that kicked it off
        /// </summary>
        private void Flush(TableBatch<T> batch)
        {
            // Execute
            var operation = batch.GetBatchOperation();

            var task = Table.ExecuteBatchAsync(operation);

            var result = new FlywheelResult<T>(batch, task);

            // Move to results
            Executing.Add(result);

            var partition = GetFlywheelPartition(batch);

            partition.Remove(batch);
        }

        #endregion

        public bool HasErrors { get { return Errors.Count > 0; } }

        /// <summary>
        /// Waits for all flushed results to finish
        /// </summary>
        public void Wait()
        { 
            foreach (var item in Executing)
                item.TableTask.Wait();
        }


        private void ProcessExecuting(bool waitAll = false)
        {
            if (waitAll)
                Wait();

            var items = Executing.Where(q => q.TableTask.IsCompleted).ToList();

            foreach (var item in items)
            {
                item.ProcessResults();

                Retrieved.AddRange(item.RetrieveResults);

                Errors.AddRange(item.Errors);

                SuccessCount += item.SuccessCount;

                Executing.Remove(item);
            }
        }


    }
}
