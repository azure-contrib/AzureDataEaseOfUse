using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public static class TablesExtensions
    {

        #region Add

        public static TableResult Add<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();

            //TODO: Add check to ensure partition & row key naming conforms to requirements

            var operation = TableOperation.Insert(item);

            var result = table.Execute(operation);

            return result;
        }

        #endregion

        #region Retrieval

        public static T Get<T>(this CloudTable table, IAzureStorageTable item) where T : TableEntity, IAzureStorageTable
        {
            return table.Get<T>(item.GetTableKeys());
        }

        public static T Get<T>(this CloudTable table, string partitionKey, string rowKey) where T : TableEntity, IAzureStorageTable
        {
            var keys = new TableKeys(partitionKey, rowKey);

            return table.Get<T>(keys);
        }

        public static T Get<T>(this CloudTable table, TableKeys keys) where T : TableEntity, IAzureStorageTable
        {
            var operation = TableOperation.Retrieve<T>(keys.PartitionKey, keys.RowKey);

            var value = table.Execute(operation);

            var result = (T)value.Result;

            return result;
        }

        public static List<T> List<T>(this CloudTable table, string partitionKey) where T : TableEntity, IAzureStorageTable, new()
        {
            var results = table.Where<T>(q => q.PartitionKey == partitionKey).ToList();

            return results;
        }

        public static List<T> Where<T>(this CloudTable table, Expression<Func<T, bool>> predicate) where T : TableEntity, IAzureStorageTable, new()
        {
            return table.CreateQuery<T>().Where(predicate).ToList();
        }

        #endregion

        #region Update

        public static TableResult Update<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();
            
            var operation = TableOperation.Replace(item);

            var result = table.Execute(operation);

            return result;
        }

        #endregion

        #region Deletion

        public static TableResult Delete<T>(this CloudTable table, T item) where T : TableEntity, IAzureStorageTable
        {
            item.SyncKeysOnRow();

            var operation = TableOperation.Delete(item);

            var result = table.Execute(operation);

            return result;
        }

        public static bool Delete(this CloudTable table, bool confirm)
        {
            return confirm ? table.DeleteIfExists() : false;
        }

        #endregion

        public static TableBatch<T> Batch<T>(this CloudTable table) where T : TableEntity, IAzureStorageTable
        {
            return new TableBatch<T>(table);
        }

        #region Table(s)

        public static IEnumerable<CloudTable> Tables(this CloudStorageAccount account, string prefix = null)
        {
            var client = account.CreateCloudTableClient();

            var tables = client.ListTables(prefix);

            return tables;
        }


        public static CloudTable Table(this CloudStorageAccount account, string name, bool createIfNotExists = true)
        {
            //TODO: Add check to ensure table name conforms to requirements

            var client = account.CreateCloudTableClient();

            var table = client.GetTableReference(name);

            if (createIfNotExists)
                table.CreateIfNotExists();

            return table;
        }

        #endregion

        /// <summary>
        /// Ensures the PartitionKey & Row Keys are set on item
        /// </summary>
        public static void SyncKeysOnRow<T>(this T item) where T : TableEntity, IAzureStorageTable
        {
            item.GetTableKeys().SyncTo(item);        
        }


    }
}
