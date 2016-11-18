using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Azure.IO.Serializers;

namespace Azure.IO.Blobs.Block
{
    public sealed class AsString : As
    {

        public AsString([CallerMemberName] string containerName = "") : base(containerName) { }

        public AsString(string formatString, [CallerMemberName] string containerName = "") : base(formatString, containerName) { }

        public AsString(Func<string, string> pathFormatter, [CallerMemberName] string containerName = "") : base(pathFormatter, containerName) { }


        public async Task Save(string path, string value) => await IO.Save(path, value.ToByteArray());


        public async Task<string> Load(string path) => (await IO.Load(path)).FromByteArray();

        public async Task Update(string path, Func<string, string> change)
        {
            var value = await Load(path);
            var result = change(value);
            await Save(path, result);
        }

    }
}
