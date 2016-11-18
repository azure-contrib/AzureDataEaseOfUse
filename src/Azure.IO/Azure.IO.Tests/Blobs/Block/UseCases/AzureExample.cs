using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Runtime.CompilerServices;

namespace Azure.IO.Tests.Blobs.Block.UseCases
{
 


    //public class AzureExample : Repo
    //{
    //    public AzureExample() : base() {}





    //    public IO.Blobs.Block.AsBytes Pngs { get; private set; } //= GetAsBytesAdapter();//("Pngs5");  //new Azure.IO.Blobs.Block.AsBytes(GetIO(Azure.IO.Configuration.ConnectionString("Azure.IO:Emulator"), "Pngs"));


    //    private static IO.Blobs.Block.AsBytes GetAsBytesAdapter([CallerMemberName] string containerName = "")
    //    {
    //        var Source = GetContainer("Azure.IO:Emulator", containerName);
    //        var Container = new Azure.IO.Blobs.AzureContainer(Source);
    //        var IO = new Azure.IO.Blobs.Block.IO(Container);
    //        var Adapter = new Azure.IO.Blobs.Block.AsBytes(IO);

    //        return Adapter;
    //    }

    //    private static CloudBlobContainer GetContainer(string connectionName, string containerName)
    //    {
    //        var account = CloudStorageAccount.Parse(Azure.IO.Configuration.ConnectionString(connectionName));

    //        var client = account.CreateCloudBlobClient();

    //        var container = client.GetContainerReference(containerName);

    //        container.CreateIfNotExists();

    //        return container;
    //    }
    //}

    //public class Repo
    //{
    //    public Repo()
    //    {
    //        foreach (var p in this.GetType().GetProperties())
    //        {
    //            if (p.GetValue(this) == null)
    //            {
    //                var adapter = GetAsBytesAdapter(p.Name);
    //                p.SetValue(this, adapter);
    //            }
    //        }
    //    }

    //    private static IO.Blobs.Block.AsBytes GetAsBytesAdapter([CallerMemberName] string containerName = "")
    //    {
    //        var Source = GetContainer("Azure.IO:Emulator", containerName);
    //        var Container = new Azure.IO.Blobs.AzureContainer(Source);
    //        var IO = new Azure.IO.Blobs.Block.IO(Container);
    //        var Adapter = new Azure.IO.Blobs.Block.AsBytes(IO);

    //        return Adapter;
    //    }

    //    private static CloudBlobContainer GetContainer(string connectionName, string containerName)
    //    {
    //        var account = CloudStorageAccount.Parse(Azure.IO.Configuration.ConnectionString(connectionName));

    //        var client = account.CreateCloudBlobClient();

    //        var container = client.GetContainerReference(containerName.ToLowerInvariant());

    //        container.CreateIfNotExists();

    //        return container;
    //    }
    //}

}
