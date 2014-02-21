using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using AzureDataEaseOfUse;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse.Tests
{
    /// <summary>
    /// Summary description for StorageTables
    /// </summary>
    [TestClass]
    public class TableTests
    {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Reset Tests

        [ClassCleanup()]
        public async static void Cleanup() 
        {
            var azure = Storage.Connect();

            foreach (var table in azure.Tables())
                await table.Delete(true);
        }

        #endregion

        #region Add, Update, Delete

        [TestMethod]
        public async Task Can_Add_Item()
        {
            var table = await Simulate.Table();

            var result = await table.Add(Simulate.Post());

            Assert.AreEqual(204, result.HttpStatusCode);
        }


        [TestMethod]
        public async Task Can_Update_Item()
        {
            var table = await Simulate.Table();
            var original = Simulate.Post();

            await table.Add(original);

            var updated = await table.Get<ExamplePost>(original);

            updated.Posted = original.Posted.AddDays(3);

            var result = await table.Update(updated);

            Assert.AreEqual(204, result.HttpStatusCode);
        }


        [TestMethod]
        public async Task Can_Delete_Item()
        {
            var table = await Simulate.Table();
            var post = Simulate.Post();

            await table.Add(post);
            
            var result = await table.Delete(post);

            Assert.AreEqual(204, result.HttpStatusCode);
        }

        #endregion

        #region Get & List

        [TestMethod]
        public async Task Can_Get_Item()
        {
            var table = await Simulate.Table();
            var post = Simulate.Post();

            await table.Add(post);

            var result = await table.Get<ExamplePost>(post.PartitionKey, post.RowKey);

            Assert.AreEqual(post.Posted, result.Posted);
        }

        [TestMethod]
        public async Task Can_List_Items()
        {
            var table = await Simulate.FilledTable();

            var results = table.List<ExamplePost>("a");

            Assert.AreEqual(3, results.Count);
        }

        #endregion

        #region Search

        [TestMethod]
        public async Task Can_Search_Items_By_Row_Key_Equals()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2");

            Assert.AreEqual(3, results.Count);
        }


        [TestMethod]
        public async Task Can_Search_Items_By_Column_Equal()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount == 3);

            Assert.AreEqual(9, results.Count);
        }

        [TestMethod]
        public async Task Can_Search_Items_By_Column_Range()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount < 4);

            Assert.AreEqual(9, results.Count);
        }


        [TestMethod]
        public async Task Can_Search_Items_By_Row_And_Column()
        {
            var table = await Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2" && q.Amount < 4);

            Assert.AreEqual(3, results.Count);
        }

        #endregion

        #region Flywheel (aka Batch)

        //[TestMethod]
        //public async Task Can_Batch_Add_Items()
        //{
        //    var table = await Simulate.Table();
        //    var flywheel = table.Flywheel<ExamplePost>(autoFlush: false);

        //    for (int x = 1; x <= 5; x++)
        //        flywheel.Insert(Simulate.Post("a", x.ToString()));

        //    flywheel.Flush();

        //    Assert.AreEqual(0, flywheel.Errors.Count);
        //    Assert.AreEqual(5, flywheel.SuccessCount);
        //}

        //[TestMethod]
        //public async Task Can_Batch_Add_1000_Items_Same_Partition()
        //{
        //    var flywheel = Simulate.Flywheel();

        //    for (int x = 1; x <= 1000; x++)
        //        flywheel.Insert(Simulate.Post("a", x.ToString()));

        //    foreach (var item in Simulate.Posts(1, 200))
        //        flywheel.Insert(item);

        //    flywheel.Flush();

        //    Assert.AreEqual(0, flywheel.Errors.Count);
        //    Assert.AreEqual(1000, flywheel.SuccessCount);
        //}


        //[TestMethod]
        //public async Task Can_Batch_Add_1000_Items_Over_Mulitiple_Partitions()
        //{
        //    var table = await Simulate.Table();
        //    var flywheel = table.Flywheel<ExamplePost>(autoFlush: false);

        //    foreach (var item in Simulate.Posts(3, 5))
        //        flywheel.Insert(item);

        //    flywheel.Flush();

        //    Assert.AreEqual(0, flywheel.Errors.Count);
        //    Assert.AreEqual(1000, flywheel.SuccessCount);
        //}

        #endregion


        [TestMethod]
        public void SyncKeysOnRow_Works()
        {
            var post = Simulate.Post();

            post.SyncKeysOnRow();

            Assert.AreEqual(post.BlogId, post.PartitionKey);
            Assert.AreEqual(post.Title, post.RowKey);
        }

    }
}
