using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Azure.IO.Blobs.Block
{
    public sealed class IO : IIO
    {
        public IO(IContainer container, Func<string, string> pathFormatter)
        {
            Container = container;
            Container.Init();

            PathFormatter = pathFormatter;
        }

        public IContainer Container { get; }

        public Func<string, string> PathFormatter { get; }

        public async Task Save(string path, byte[] value) => await (await GetBlob(path)).Upload(value);

        public async Task<byte[]> Load(string path) => await (await GetBlob(path)).Download();


        public async Task<bool> Exists(string path) => await (await GetBlob(path)).Exists();

        public async Task Delete(string path) => await (await GetBlob(path)).Delete();


        //public async Task<List<string>> List() => await Container.List();

        public async Task<IBlob> GetBlob(string path) => await Container.GetBlob(PathFormatter(path));

    }
}
