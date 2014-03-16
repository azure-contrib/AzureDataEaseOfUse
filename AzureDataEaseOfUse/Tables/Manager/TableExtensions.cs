using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace AzureDataEaseOfUse.Tables
{
    public static class TableExtensions
    {

        /// <summary>
        /// Ensures the PartitionKey and Row Keys are set on item
        /// </summary>
        public static void SetPartitionAndRowKeys<T>(this T item) where T : AzureDataTableEntity<T>
        {
            var partitionKey = item.GetPartitionKey();
            var rowKey = item.GetRowKey();

            if (string.IsNullOrEmpty(item.PartitionKey))
                item.PartitionKey = partitionKey;

            if (string.IsNullOrEmpty(item.RowKey))
                item.RowKey = rowKey;
        }


        /// <summary>
        /// Returns the TypeName or name override configured via [TableName("OtherName")]
        /// </summary>
        public static string GetTableName<T>(this T item) where T : AzureDataTableEntity<T>
        {
            var info = typeof(T);

            var att = info.GetCustomAttribute<TableNameAttribute>();

            return att == null ? info.Name : att.TableName;
        }


    }
}
