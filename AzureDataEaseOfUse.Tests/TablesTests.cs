using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using AzureDataEaseOfUse;
using AzureDataEaseOfUse.Tables;

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
        public static void Cleanup() 
        {
            var azure = Storage.Connect();

            foreach (var table in azure.Tables())
                table.Delete(true);
        }

        #endregion

        #region Add, Update, Delete

        [TestMethod]
        public void Can_Add_Item()
        {
            var table = Simulate.Table();

            var result = table.Add(Simulate.Post());

            Assert.AreEqual(204, result.HttpStatusCode);
        }


        [TestMethod]
        public void Can_Update_Item()
        {
            var table = Simulate.Table();
            var original = Simulate.Post();

            table.Add(original);

            var updated = table.Get<ExamplePost>(original);

            updated.Posted = original.Posted.AddDays(3);

            var result = table.Update(updated);

            Assert.AreEqual(204, result.HttpStatusCode);
        }


        [TestMethod]
        public void Can_Delete_Item()
        {
            var table = Simulate.Table();
            var post = Simulate.Post();

            table.Add(post);

            var result = table.Delete(post);

            Assert.AreEqual(204, result.HttpStatusCode);
        }

        #endregion

        #region Get & List

        [TestMethod]
        public void Can_Get_Item()
        {
            var table = Simulate.Table();
            var post = Simulate.Post();

            table.Add(post);

            var result = table.Get<ExamplePost>(post.PartitionKey, post.RowKey);

            Assert.AreEqual(post.Posted, result.Posted);
        }

        [TestMethod]
        public void Can_List_Items()
        {
            var table = Simulate.FilledTable();

            var results = table.List<ExamplePost>("a");

            Assert.AreEqual(3, results.Count);
        }

        #endregion

        #region Search

        [TestMethod]
        public void Can_Search_Items_By_Row_Key_Equals()
        {
            var table = Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2");

            Assert.AreEqual(3, results.Count);
        }


        [TestMethod]
        public void Can_Search_Items_By_Column_Equal()
        {
            var table = Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount == 3);

            Assert.AreEqual(9, results.Count);
        }

        [TestMethod]
        public void Can_Search_Items_By_Column_Range()
        {
            var table = Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.Amount < 4);

            Assert.AreEqual(9, results.Count);
        }


        [TestMethod]
        public void Can_Search_Items_By_Row_And_Column()
        {
            var table = Simulate.FilledTable();

            var results = table.Where<ExamplePost>(q => q.RowKey == "2" && q.Amount < 4);

            Assert.AreEqual(3, results.Count);
        }

        #endregion

        #region Batch

        [TestMethod]
        public void Can_Batch_Add_Items()
        {
            var table = Simulate.Table();
            var batch = table.Batch<ExamplePost>();

            for (int x = 1; x <= 5; x++)
                batch.Add(Simulate.Post("a", x.ToString()));

            var results = batch.Execute();

            foreach(var result in results)
                Assert.AreEqual(201, result.HttpStatusCode);
        }

        [TestMethod]
        public void Can_Batch_Add_Over_100_Items_Same_Partion_Key()
        {
            var table = Simulate.Table();
            var batch = table.Batch<ExamplePost>();

            for (int x = 1; x <= 110; x++)
                batch.Add(Simulate.Post("a", x.ToString()));

            var results = batch.Execute();

            foreach (var result in results)
                Assert.AreEqual(201, result.HttpStatusCode);
        }

        [TestMethod]
        public void Can_Batch_Different_Partitions()
        {
            var table = Simulate.Table();
            var batch = table.Batch<ExamplePost>();

            batch.Add(Simulate.Post("a"));
            batch.Add(Simulate.Post("b"));

            var results = batch.Execute();

            foreach (var result in results)
                Assert.AreEqual(201, result.HttpStatusCode);
        }

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
