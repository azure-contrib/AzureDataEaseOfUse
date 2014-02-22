using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables.Async
{
    public class FlywheelResult<T> where T : TableEntity, IAzureStorageTable
    {
        public FlywheelResult(TableBatch<T> batch, Task<IList<TableResult>> result)
        {
            this.Batch = batch;
            this.TableTask = result;
        }


        public readonly TableBatch<T> Batch;
        public readonly Task<IList<TableResult>> TableTask;

        bool Processed = false;
        public readonly List<FlywheelError<T>> Errors = new List<FlywheelError<T>>();

        public bool HasErrors { get; private set; }
        public int SuccessCount { get; private set; }

        public void ProcessResults()
        {
            if (Processed)
                return;

            TableTask.Wait();

            if (TableTask.IsSuccessful())
                ProcessEach();
            else
                FailAll();

            Processed = true;
        }

        private void ProcessEach()
        {
            foreach (var item in TableTask.Result)
                Process(item);
        }

        private void Process(TableResult item)
        {
            if (item.IsSuccessful())
                SuccessCount++;
            else
                Fail(item); 
        }

        private void Fail(TableResult item)
        {
            HasErrors = true;

            Errors.Add(new FlywheelError<T>(this, item));
        }

        private void FailAll()
        {
            HasErrors = true;

            foreach (var item in Batch.GetFlywheelOperations())
                Errors.Add(new FlywheelError<T>(this, item));
        }

    }
}
