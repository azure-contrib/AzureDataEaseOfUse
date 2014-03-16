using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public interface IAzureDataTableEntity : ITableEntity
    {
        string GetPartitionKey();

        string GetRowKey();
    }
}