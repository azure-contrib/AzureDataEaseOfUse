using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{

    public interface IConnectionManager
    {
        CloudTable GetTableConnection(string name);

        Task<TableResult> TableExecute(string tableName, TableOperation operation);

        Task<IList<TableResult>> TableExecute(string tableName, TableBatchOperation batch);
    }

}
