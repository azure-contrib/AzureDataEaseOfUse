using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public class TableKeys
    {
        public TableKeys(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public readonly string PartitionKey;
        public readonly string RowKey;

        public void SyncTo<T>(T item) where T : TableEntity, IAzureStorageTable
        {
            item.PartitionKey = PartitionKey;
            item.RowKey = RowKey;
        }

    }
}
