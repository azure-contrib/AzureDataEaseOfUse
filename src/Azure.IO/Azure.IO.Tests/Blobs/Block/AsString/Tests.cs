using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Azure.IO.Tests.Blobs.Block.AsString
{
    public abstract class Tests
    {
        public Tests(Azure.IO.Blobs.Block.AsString adapter) { Adapter = adapter; }

        private readonly Azure.IO.Blobs.Block.AsString Adapter;
        private string Path = Guid.NewGuid().ToString("N");
        private readonly string Data = "abc";


        [Fact]
        public async Task Save()
        {
            await Adapter.Save(Path, Data);

            Assert.Equal(Data, await Adapter.Load(Path));
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

        [Fact]
        public async Task UpdateAsFunction()
        {
            var data2 = "abcd";

            await Adapter.Save(Path, Data);

            await Adapter.Update(Path, (x) => data2);

            var result = await Adapter.Load(Path);

            Assert.Equal(data2, result);
        }

    }
}
