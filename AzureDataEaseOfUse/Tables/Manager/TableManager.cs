using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public class TableManager<T> : ITableManager<T> where T : AzureDataTableEntity<T>
    {
        public TableManager(IConnectionManager connectionManager)
        {
            _ConnectionManager = connectionManager;
            _TableName = TableExtensions.GetTableName(typeof(T));
            _Flywheel = new TableFlywheel<T>(this);
        }

        #region ConnectionManager, TableName, Flywheel (Properties & Vars)

        private readonly IConnectionManager _ConnectionManager;
        private readonly string _TableName;
        private readonly TableFlywheel<T> _Flywheel;

        public IConnectionManager ConnectionManager
        {
            get { return _ConnectionManager; }
        }

        public string TableName {
            get { return _TableName; }
        }

        public TableFlywheel<T> Flywheel
        {
            get { return _Flywheel; }
        }

        #endregion

        #region Retrieval

        public Task<TableOperationResult<T>> Retrieve(string partitionKey, string rowKey)
        {
            return Execute(TableOperation.Retrieve<T>(partitionKey, rowKey));
        }

        public Task<TableQueryResult<T>> Retrieve(string partitionKey, params string[] rowKeys)
        {
            return Where(q => q.PartitionKey == partitionKey && rowKeys.Contains(q.RowKey));
        }

        public Task<TableQueryResult<T>> List(string partitionKey)
        {
            return Where(q => q.PartitionKey == partitionKey);
        }

        #endregion

        #region Insert, Replace, Delete, & Merge Operations

        public Task<TableOperationResult<T>> Insert(T item)
        {
            item.SetPartitionAndRowKeys();

            return Execute(TableOperation.Insert(item));
        }

        public Task<TableOperationResult<T>> Replace(T item)
        {
            return Execute(TableOperation.Replace(item));
        }

        public Task<TableOperationResult<T>> Delete(T item)
        {
            return Execute(TableOperation.Delete(item));
        }

        public Task<TableOperationResult<T>> InsertOrReplace(T item)
        {
            item.SetPartitionAndRowKeys();

            return Execute(TableOperation.InsertOrReplace(item));
        }

        public Task<TableOperationResult<T>> Merge(T item)
        {
            return Execute(TableOperation.Merge(item));
        }

        public Task<TableOperationResult<T>> InsertOrMerge(T item)
        {
            item.SetPartitionAndRowKeys();

            return Execute(TableOperation.InsertOrMerge(item));
        }

        #endregion

        #region Execute Operations

        public Task<TableQueryResult<T>> Where(Expression<Func<T, bool>> predicate)
        {
            return ConnectionManager.TableQuery(TableName, predicate);
        }

        public Task<TableOperationResult<T>> Execute(TableOperation operation)
        {
            return ConnectionManager.TableExecute<T>(TableName, operation);
        }

        public Task<TableBatchResult> Execute(TableBatchOperation batch)
        {
            return ConnectionManager.TableExecute(TableName, batch);
        }

        #endregion

        #region Aggregators - Operations

        public IList<Task<TableOperationResult<T>>> Execute(params TableOperation[] operations)
        {
            return Execute(operations.AsEnumerable());
        }

        public IList<Task<TableOperationResult<T>>> Execute(IEnumerable<TableOperation> operations)
        {
            var results = new List<Task<TableOperationResult<T>>>();

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
