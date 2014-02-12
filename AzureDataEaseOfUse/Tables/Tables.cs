using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables;

namespace AzureDataEaseOfUse.Tables.Async
{
    public static class TablesExtensions
    {

        #region Add

        public async static Task<TableResult> Add<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();

            //TODO: Add check to ensure partition & row key naming conforms to requirements

            var operation = TableOperation.Insert(item);

            var result = await table.ExecuteAsync(operation);

            return result;
        }

        #endregion

        #region Retrieval

        public async static Task<T> Get<T>(this CloudTable table, IAzureStorageTable item) where T : TableEntity, IAzureStorageTable
        {
            return await table.Get<T>(item.GetTableKeys());
        }

        public async static Task<T> Get<T>(this CloudTable table, string partitionKey, string rowKey) where T : TableEntity, IAzureStorageTable
        {
            var keys = new TableKeys(partitionKey, rowKey);

            return await table.Get<T>(keys);
        }

        public async static Task<T> Get<T>(this CloudTable table, TableKeys keys) where T : TableEntity, IAzureStorageTable
        {
            var operation = TableOperation.Retrieve<T>(keys.PartitionKey, keys.RowKey);

            var value = await table.ExecuteAsync(operation);

            var result = (T)value.Result;

            return result;
        }

        /// <summary>
        /// [Synchronous execution]
        /// </summary>
        public static List<T> List<T>(this CloudTable table, string partitionKey) where T : TableEntity, IAzureStorageTable, new()
        {
            var results = table.Where<T>(q => q.PartitionKey == partitionKey).ToList();

            return results;
        }

        /// <summary>
        /// [Synchronous execution]
        /// </summary>
        public static List<T> Where<T>(this CloudTable table, Expression<Func<T, bool>> predicate) where T : TableEntity, IAzureStorageTable, new()
        {
            return table.CreateQuery<T>().Where(predicate).ToList();
        }

        #endregion

        #region Update

        public async static Task<TableResult> Update<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();
            
            var operation = TableOperation.Replace(item);

            var result = await table.ExecuteAsync(operation);

            return result;
        }

        #endregion

        #region Deletion

        public async static Task<TableResult> Delete<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();

            var operation = TableOperation.Delete(item);

            var result = await table.ExecuteAsync(operation);

            return result;
        }

        public async static Task<bool> Delete(this CloudTable table, bool confirm)
        {
            return confirm ? await table.DeleteIfExistsAsync() : false;
        }

        #endregion

        #region Table(s)

        /// <summary>
        /// [Synchronous execution]
        /// </summary>
        public static IEnumerable<CloudTable> Tables(this CloudStorageAccount account, string prefix = null)
        {
            var client = account.CreateCloudTableClient();
            
            var tables = client.ListTables(prefix);

            return tables;
        }


        public async static Task<CloudTable> Table(this CloudStorageAccount account, string name, bool createIfNotExists = true)
        {
            //TODO: Add check to ensure table name conforms to requirements

            var client = account.CreateCloudTableClient();

            var table = client.GetTableReference(name);
            
            if (createIfNotExists)
                await table.CreateIfNotExistsAsync();

            return table;
        }

        #endregion

        public static TableFlywheel<T> Flywheel<T>(this CloudTable table, bool autoFlush = true) where T : TableEntity, IAzureStorageTable
        {
            return new TableFlywheel<T>(table);
        }


        /// <summary>
        /// Ensures the PartitionKey and Row Keys are set on item
        /// </summary>
        public static void SyncKeysOnRow<T>(this T item) where T : TableEntity, IAzureStorageTable
        {
            item.GetTableKeys().SyncTo(item);        
        }


    }
}
