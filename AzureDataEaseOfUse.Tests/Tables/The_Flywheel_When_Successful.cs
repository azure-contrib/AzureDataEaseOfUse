using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureDataEaseOfUse.NextGen7.Tests;
using AzureDataEaseOfUse.Tables.Async;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;
using Moq;

namespace AzureDataEaseOfUse.Tests
{
    public class The_Flywheel_When_Successful
    {

        #region Setup

        public The_Flywheel_When_Successful()
        {
            
            Tardis = new TimeMachine();
            ConnectionManager = Simulate.SuccessfulConnectionManager(Tardis);
            Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);
            NewData = new Example(){PartitionKey = "a", RowKey = "b"};
            NewDataDifferentPartition = new Example() { PartitionKey = "b", RowKey = "a" };
            ExistingData = new Example() { PartitionKey = "a", RowKey = "c", ETag = "abc" };
        }

        private readonly Mock<IConnectionManager> ConnectionManager;
        private readonly TableFlywheel<Example> Flywheel;
        private readonly TimeMachine Tardis;
        private readonly Example NewData;
        private readonly Example NewDataDifferentPartition;
        private readonly Example ExistingData;

        #endregion

        [Fact]
        public void Can_Create_Flywheel()
        {
            var tableManager = Simulate.TableManager<Example>(ConnectionManager, Tardis);
            var flywheel = new TableFlywheel<Example>(tableManager);

            Assert.NotNull(flywheel);
        }


        #region Change Operations

        [Fact]
        public void Can_Insert()
        {
            var result = Flywheel.Insert(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Replace()
        {
            var result = Flywheel.Replace(ExistingData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Delete()
        {
            var result = Flywheel.Delete(ExistingData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrReplace_New_Data()
        {
            var result = Flywheel.InsertOrReplace(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrReplace_Existing_Data()
        {
            var result = Flywheel.InsertOrReplace(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Merge()
        {
            var result = Flywheel.Merge(ExistingData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrMerge_New_Data()
        {
            var result = Flywheel.InsertOrMerge(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrMerge_Existing_Data()
        {
            var result = Flywheel.InsertOrMerge(ExistingData);

            Assert.NotNull(result);
        }

        #endregion


        [Fact]
        public void Pending_Counts_Update()
        {
            Assert.Equal(0, Flywheel.PendingCount());

            Flywheel.Insert(NewData);

            Assert.Equal(1, Flywheel.PendingCount());
            Assert.Equal(1, Flywheel.PendingBatchCount());
            Assert.Equal(1, Flywheel.PendingPartitionCount());
        }

        [Fact]
        public void Item_Partitions_Are_Separated()
        {
            Flywheel.Insert(NewData);
            Flywheel.Insert(NewDataDifferentPartition);

            Assert.Equal(2, Flywheel.PendingPartitionCount());
        }

        [Fact]
        public void Items_Are_Batched_Together_In_Same_Partition()
        {
            Flywheel.Insert(NewData);
            Flywheel.Replace(ExistingData);

            Assert.Equal(1, Flywheel.PendingBatchCount());
        }

        [Fact]
        public void Auto_Flushes_A_Batch_When_Full()
        {
            var items = Simulate.NewDataSet(100);

            foreach (var item in items)
                Flywheel.Insert(item);

            Assert.Equal(0, Flywheel.PendingCount());
            Assert.Equal(100, Flywheel.ExecutedCount);
        }

        [Fact]
        public void Auto_Processes_When_Operation_Completes()
        {
            Tardis.ExecuteInContext(advancer =>
            {
                Flywheel.Insert(NewData);

                Flywheel.Flush();

                advancer.AdvanceToCurrent();

                Flywheel.Wait();

                Assert.Equal(0, Flywheel.PendingCount());
                Assert.Equal(1, Flywheel.ExecutedCount);
                Assert.Equal(1, Flywheel.SuccessCount);
            });
        }


        [Fact]
        public void Processing_Tasks_Are_CleanedUp_On_Wait()
        {
            Tardis.ExecuteInContext(advancer =>
            {
                Flywheel.Insert(NewData).Flush();

                advancer.AdvanceToCurrent();

                Assert.Equal(2, Flywheel.ProcessingCount());

                Flywheel.Wait();

                Assert.Equal(0, Flywheel.ProcessingCount());
            });
        }

    }
}
