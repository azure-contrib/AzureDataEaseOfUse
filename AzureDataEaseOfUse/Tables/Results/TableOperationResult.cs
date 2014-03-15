using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataEaseOfUse.Tables.Async;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public class TableOperationResult
    {
        public TableOperationResult(TableOperation operation, TableResult result)
        {
            this.Operation = operation;
            this.Result = result;
        }

        public readonly TableOperation Operation;
        public readonly TableResult Result;

        public T Value<T>()
        {
            return Result.IsSuccessful() ? (T) Result.Result : default(T);
        }


    }
}
