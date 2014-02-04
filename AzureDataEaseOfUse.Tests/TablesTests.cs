using System;
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

        // Items that execute multiple options have to have a delay, otherwise Azure storage gives 400's


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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            System.Threading.Thread.Sleep(3000);
        }

        [ClassCleanup()]
        public static void Cleanup() 
        {

            System.Threading.Thread.Sleep(5000);

            var azure = Storage.Connect();

            foreach (var table in Storage.Connect().Tables())
                table.Delete(true);

            System.Threading.Thread.Sleep(5000);
        }

        #endregion


        [TestMethod]
        public void Can_Add_Item()
        {
            var tableName = Simulate.TableName();

            var post = Simulate.Post();

            var result = Storage.Connect().Table(tableName).Add(post);

            Assert.AreEqual(204, result.HttpStatusCode);
        }


        [TestMethod]
        public void Can_Get_Item()
        {
            var tableName = Simulate.TableName();

            var post = Simulate.Post();

            Storage.Connect().Table(tableName).Add(post);

            var result = Storage.Connect().Table(tableName).Get<ExamplePost>(post.PartitionKey, post.RowKey);

            Assert.AreEqual(post.Posted, result.Posted);
        }

        [TestMethod]
        public void Can_Delete_Item()
        {
            var tableName = Simulate.TableName();

            var post = Simulate.Post();

            Storage.Connect().Table(tableName).Add(post);

            var result = Storage.Connect().Table(tableName).Delete(post);

            Assert.AreEqual(204, result.HttpStatusCode);
        }

        [TestMethod]
        public void Can_List_Items()
        {
            var tableName = Simulate.TableName();

            var table = Storage.Connect().Table(tableName);

            table.Add(Simulate.Post("blog1", "abc"));
            table.Add(Simulate.Post("blog1", "123"));
            table.Add(Simulate.Post("blog2", "xyz"));

            var results = table.List<ExamplePost>("blog1");

            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void Can_Batch_Add_Items()
        {
            var tableName = Simulate.TableName();

            var table = Storage.Connect().Table(tableName);

            var additions = new List<ExamplePost>();

            additions.Add(Simulate.Post("blog1", "abc"));
            additions.Add(Simulate.Post("blog1", "123"));
            additions.Add(Simulate.Post("blog1", "xyz"));

            var results = table.Add(additions);

            foreach(var result in results)
                Assert.AreEqual(201, result.HttpStatusCode);
        }


    }
}
