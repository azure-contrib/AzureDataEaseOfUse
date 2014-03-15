using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse
{
    public interface IAzureDataTableEntity : ITableEntity
    {
        string GetPartitionKey();

        string GetRowKey();
    }
}