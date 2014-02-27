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
    
    public class The_Flywheel :IDisposable
    {

        #region Basic Crud

        [Fact]
        public void Can_Add() 
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post("1", "1")).Flush();

            Assert.False(flywheel.HasErrors);
            Assert.Equal(1, flywheel.SuccessCount);
        }

        [Fact]
        public async Task Can_Update() 
        {
            var flywheel = Simulate.Flywheel().Filled(1, 1);

            var post = await flywheel.Table.Get<ExamplePost>("1", "1");

            post.Amount = 72;

            flywheel.Replace(post).Flush();

            Assert.False(flywheel.HasErrors);
        }

        [Fact]
        public async Task Can_Delete() 
        {
            var flywheel = Simulate.Flywheel().Filled(1, 1);

            var post = await flywheel.Table.Get<ExamplePost>("1", "1");

            flywheel.Delete(post).Flush();

            Assert.False(flywheel.HasErrors);
        }

        #endregion


        [Fact]
        public void Flushes_Batch_When_Full() 
        {
            var flywheel = Simulate.Flywheel();

            for (int x = 1; x <= 100; x++)
                flywheel.Insert(Simulate.Post("1", x.ToString()));

            Assert.Equal(0, flywheel.PendingCount());
            Assert.Equal(100, flywheel.ExecutingCount());
        }

        [Fact]
        public void Processes_Completed_During_AutoFlush()
        {
            var flywheel = Simulate.Flywheel();

            for (int x = 1; x <= 100; x++)
                flywheel.Insert(Simulate.Post("1", x.ToString()));

            flywheel.Wait();

            for (int x = 1; x <= 99; x++)
                flywheel.Insert(Simulate.Post("2", x.ToString()));

            flywheel.Insert(Simulate.Post(2, 100));

            Assert.Equal(100, flywheel.ExecutingCount());
            Assert.Equal(100, flywheel.SuccessCount);
        }

        [Fact]
        public void Can_Process_Multiple_Items_On_Same_Partition()
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post(1, 1));
            flywheel.Insert(Simulate.Post(1, 2));

            flywheel.Flush();

            Assert.Equal(2, flywheel.SuccessCount);
        }

        [Fact]
        public void Can_Process_Multiple_Items_On_Multiple_Partitions() 
        {
            var flywheel = Simulate.Flywheel();

            flywheel.Insert(Simulate.Post(1, 1));
            flywheel.Insert(Simulate.Post(2, 1));

            flywheel.Flush();

            Assert.Equal(2, flywheel.SuccessCount);
        
        }

        [Fact]
        public void SuccessCount_Increments() 
        {
            var flywheel = Simulate.Flywheel();

            Assert.Equal(0, flywheel.SuccessCount);

            flywheel.Insert(Simulate.Post(1, 1)).Flush();

            Assert.Equal(1, flywheel.SuccessCount);
        }

        // Can process at (close to) the limit fo 2k/s
        // Can handle "perceived" going over the limit


        public void Dispose()
        {
            Simulate.CleanUp();
        }
    }
}
