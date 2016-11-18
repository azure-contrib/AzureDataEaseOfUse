using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Azure.IO.Blobs.Block
{
    public sealed class AsBytes : As
    {

        public AsBytes([CallerMemberName] string containerName = "") : base(containerName) { }

        public AsBytes(string formatString, [CallerMemberName] string containerName = "") : base(formatString, containerName) { }

        public AsBytes(Func<string, string> pathFormatter, [CallerMemberName] string containerName = "") : base(pathFormatter, containerName) { }


        public async Task Save(string path, byte[] value) => await IO.Save(path, value);

        public async Task<byte[]> Load(string path) => await IO.Load(path);

        public async Task Update(string path, Func<byte[], byte[]> change)
        {
            var value = await Load(path);
            var result = change(value);
            await Save(path, result);
        }

        public async Task Update(string path, Action<byte[]> change)
        {
            var value = await Load(path);
            change(value);
            await Save(path, value);
        }

    }
}
