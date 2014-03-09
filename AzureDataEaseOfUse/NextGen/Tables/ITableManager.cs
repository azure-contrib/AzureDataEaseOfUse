using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataEaseOfUse.NextGen;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public interface ITableManager<T> where T : IAzureDataTableEntity
    {

        string GetTableName();
        
        IConnectionManager ConnectionManager { get; }

        Task<TableResult> Execute(TableOperation operation);

        IList<Task<TableResult>> Execute(params TableOperation[] operations);

        IList<Task<TableResult>> Execute(IEnumerable<TableOperation> operations);

        Task<IList<TableResult>> Execute(TableBatchOperation batch);

        IList<Task<IList<TableResult>>> Execute(params TableBatchOperation[] batches);

        IList<Task<IList<TableResult>>> Execute(IEnumerable<TableBatchOperation> batches);

    }

}
