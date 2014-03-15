using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AzureDataEaseOfUse.Tables.Results;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{

    /// <summary>
    /// Expected that all operations will guarantee Task is successful (encapsulating from underlying cancellations/faults)
    /// </summary>
    public interface IConnectionManager
    {


        Task<TableOperationResult> TableExecute(string tableName, TableOperation operation);

        Task<TableBatchResult> TableExecute(string tableName, TableBatchOperation batch);

        Task<TableRetrieveResult<T>> TableRetrieve<T>(string table, TableOperation operation)
            where T : AzureDataTableEntity<T>; 

        Task<TableQueryResult<T>> TableQuery<T>(string tableName, Expression<Func<T, bool>> predicate)
            where T : AzureDataTableEntity<T>;
        //Example: return table.CreateQuery<T>().Where(predicate).ToList();



    }

}
