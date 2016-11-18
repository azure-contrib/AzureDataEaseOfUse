using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IO.Blobs
{
    public class AzureContainer : IContainer
    {
        public AzureContainer(CloudBlobContainer source) { Source = source; }

        private readonly CloudBlobContainer Source;

        public async Task<IBlob> GetBlob(string path)
        {
            var blob = Source.GetBlockBlobReference(path);
            
            return new AzureBlob(blob);
        }
            
            //    => new AzureBlob(await Source.GetBlobReferenceFromServerAsync(path));

        public async Task<List<string>> List()
        {
            throw new NotImplementedException();

            //BlobContinuationToken token = null;

            //var blobs = new List<string>();

            //blobs.AddRange(Source.ListBlobs(null, true).Select(s => s.Uri.ToString().Replace(s.Container.Uri.ToString() + "/", "")));

            //return await Task.FromResult(blobs);
            //do
            //{
            //    var response = await Source.ListBlobsSegmentedAsync(token);
            //    token = response.ContinuationToken;
            //    //blobs.AddRange(response.Results.Select(s => s.Uri.Segments.Last()));
            //    blobs.AddRange(response.Results.Select(s => s.Uri.ToString().Replace(s.Container.Uri.ToString() + "/", "")));
            //}
            //while (token != null);

            //return blobs;

            //return Task.FromResult(blobs);
        }

        public async Task Init() => await Source.CreateIfNotExistsAsync();
        
    }
}
