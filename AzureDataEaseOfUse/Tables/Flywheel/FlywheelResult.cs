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
            this.Errors = new List<FlywheelError<T>>();
            this.RetrieveResults = new List<T>();
        }


        public readonly TableBatch<T> Batch;
        public readonly Task<IList<TableResult>> TableTask;

        bool Processed = false;
        public readonly List<FlywheelError<T>> Errors;
        public readonly List<T> RetrieveResults;

        public bool HasErrors { get; private set; }
        public int SuccessCount { get; private set; }

        public void ProcessResults()
        {
            if (Processed)
                return;

            TableTask.Wait();

            if (TableTask.IsCanceled || TableTask.IsFaulted)
                FailAllOperations();
            else
                ProcessEach();

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
                Succeed(item);
            else
                Fail(item); 
        }

        private void Succeed(TableResult item)
        {
            SuccessCount++;

            if (Batch.IsChange)
                return;
            
            RetrieveResults.Add((T)item.Result);
        }


        private void FailAllOperations()
        {
            HasErrors = true;
            
            foreach (var item in Batch.GetFlywheelOperations())
                Errors.Add(new FlywheelError<T>(this, item));
        }

        private void Fail(TableResult item)
        {
            HasErrors = true;

            Errors.Add(new FlywheelError<T>(this, item));
        }

    }
}
