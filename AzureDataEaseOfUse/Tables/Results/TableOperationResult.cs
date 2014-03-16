using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public class TableOperationResult<T> where T : AzureDataTableEntity<T>
    {
        public TableOperationResult(TableOperation operation, TableResult result)
        {
            this.Operation = operation;
            this.Result = result;
        }

        public readonly TableOperation Operation;
        public readonly TableResult Result;


        public bool HasValue { get { return Value != null; } }

        /// <summary>
        /// If successful, returns object from retrieve operation.  Otherwise default(T).
        /// </summary>
        public T Value
        {
            get { return Result.IsSuccessful() ? (T)Result.Result : default(T); }
        }

    }
}
