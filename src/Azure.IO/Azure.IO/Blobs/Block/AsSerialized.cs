using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.IO.Serializers;

namespace Azure.IO.Blobs.Block
{
    public abstract class AsSerialized<T, S> : As 
        where T : class, new()
        where S : ISerializer, new()
    { 

        public AsSerialized(string containerName = "") : base(containerName) { }

        public AsSerialized(string formatString, string containerName = "") : base(formatString, containerName) { }

        public AsSerialized(Func<string, string> pathFormatter, string containerName = "") : base(pathFormatter, containerName) { }


        ISerializer Serializer = new S();

        public async Task Save(string path, T value) => await IO.Save(path, Serializer.SerializeObject(value));

        public async Task<T> Load(string path) => Serializer.DeserializeObject<T>(await IO.Load(path));

        public async Task Update(string path, Func<T, T> change)
        {
            var value = await Load(path);
            var result = change(value);
            await Save(path, result);
        }

        public async Task Update(string path, Action<T> change)
        {
            var value = await Load(path);
            change(value);
            await Save(path, value);
        }


    }
}
