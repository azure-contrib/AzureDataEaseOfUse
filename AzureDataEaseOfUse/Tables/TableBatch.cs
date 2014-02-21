using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse.Tables
{
    // ref: http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storage.table.tablebatchoperation.aspx

    /// <summary>
    /// A batch operation may contain up to 100 individual table operations, 
    /// with the requirement that each operation entity must have same partition key. 
    /// A batch with a retrieve operation cannot contain any other operations. 
    /// Note that the total payload of a batch operation is limited to 4MB.
    /// </summary>
    public class TableBatch<T> where T : TableEntity, IAzureStorageTable
    {
        public TableBatch(FlywheelOperation<T> firstItem)
        {
            this.PartitionKey = firstItem.PartitionKey;
            this.IsChange = firstItem.IsChange;

            Include(firstItem);
        }

        
        public readonly string PartitionKey;
        public readonly bool IsChange;


        List<FlywheelOperation<T>> Batches = new List<FlywheelOperation<T>>();

        public void Include(FlywheelOperation<T> operation)
        {
            if (CanNotInclude(operation))
                return;

            Batches.Add(operation);
        }

        public TableBatchOperation GetBatchOperation()
        {
            var batch = new TableBatchOperation();

            foreach (var item in Batches)
                batch.Add(item.Operation);

            return batch;
        }

        public List<FlywheelOperation<T>> GetFlywheelOperations()
        {
            return Batches.ToList();
        }

        #region State Checks

        public int Count { get { return Batches.Count; } }

        public bool IsFull()
        {
            return Count == 100;
        }

        public bool IsNotFull()
        {
            return !IsFull();
        }

        /// <summary>
        /// If batch isn't full, matches class (changes/retrieve), and partition key.
        /// </summary>
        public bool CanInclude(FlywheelOperation<T> operation)
        {
            return (
                operation.IsChange == IsChange &&
                operation.PartitionKey == PartitionKey &&
                Batches.Count < 100);
        }

        public bool CanNotInclude(FlywheelOperation<T> operation)
        {
            return !CanInclude(operation);
        }

        #endregion

    }
}
