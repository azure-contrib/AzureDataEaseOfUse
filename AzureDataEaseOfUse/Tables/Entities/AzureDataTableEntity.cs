using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables
{
    public class AzureDataTableEntity<T> : TableEntity, IAzureDataTableEntity where T : AzureDataTableEntity<T>
    {

        /// <summary>
        /// By default gets the "PartitionKey".  Override if you aren't setting PartitionKey directly.
        /// </summary>
        public virtual string GetPartitionKey()
        {
            return PartitionKey;
        }

        /// <summary>
        /// By default gets the "RowKey".  Override if you aren't setting RowKey directly.
        /// </summary>
        public virtual string GetRowKey()
        {
            return RowKey;
        }

    }
}