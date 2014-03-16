using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public class TableBatchResult
    {
        public TableBatchResult(TableBatchOperation batch, IList<TableResult> results)
        {
            this.Batch = batch;
            this.Results = results!=null ? results.ToList() : new List<TableResult>();

            foreach (var result in this.Results)
            {
                if (result.IsSuccessful())
                    Successful.Add(result);
                else
                    Errors.Add(result);
            }

            HasErrors = Errors.Count > 0;
        }

        public readonly TableBatchOperation Batch;
        public readonly List<TableResult> Results;
        public readonly List<TableResult> Successful = new List<TableResult>(); 
        public readonly List<TableResult> Errors = new List<TableResult>(); 

        public readonly bool HasErrors;
    }
}
