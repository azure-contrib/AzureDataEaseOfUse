using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Azure.IO.Serializers;

namespace Azure.IO.Blobs.Block
{
    public sealed class AsCsv<T> : AsSerialized<T, CsvSerializer> where T : class, new()
    {
        
        public AsCsv([CallerMemberName] string containerName = "") : base(containerName) { }

        public AsCsv(string formatString, [CallerMemberName] string containerName = "") : base(formatString, containerName) { }

        public AsCsv(Func<string, string> pathFormatter, [CallerMemberName] string containerName = "") : base(pathFormatter, containerName) { }

    }
}
