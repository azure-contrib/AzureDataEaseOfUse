using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Xunit;
using AzureDataEaseOfUse;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse.Tests
{

    public class TableTests : IDisposable
    {


        #region Add, Update, Delete

        [Fact]
        public async Task Can_Add_Item()
        {
            var table = await Simulate.Table();

            var result = await table.Add(Simulate.Post());

            Assert.Equal(204, result.HttpStatusCode);
        }


        [Fact]
        public async Task Can_Update_Item()
        {
            var table = await Simulate.Table();
            var original = Simulate.Post();

            await table.Add(original);

            var updated = await table.Get<ExamplePost>(original);

            updated.Posted = original.Posted.AddDays(3);

            var result = await table.Update(updated);

            Assert.Equal(204, result.HttpStatusCode);
        }


        [Fact]
        public async Task Can_Delete_Item()
        {
            var table = await Simulate.Table();
            var post = Simulate.Post();

            await table.Add(post);
            
            var result = await table.Delete(post);

            Assert.Equal(204, result.HttpStatusCode);
        }

        #endregion

        #region Get & List

        [Fact]
        public async Task Can_Get_Item()
        {
            var table = await Simulate.Table();
            var post = Simulate.Post();

            await table.Add(post);

            var result = await table.Get<ExamplePost>(post.PartitionKey, post.RowKey);

            Assert.Equal(post.Posted, result.Posted);
        }

        [Fact]
        public async Task Can_List_Items()
        {
            var table = await Simulate.FilledTable();

            var results = table.List<ExamplePost>("a");

            Assert.Equal(3, results.Count);
        }

        #endregion

        #region Search

        [Fact]
        public async Task Can_Search_Items_By_Row_Key_Equals()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2");

            Assert.Equal(3, results.Count);
        }


        [Fact]
        public async Task Can_Search_Items_By_Column_Equal()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount == 3);

            Assert.Equal(9, results.Count);
        }

        [Fact]
        public async Task Can_Search_Items_By_Column_Range()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount < 4);

            Assert.Equal(9, results.Count);
        }


        [Fact]
        public async Task Can_Search_Items_By_Row_And_Column()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2" && q.Amount < 4);

            Assert.Equal(3, results.Count);
        }

        #endregion

        [Fact]
        public void SyncKeysOnRow_Works()
        {
            var post = Simulate.Post();

            post.SyncKeysOnRow();

            Assert.Equal(post.BlogId, post.PartitionKey);
            Assert.Equal(post.Title, post.RowKey);
        }


        public void Dispose()
        {
            Simulate.CleanUp();
        }
    }
}
