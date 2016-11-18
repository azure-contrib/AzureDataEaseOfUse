using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Azure.IO.Blobs
{
    public class InMemoryContainer : IContainer
    {
        public InMemoryContainer() : this(new ConcurrentDictionary<string, byte[]>()) { }

        public InMemoryContainer(IDictionary<string, byte[]> source) { Source = source; }

        private readonly IDictionary<string, byte[]> Source;

        public async Task<IBlob> GetBlob(string path)
            => await Task.FromResult<IBlob>(new InMemoryBlob(Source, path));

        public async Task<List<string>> List()
        {
            return await Task.FromResult(Source.Keys.ToList());
        }

        public async Task Init() => await Task.FromResult(0);

    }
}
