using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse
{
    public class ConnectionManager : IConnectionManager
    {

        //TODO: add profiles to handle multiple storage accounts under the hood

        public CloudTable GetTableConnection(string name)
        {
            var client = Storage.Connect().CreateCloudTableClient();

            var table = client.GetTableReference(name);

            return table;
        }

        public Task<TableResult> TableExecute(string tableName, TableOperation operation)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TableResult>> TableExecute(string tableName, TableBatchOperation batch)
        {
            throw new NotImplementedException();
        }


    }
}
