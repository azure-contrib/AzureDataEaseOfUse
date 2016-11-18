using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IO.Blobs
{
    public class AzureBlob : IBlob
    {
        public AzureBlob(ICloudBlob cloudBlob) { CloudBlob = cloudBlob; }

        private readonly ICloudBlob CloudBlob;

        public async Task<byte[]> Download()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                await CloudBlob.DownloadToStreamAsync(ms);

                ms.Position = 0;

                return ms.ToArray();
            }
        }

        public async Task Upload(byte[] value) => await CloudBlob.UploadFromByteArrayAsync(value, 0, value.Length);

        public async Task<bool> Exists() => await CloudBlob.ExistsAsync();

        public async Task Delete() => await CloudBlob.DeleteIfExistsAsync();
        
    }
}
