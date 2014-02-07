using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureDataEaseOfUse
{
    public class AzureData
    {
        public AzureData(string connectionStringName)
        { 
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            Storage =  CloudStorageAccount.Parse(connectionString);

            Tables = Storage.CreateCloudTableClient();
            Queues = Storage.CreateCloudQueueClient();
            Blobs = Storage.CreateCloudBlobClient();       
        }

        public readonly CloudStorageAccount Storage;

        public readonly CloudTableClient Tables;
        public readonly CloudQueueClient Queues;
        public readonly CloudBlobClient Blobs;


    }
}
