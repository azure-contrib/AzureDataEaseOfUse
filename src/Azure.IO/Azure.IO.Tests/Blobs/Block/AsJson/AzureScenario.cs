using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IO.Tests.Blobs.Block.AsJson
{
    //public class AzureScenario : Tests
    //{
    //    public AzureScenario()
    //    {
    //        Source =  GetContainer("Azure.IO:Emulator");
    //        Container = new Azure.IO.Blobs.AzureContainer(Source); 
    //        IO = new Azure.IO.Blobs.Block.IO(Container);
    //        Adapter = new Azure.IO.Blobs.Block.AsJson<Contact>(IO);
    //    }

    //    private readonly CloudBlobContainer Source;

    //    protected override async Task<Contact> GetDirectValue(string key) => await Adapter.Load(key);

    //    private static CloudBlobContainer GetContainer(string connectionName)
    //    {
    //        var account = CloudStorageAccount.Parse(Azure.IO.Configuration.ConnectionString(connectionName));

    //        var client = account.CreateCloudBlobClient();

    //        var container = client.GetContainerReference($"c{Guid.NewGuid().ToString("N")}");

    //        container.CreateIfNotExists();

    //        return container;
    //    }

    //}
}
