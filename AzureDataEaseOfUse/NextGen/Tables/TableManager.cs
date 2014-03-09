using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AzureDataEaseOfUse.NextGen;
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


        #region Insert, Replace, Delete, & Merge Operations

        public Task<TableResult> Insert(T item)
        {
            return Execute(TableOperation.Insert(item));
        }

        public Task<TableResult> Replace(T item)
        {
            return Execute(TableOperation.Replace(item));
        }

        public Task<TableResult> Delete(T item)
        {
            return Execute(TableOperation.Delete(item));
        }

        public Task<TableResult> InsertOrReplace(T item)
        {
            return Execute(TableOperation.InsertOrReplace(item));
        }

        public Task<TableResult> Merge(T item)
        {
            return Execute(TableOperation.Merge(item));
        }

        public Task<TableResult> InsertOrMerge(T item)
        {
            return Execute(TableOperation.InsertOrMerge(item));
        }

        #endregion

        #region Execute Operations

        public Task<TableResult> Execute(TableOperation operation)
        {
            var tableName = GetTableName();

            return ConnectionManager.TableExecute(tableName, operation);
        }

        public Task<IList<TableResult>> Execute(TableBatchOperation batch)
        {
            var tableName = GetTableName();

            return ConnectionManager.TableExecute(tableName, batch);
        }

        #endregion

        #region Aggregators - Operations

        public IList<Task<TableResult>> Execute(params TableOperation[] operations)
        {
            return Execute(operations.AsEnumerable());
        }

        public IList<Task<TableResult>> Execute(IEnumerable<TableOperation> operations)
        {
            var results = new List<Task<TableResult>>();

            foreach (var operation in operations)
                results.Add(Execute(operation));

            return results;
        }

        #endregion

        #region Aggregators - Batches

        public IList<Task<IList<TableResult>>> Execute(params TableBatchOperation[] batches)
        {
            return Execute(batches.AsEnumerable());
        }

        public IList<Task<IList<TableResult>>> Execute(IEnumerable<TableBatchOperation> batches)
        {
            var results = new List<Task<IList<TableResult>>>();

            foreach (var batch in batches)
                results.Add(Execute(batch));

            return results;
        }

        #endregion

    }

}
