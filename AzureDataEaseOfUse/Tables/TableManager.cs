using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public class TableManager<T> : ITableManager<T> where T : IAzureDataTableEntity
    {
        public TableManager(IConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public IConnectionManager ConnectionManager { get; private set; }

        public string GetTableName()
        {
            var info = typeof (T);
            
            var att = info.GetCustomAttribute<TableNameAttribute>();

            return att == null ? info.Name : att.TableName;
        }


        #region Query & Retrieval

        public Task<TableOperationResult> Retrieve(string partitionKey, string rowKey)
        {
            return Execute(TableOperation.Retrieve<T>(partitionKey, rowKey));
        }

        public void Where(Expression<Func<T, bool>> predicate)
        {
            var query = new TableQuery<T>();
        }




        #endregion




        #region Insert, Replace, Delete, & Merge Operations

        public Task<TableOperationResult> Insert(T item)
        {
            return Execute(TableOperation.Insert(item));
        }

        public Task<TableOperationResult> Replace(T item)
        {
            return Execute(TableOperation.Replace(item));
        }

        public Task<TableOperationResult> Delete(T item)
        {
            return Execute(TableOperation.Delete(item));
        }

        public Task<TableOperationResult> InsertOrReplace(T item)
        {
            return Execute(TableOperation.InsertOrReplace(item));
        }

        public Task<TableOperationResult> Merge(T item)
        {
            return Execute(TableOperation.Merge(item));
        }

        public Task<TableOperationResult> InsertOrMerge(T item)
        {
            return Execute(TableOperation.InsertOrMerge(item));
        }

        #endregion

        #region Execute Operations

        public Task<TableOperationResult> Execute(TableOperation operation)
        {
            var tableName = GetTableName();

            return ConnectionManager.TableExecute(tableName, operation);
        }

        public Task<TableBatchResult> Execute(TableBatchOperation batch)
        {
            var tableName = GetTableName();

            return ConnectionManager.TableExecute(tableName, batch);
        }

        #endregion

        #region Aggregators - Operations

        public IList<Task<TableOperationResult>> Execute(params TableOperation[] operations)
        {
            return Execute(operations.AsEnumerable());
        }

        public IList<Task<TableOperationResult>> Execute(IEnumerable<TableOperation> operations)
        {
            var results = new List<Task<TableOperationResult>>();

            foreach (var operation in operations)
                results.Add(Execute(operation));

            return results;
        }

        #endregion

        #region Aggregators - Batches

        public IList<Task<TableBatchResult>> Execute(params TableBatchOperation[] batches)
        {
            return Execute(batches.AsEnumerable());
        }

        public IList<Task<TableBatchResult>> Execute(IEnumerable<TableBatchOperation> batches)
        {
            return batches.Select(batch => Execute(batch)).ToList();
        }

        #endregion

    }

}
