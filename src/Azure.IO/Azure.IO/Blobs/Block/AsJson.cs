using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Azure.IO.Serializers;

namespace Azure.IO.Blobs.Block
{
    public sealed class AsJson<T> : AsSerialized<T, JsonSerializer> where T : class, new()
    {

        public AsJson([CallerMemberName] string containerName = "") : base(containerName) { }

        public AsJson(string formatString, [CallerMemberName] string containerName = "") : base(formatString, containerName) { }

        public AsJson(Func<string, string> pathFormatter, [CallerMemberName] string containerName = "") : base(pathFormatter, containerName) { }

    }
}
