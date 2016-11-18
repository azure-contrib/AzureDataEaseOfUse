using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IO.Blobs.Block
{
    public interface IIO
    {
        IContainer Container { get; }

        Func<string, string> PathFormatter { get; }

        Task<IBlob> GetBlob(string path);

        Task Save(string path, byte[] value);

        Task<byte[]> Load(string path);

        Task<bool> Exists(string path);

        Task Delete(string path);

        //Task<List<string>> List();

    }
}
