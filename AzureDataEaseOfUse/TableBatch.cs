using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables;

namespace AzureDataEaseOfUse
{
    public class TableBatch<T> where T : TableEntity, IAzureStorageTable
    {
        public TableBatch(CloudTable table)
        {
            this.Table = table;
        }

        public readonly CloudTable Table;

        Dictionary<string, List<TableBatchOperation>> Directory;

        #region CRUD Operations

        public TableBatch<T> Add(T item)
        {
            item.SyncKeysOnRow();

            var batch = GetBatchOperation(item.GetPartitionKey());

            batch.Insert(item);

            return this;
        }

        public TableBatch<T> Update(T item)
        {
            item.SyncKeysOnRow();

            var batch = GetBatchOperation(item.GetPartitionKey());

            batch.Replace(item);

            return this;      
        }

        public TableBatch<T> Delete(T item)
        {
            item.SyncKeysOnRow();

            var batch = GetBatchOperation(item.GetPartitionKey());

            batch.Delete(item);

            return this;
        }

        public TableBatch<T> Get(string partitionKey, string rowKey)
        {
            var batch = GetBatchOperation(partitionKey);

            batch.Retrieve(partitionKey, rowKey);

            return this;
        }

        #endregion

        #region Batch Directory Management

        private TableBatchOperation GetBatchOperation(string partitionKey)
        {
            EnsureDirectoryPartitionExists(partitionKey);

            var batches = Directory[partitionKey];

            var batch = batches.FirstOrDefault(q => q.Count < 100);

            if (batch == null)
            {
                batch = new TableBatchOperation();
                batches.Add(batch);
            }

            return batch;        
        }

        private void EnsureDirectoryPartitionExists(string partitionKey)
        { 
            if (Directory.ContainsKey(partitionKey) == false)
                Directory.Add(partitionKey, new List<TableBatchOperation>());
        }

        private List<TableBatchOperation> AllBatches()
        {
            var operations = new List<TableBatchOperation>();

            foreach (var group in Directory.Values)
                operations.AddRange(group);

            return operations;
        }


        #endregion

        #region Execute

        public List<TableResult> Execute()
        {
            var batches = AllBatches();
        
            var results = new List<TableResult>();

            foreach (var batch in batches)
            {
                var result = Table.ExecuteBatch(batch);

                results.AddRange(result);
            }

            return results;
        }

        public List<Task<IList<TableResult>>> ExecuteAsync()
        {
            var batches = AllBatches();

            var results = new List<Task<IList<TableResult>>>();

            foreach (var batch in batches)
            {
                var result = Table.ExecuteBatchAsync(batch);

                results.Add(result);
            }

            return results;           
        }

        #endregion






    }
}
