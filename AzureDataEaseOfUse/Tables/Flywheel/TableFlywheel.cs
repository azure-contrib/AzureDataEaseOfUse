using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse;

namespace AzureDataEaseOfUse.Tables
{
    /// <summary>
    /// Used to process a flow of table batches.
    /// Abstracting the user from the batch creation mechanics.
    /// Executing batches asynchronously when they fill up (100) or when Flush() is called.
    /// Order of execution is not guaranteed.
    /// </summary>
    public class TableFlywheel<T> : IDisposable where T : AzureDataTableEntity<T>, new()
    {
        public TableFlywheel(TableManager<T> tableManager)
        {
            this.TableManager = tableManager;
        }

        public readonly TableManager<T> TableManager;

        private readonly Dictionary<string, List<TableBatch<T>>> Pending = new Dictionary<string, List<TableBatch<T>>>();

        private readonly List<TableBatchResult> Errors = new List<TableBatchResult>();

        private readonly List<Task> Processing = new List<Task>(); 


        #region HasErrors & Counts

        public long ExecutedBatchCount { get; private set; }
        public long ExecutedCount { get; private set; }

        private long _SuccessCount = 0;
        private long _FailureCount = 0;

        public long SuccessCount
        {
            get { return Interlocked.Read(ref _SuccessCount); }
        }

        public long FailureCount {
            get { return Interlocked.Read(ref _FailureCount); }
        }

        public bool HasErrors { get { return FailureCount > 0; } }

        public int PendingPartitionCount()
        {
            return Pending.Values.Count;
        }

        public int PendingBatchCount()
        {
            return Pending.Sum(partition => partition.Value.Count);
        }

        public int PendingCount()
        {
            return Pending.Values.Sum(partition => partition.Sum(batch => batch.Count));
        }

        public int ProcessingCount()
        {
            return Processing.Count;
        }

        #endregion

        #region Aggregators for Change Operations

        // These should probably be moved to static helper

        public TableFlywheel<T> Insert(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => Insert(i));

            return this;
        }

        public TableFlywheel<T> Replace(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => Replace(i));

            return this;
        }

        public TableFlywheel<T> Delete(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => Delete(i));

            return this;
        }

        public TableFlywheel<T> InsertOrReplace(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => InsertOrReplace(i));

            return this;
        }

        public TableFlywheel<T> Merge(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => Merge(i));

            return this;
        }

        public TableFlywheel<T> InsertOrMerge(IEnumerable<T> items)
        {
            items.ToList().ForEach(i => InsertOrMerge(i));

            return this;
        }

        #endregion

        #region Insert, Replace, Delete, & Merge Operations

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

            if (batch.IsFull())
                Flush(batch);

            return this;
        }

        private List<TableBatch<T>> GetFlywheelPartition(FlywheelOperation<T> request)
        {
            return GetFlywheelPartition(request.PartitionKey);
        }

        private List<TableBatch<T>> GetFlywheelPartition(TableBatch<T> batch)
        {
            return GetFlywheelPartition(batch.PartitionKey);
        }

        private List<TableBatch<T>> GetFlywheelPartition(string key)
        {
            if (Pending.ContainsKey(key) == false)
                Pending.Add(key, new List<TableBatch<T>>());

            return Pending[key];
        }

        #endregion

        #region Flush (aka execute)

        /// <summary>
        /// Executes all pending batches Async and schedules task continuations for processing results.  Executed Counts are incremented immediately.
        /// </summary>
        public TableFlywheel<T> Flush()
        {
            var batches = new List<TableBatch<T>>();

            foreach (var partition in Pending.Values)
                batches.AddRange(partition);

            batches.ForEach(Flush);

            return this;
        }

        /// <summary>
        /// Executes the batch Async and schedules task continuation for processing results.  Executed Counts are incremented immediately.
        /// </summary>
        private void Flush(TableBatch<T> batch)
        {
            var operation = batch.GetBatchOperation();

            var batchTask = TableManager.Execute(operation);
            var processingTask = batchTask.OnCompletion(Process);


            MoveToProcessing(batch, batchTask, processingTask);
            
            ExecutedBatchCount++;
            ExecutedCount += operation.Count;

        }

        private void MoveToProcessing(TableBatch<T> batch, Task batchTask, Task processingTask)
        {
            var partition = GetFlywheelPartition(batch);

            partition.Remove(batch);

            Processing.Add(batchTask);
            Processing.Add(processingTask);

            CleanUpProcessing();
        }

        private void CleanUpProcessing()
        {
            var completed = Processing.Where(q => q.IsCompleted).ToList();

            foreach (var item in completed)
            {
                Processing.Remove(item);
                item.Dispose();
            }
        }

        /// <summary>
        /// Waits for all executing and processing tasks to complete
        /// </summary>
        public void Wait()
        {
            var task = WhenAll();

            task.Wait();

            CleanUpProcessing();
        }

        /// <summary>
        /// Returns task to detect when all executing and processing tasks are complete
        /// </summary>
        public Task WhenAll()
        {
            return Task.WhenAll(Processing);
        }

        public void FlushAndWait()
        {
            Flush();
            Wait();
        }

        #endregion

        private void Process(Task<TableBatchResult> task)
        {
            // Note: IConnectionManager guarantees that task will return successful with batch result.
            //          It will encapsulate underlying task errors.

            var item = task.Result;

            if (item.HasErrors)
            {
                Errors.Add(item);

                Interlocked.Add(ref _FailureCount, item.Errors.Count);
            }

            Interlocked.Add(ref _SuccessCount, item.Successful.Count);
        }


        public void Dispose()
        {
            CleanUpProcessing();
        }
    }
}
