using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse.Tables.Async
{

    public class FlywheelOperation<T> where T : AzureDataTableEntity<T>
    {
        public FlywheelOperation(T item, TableOperationType operationType)
        {
            item.SyncKeysOnRow();

            this.Item = item;
            this.OperationType = operationType;
            
            this.PartitionKey = item.PartitionKey;
            this.RowKey = item.RowKey;

            switch(operationType)
            {
                case TableOperationType.Insert: this.Operation = TableOperation.Insert(item); break;
                case TableOperationType.Replace: this.Operation = TableOperation.Replace(item); break;
                case TableOperationType.Delete: this.Operation = TableOperation.Delete(item); break;
                
                case TableOperationType.InsertOrMerge: this.Operation = TableOperation.InsertOrMerge(item); break;
                case TableOperationType.InsertOrReplace: this.Operation = TableOperation.InsertOrReplace(item); break;
                case TableOperationType.Merge: this.Operation = TableOperation.Merge(item); break;
                
                default: throw new NotImplementedException("Operation type not supported");
            }
        }

        public  T Item { get; private set; }

        public  TableOperation Operation { get; private set; }
        
        public TableOperationType OperationType { get; private set; }

        public string PartitionKey { get; private set; }

        public string RowKey { get; private set; }
        
    }
}
