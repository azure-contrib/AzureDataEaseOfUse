using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AzureDataEaseOfUse.Tables;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public class SingleAccountConnectionManager : IConnectionManager
    {

        public SingleAccountConnectionManager(string connectionStringOrName = "DefaultStorageConnection")
        {
            _Account = CloudStorageAccount.Parse(ConnectionExtensions.GetConnectionString(connectionStringOrName));

            _Account.SetupBestPracticies();
        }

        private readonly CloudStorageAccount _Account;

        public CloudStorageAccount Account
        {
            get { return _Account; }
        }

        #region Tables

        public CloudTableClient TableClient()
        {
            return Account.CreateCloudTableClient();
        }

        public CloudTable Table(string name)
        {
            return TableClient().GetTableReference(name);
        }

        public List<Task> CreateTablesIfNotExist(params string[] tableNames)
        {
            var tasks = new List<Task>();

            foreach(var name in tableNames)
                tasks.Add(Table(name).CreateIfNotExistsAsync());

            return tasks;
        }

        public Task<Tables.TableOperationResult<T>> TableExecute<T>(string tableName, TableOperation operation)
            where T : Tables.AzureDataTableEntity<T>
        {
            var table = Table(tableName);

            var task = table.ExecuteAsync(operation);

            var tsc = new TaskCompletionSource<Tables.TableOperationResult<T>>();

            task.ContinueWith((t) => tsc.SetResult(new TableOperationResult<T>(operation, t.IsSuccessful() ? t.Result : null)));

            return tsc.Task;
        }

        public Task<Tables.TableBatchResult> TableExecute(string tableName, TableBatchOperation batch)
        {
            var table = Table(tableName);

            var task = table.ExecuteBatchAsync(batch);

            var tsc = new TaskCompletionSource<Tables.TableBatchResult>();

            task.ContinueWith((t) => tsc.SetResult(new TableBatchResult(batch, t.IsSuccessful() ? t.Result : null)));

            return tsc.Task;
        }

        public Task<Tables.TableQueryResult<T>> TableQuery<T>(string tableName, Expression<Func<T, bool>> predicate) 
            where T : Tables.AzureDataTableEntity<T>, new()
        {
            var table = Table(tableName);

            var task = Task.Run(() => table.CreateQuery<T>().Where(predicate).ToList());

            var tsc = new TaskCompletionSource<Tables.TableQueryResult<T>>();

            task.ContinueWith((t) => tsc.SetResult(new TableQueryResult<T>(predicate, t.IsSuccessful() ? t.Result : new List<T>())));

            return tsc.Task;
        }

        public Task<List<string>> TableNames(string prefix = null)
        {
            var client = TableClient();

            var task = Task.Run(() => client.ListTables(prefix));

            var tsc = new TaskCompletionSource<List<string>>();

            task.ContinueWith((t) => tsc.SetResult(t.Result.Select(i=> i.Name).ToList()));

            return tsc.Task;
            
        }

        #endregion


    }
}
