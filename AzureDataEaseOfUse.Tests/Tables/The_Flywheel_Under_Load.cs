using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AzureDataEaseOfUse.Tables;

namespace AzureDataEaseOfUse.Tests.Tables
{
    public class The_Flywheel_Under_Load
    {

        #region Setup

        public The_Flywheel_Under_Load()
        {

            Tardis = new TimeMachine();
            ConnectionManager = Simulate.FastReturnConnectionManager<Example>(Tardis);
            Flywheel = new TableManager<Example>(ConnectionManager.Object).Flywheel;
            NewData = new Example() {PartitionKey = "a", RowKey = "b"};
            NewDataDifferentPartition = new Example() {PartitionKey = "b", RowKey = "a"};
            ExistingData = new Example() {PartitionKey = "a", RowKey = "c", ETag = "abc"};
        }

        private readonly Mock<IConnectionManager> ConnectionManager;
        private readonly TableFlywheel<Example> Flywheel;
        private readonly TimeMachine Tardis;
        private readonly Example NewData;
        private readonly Example NewDataDifferentPartition;
        private readonly Example ExistingData;

        private List<Example> NewData_1k
        {
            get { return Simulate.NewDataSet(1000); }
        }

        private List<Example> NewData_100k
        {
            get { return Simulate.NewDataSet(100000); }
        }

        private List<Example> ExistingData_1k
        {
            get { return Simulate.ExistingDataSet(1000); }
        }

        private List<Example> ExistingData_100k
        {
            get { return Simulate.ExistingDataSet(100000); }
        }

        #endregion

        #region Aggregators

        [Fact]
        public void Can_Insert_1k_Together()
        {
            Flywheel.Insert(NewData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        [Fact]
        public void Can_Replace_1k_Together()
        {
            Flywheel.Replace(ExistingData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        [Fact]
        public void Can_Delete_1k_Together()
        {
            Flywheel.Delete(ExistingData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        [Fact]
        public void Can_InsertOrReplace_1k_Together()
        {
            Flywheel.InsertOrReplace(ExistingData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        [Fact]
        public void Can_Merge_1k_Together()
        {
            Flywheel.Merge(ExistingData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        [Fact]
        public void Can_InsertOrMerge_1k_Together()
        {
            Flywheel.InsertOrMerge(ExistingData_1k).FlushAndWait();

            Assert.Equal(1000, Flywheel.SuccessCount);
        }

        #endregion

        [Fact]
        public void Can_Handle_100k_Change_Operations()
        {
            Flywheel.Insert(NewData_100k);

            Flywheel.FlushAndWait();

            Assert.Equal(NewData_100k.Count, Flywheel.SuccessCount);
            Assert.Equal(0, Flywheel.FailureCount);
        }

    }
}
