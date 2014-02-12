using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse.Tables.Async
{

    public class FlywheelOperation<T> where T: TableEntity, IAzureStorageTable
    {
        public FlywheelOperation(T item, TableOperationType operationType)
        {
            item.SyncKeysOnRow();

            this.Item = item;
            this.OperationType = operationType;
            
            this.IsChange = operationType != TableOperationType.Retrieve;
   
            switch(operationType)
            {
                case TableOperationType.Retrieve: this.Operation = TableOperation.Retrieve(item.PartitionKey, item.RowKey); break;
                case TableOperationType.Insert: this.Operation = TableOperation.Insert(item); break;
                case TableOperationType.Replace: this.Operation = TableOperation.Replace(item); break;
                case TableOperationType.Delete: this.Operation = TableOperation.Delete(item); break;
                
                case TableOperationType.InsertOrMerge: this.Operation = TableOperation.InsertOrMerge(item); break;
                case TableOperationType.InsertOrReplace: this.Operation = TableOperation.InsertOrReplace(item); break;
                case TableOperationType.Merge: this.Operation = TableOperation.Merge(item); break;
                
                default: throw new NotImplementedException("Operation type not supported");
            }
        }

        public FlywheelOperation(string partitionKey, string rowKey)
        {
            this.IsChange = false;
            this.OperationType = TableOperationType.Retrieve;
            this.Operation = TableOperation.Retrieve(partitionKey, rowKey);
        }


        public  T Item { get; private set; }
        public  bool IsChange { get; private set; }
        public  TableOperation Operation { get; private set; }
        public TableOperationType OperationType { get; private set; }
        
    }
}
