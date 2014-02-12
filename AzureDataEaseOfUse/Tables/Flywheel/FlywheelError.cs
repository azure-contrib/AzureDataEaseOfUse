using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables.Async
{
    public class FlywheelError<T> where T : TableEntity, IAzureStorageTable
    {

        public FlywheelError(FlywheelResult<T> flywheelResult, FlywheelOperation<T> operation)
        {
            this.FlywheelResult = flywheelResult;

            this.TaskFailed = true;
            this.Operation = operation;
            this.Result = null;

            IsConflict = false;
        }

        public FlywheelError(FlywheelResult<T> flywheelResult, TableResult result)
        {
            this.FlywheelResult = flywheelResult;

            this.TaskFailed = false;
            this.Operation = null;
            this.Result = result;

            this.IsConflict = result.IsConflict();
        }


        public readonly FlywheelResult<T> FlywheelResult;
        public readonly bool TaskFailed;
        public readonly FlywheelOperation<T> Operation;
        public readonly TableResult Result;

        public readonly bool IsConflict;

    }
}
