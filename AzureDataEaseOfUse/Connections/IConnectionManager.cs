using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables;

namespace AzureDataEaseOfUse
{

    /// <summary>
    /// Expected that all operations will guarantee Task is successful (encapsulating from underlying cancellations/faults)
    /// </summary>
    public interface IConnectionManager
    {

        Task CreateTablesIfNotExist(params string[] tableNames);

        Task<TableOperationResult<T>> TableExecute<T>(string tableName, TableOperation operation)
            where T : AzureDataTableEntity<T>;

        Task<TableBatchResult> TableExecute(string tableName, TableBatchOperation batch);


        Task<TableQueryResult<T>> TableQuery<T>(string tableName, Expression<Func<T, bool>> predicate)
            where T : AzureDataTableEntity<T>;
        //Example: return table.CreateQuery<T>().Where(predicate).ToList();

        /// <summary>
        /// Gets a list of table names found in the account
        /// </summary>
        Task<List<string>> TableNames();

    }

}
