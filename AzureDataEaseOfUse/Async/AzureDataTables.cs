using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace AzureDataEaseOfUse.Async
{
    public class AzureDataTables
    {

        //public async void Get<T>(CloudTable table, TableKeys keys) where T : TableEntity, IAzureStorageTable
        //{
        //    var operation = TableOperation.Retrieve<T>(keys.PartitionKey, keys.RowKey);
        //    var value = await table.ExecuteAsync(operation);
        //    var result = (T)value.Result;
        //    return result;
        //}



    }
}
