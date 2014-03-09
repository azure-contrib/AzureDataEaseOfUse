using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public interface IAzureTableEntity : ITableEntity
    {

        string GetPartitionKey();

        string GetRowKey();

    }
}
