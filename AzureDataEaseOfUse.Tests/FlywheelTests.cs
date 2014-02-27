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
    [TestClass]
    public class The_Flywheel
    {

        #region Reset Tests

        [ClassCleanup()]
        public async static void Cleanup()
        {
            var azure = Storage.Connect();

            foreach (var table in azure.Tables())
                await table.Delete(true);
        }

        #endregion

        #region Basic Crud

        [TestMethod]
        public void Can_Add() 
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post("1", "1")).Flush();

            Assert.IsFalse(flywheel.HasErrors);
            Assert.AreEqual(1, flywheel.SuccessCount);
        }

        [TestMethod]
        public async Task Can_Update() 
        {
            var flywheel = Simulate.Flywheel().Filled(1, 1);

            var post = await flywheel.Table.Get<ExamplePost>("1", "1");

            post.Amount = 72;

            flywheel.Replace(post).Flush();

            Assert.IsFalse(flywheel.HasErrors);
        }

        [TestMethod]
        public async Task Can_Delete() 
        {
            var flywheel = Simulate.Flywheel().Filled(1, 1);

            var post = await flywheel.Table.Get<ExamplePost>("1", "1");

            flywheel.Delete(post).Flush();

            Assert.IsFalse(flywheel.HasErrors);
        }

        #endregion


        [TestMethod]
        public void Flushes_Batch_When_Full() 
        {
            var flywheel = Simulate.Flywheel();

            for (int x = 1; x <= 100; x++)
                flywheel.Insert(Simulate.Post("1", x.ToString()));

            Assert.AreEqual(0, flywheel.PendingCount());
            Assert.AreEqual(100, flywheel.ExecutingCount());
        }

        [TestMethod]
        public void Processes_Completed_During_AutoFlush()
        {
            var flywheel = Simulate.Flywheel();

            for (int x = 1; x <= 100; x++)
                flywheel.Insert(Simulate.Post("1", x.ToString()));

            flywheel.Wait();

            for (int x = 1; x <= 99; x++)
                flywheel.Insert(Simulate.Post("2", x.ToString()));

            flywheel.Insert(Simulate.Post(2, 100));

            Assert.AreEqual(100, flywheel.ExecutingCount());
            Assert.AreEqual(100, flywheel.SuccessCount);
        }

        [TestMethod]
        public void Can_Process_Multiple_Items_On_Same_Partition()
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post(1, 1));
            flywheel.Insert(Simulate.Post(1, 2));

            flywheel.Flush();

            Assert.AreEqual(2, flywheel.SuccessCount);
        }

        [TestMethod]
        public void Can_Process_Multiple_Items_On_Multiple_Partitions() 
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post(1, 1));
            flywheel.Insert(Simulate.Post(2, 1));

            flywheel.Flush();

            Assert.AreEqual(2, flywheel.SuccessCount);
        
        }

        [TestMethod]
        public void SuccessCount_Increments() 
        {
            var flywheel = Simulate.Flywheel();

            Assert.AreEqual(0, flywheel.SuccessCount);

            flywheel.Insert(Simulate.Post(1, 1)).Flush();

            Assert.AreEqual(1, flywheel.SuccessCount);
        }

        // Can process at (close to) the limit fo 2k/s
        // Can handle "perceived" going over the limit

    }
}
