using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Azure.IO.Blobs;

namespace Azure.IO.Tests.Blobs.Block.AsJson
{
    public abstract class Tests
    {
        protected Azure.IO.Blobs.Block.IIO IO;
        protected Azure.IO.Blobs.IContainer Container;
        protected Azure.IO.Blobs.Block.AsJson<Contact> Adapter;

        private readonly Contact Data = new Contact() { FirstName = "John", LastName = "Doe" };
        private string Path = Guid.NewGuid().ToString("N");

        protected abstract Task<Contact> GetDirectValue(string key);


        [Fact]
        public async Task Save()
        {
            await Adapter.Save(Path, Data);

            Assert.Equal(Data, await GetDirectValue(Path));
        }

        [Fact]
        public async Task Load()
        {
            await Adapter.Save(Path, Data);

            var result = await Adapter.Load(Path);

            Assert.Equal(Data, result);
        }

        [Fact]
        public async Task Exists()
        {
            await Adapter.Save(Path, Data);

            Assert.True(await Adapter.Exists(Path));
        }

        [Fact]
        public async Task Delete()
        {
            await Adapter.Save(Path, Data);

            await Adapter.Delete(Path);

            Assert.False(await Adapter.Exists(Path));
        }

        //[Fact]
        //public async Task List()
        //{
        //    await Adapter.Save(Path, Data);

        //    var results = await Adapter.List();

        //    Assert.Equal(1, results.Count);
        //    Assert.Equal(Adapter.FormatPath(Path), results.First());
        //}

        [Fact]
        public async Task UpdateAsFunction()
        {
            var data2 = new Contact() { FirstName = "Jane", LastName = "Doe" };

            await Adapter.Save(Path, Data);

            await Adapter.Update(Path, (x) => data2);

            var result = await Adapter.Load(Path);

            Assert.Equal(data2, result);
        }

        [Fact]
        public async Task UpdateAsAction()
        {
            await Adapter.Save(Path, Data);

            await Adapter.Update(Path, (x) => x.FirstName = "Jane");

            var result = await Adapter.Load(Path);

            Assert.Equal("Jane", result.FirstName);

        }

    }
}
