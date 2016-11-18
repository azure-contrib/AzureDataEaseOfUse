using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Blobs
{
    public class InMemoryBlob : IBlob
    {
        

        public InMemoryBlob(IDictionary<string, byte[]> source, string key)
        {
            Source = source;
            Key = key;
        }

        private readonly IDictionary<string, byte[]> Source;
        private readonly string Key;


        public async Task Delete()
        {
            if(await Exists())
                Source.Remove(Key);
        }

        public async Task<byte[]> Download()
        {
            return await Task.FromResult(Source[Key]); 
        }

        public async Task<bool> Exists()
        {
            return await Task.FromResult(Source.ContainsKey(Key));
        }

        public async Task Upload(byte[] value)
        {
            Source[Key] = value;
            await Task.FromResult(0);
        }
    }
}
