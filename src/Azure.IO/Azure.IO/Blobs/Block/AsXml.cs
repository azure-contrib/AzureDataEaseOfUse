using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Azure.IO.Serializers;

namespace Azure.IO.Blobs.Block
{
    public sealed class AsXml<T> : AsSerialized<T, XmlSerializer> where T : class, new()
    {

        public AsXml([CallerMemberName] string containerName = "") : base(containerName) { }

        public AsXml(string formatString, [CallerMemberName] string containerName = "") : base(formatString, containerName) { }

        public AsXml(Func<string, string> pathFormatter, [CallerMemberName] string containerName = "") : base(pathFormatter, containerName) { }

    }
}
