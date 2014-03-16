using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataEaseOfUse.Tables;
using Xunit;
using Moq;

namespace AzureDataEaseOfUse.Tests.Tables
{
    public class The_Flywheel_When_Failing
    {

        #region Setup

        public The_Flywheel_When_Failing()
        {
            
            Tardis = new TimeMachine();
            NewData = new Example(){PartitionKey = "a", RowKey = "b"};
            NewDataDifferentPartition = new Example() { PartitionKey = "b", RowKey = "a" };
            ExistingData = new Example() { PartitionKey = "a", RowKey = "c", ETag = "abc" };
        }

        private Mock<IConnectionManager> ConnectionManager { get; set; }
        private TableFlywheel<Example> Flywheel { get; set; }
        private readonly TimeMachine Tardis;
        private readonly Example NewData;
        private readonly Example NewDataDifferentPartition;
        private readonly Example ExistingData;

        #endregion

        [Fact]
        public void Reports_Errors()
        {
            ConnectionManager = Simulate.FailingConnectionManager<Example>(Tardis);
            Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);

            Tardis.ExecuteInContext(advancer =>
            {
                Flywheel.Insert(NewData);

                Flywheel.Flush();

                advancer.AdvanceToCurrent();

                Assert.True(Flywheel.HasErrors);
                Assert.Equal(1, Flywheel.FailureCount);
            });
        }

        [Fact]
        public void Reports_Errors_And_Successes_From_Same_Batch()
        {
            ConnectionManager = Simulate.EvenSuccessOddFailingConnectionManager(Tardis);
            Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);

            Tardis.ExecuteInContext(advancer =>
            {
                var items = Simulate.NewDataSet(100);

                foreach (var item in items)
                    Flywheel.Insert(item);

                advancer.AdvanceToCurrent();

                Assert.True(Flywheel.HasErrors);
                Assert.Equal(50, Flywheel.FailureCount);
                Assert.Equal(50, Flywheel.SuccessCount);
            });
        }

        #region not needed do to IConnectionManager Guarantees

        //[Fact]
        //public void Reports_Errors_When_Task_Is_Cancelled()
        //{
        //    ConnectionManager = Simulate.CancellingTaskConnectionManager(Tardis);
        //    Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);

        //    Tardis.ExecuteInContext(advancer =>
        //    {
        //        Flywheel.Insert(NewData);

        //        var tasks = Flywheel.Flush().ToArray();

        //        advancer.AdvanceToCurrent();

        //        Task.WaitAll(tasks);

        //        Assert.True(Flywheel.HasErrors);
        //        Assert.Equal(1, Flywheel.FailureCount);
        //    });
        //}


        //[Fact]
        //public void Reports_Errors_When_Task_Has_Exception_Thrown()
        //{
        //    ConnectionManager = Simulate.FaultingTaskConnectionManager(Tardis);
        //    Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);

        //    Tardis.ExecuteInContext(advancer =>
        //    {
        //        Flywheel.Insert(NewData);

        //        var tasks = Flywheel.Flush();

        //        advancer.AdvanceToCurrent();

        //        Task.WaitAll(tasks.ToArray());

        //        Assert.True(Flywheel.HasErrors);
        //        Assert.Equal(1, Flywheel.FailureCount);
        //    });
        //}


        //[Fact]
        //public void Fails_Entire_Batch_When_Exception_Is_Thrown()
        //{
        //    ConnectionManager = Simulate.FaultingTaskConnectionManager(Tardis);
        //    Flywheel = Simulate.TableFlywheel<Example>(ConnectionManager, Tardis);

        //    Tardis.ExecuteInContext(advancer =>
        //    {
        //        var items = Simulate.NewDataSet(100);

        //        foreach (var item in items)
        //            Flywheel.Insert(item);

        //        advancer.AdvanceToCurrent();

        //        Assert.True(Flywheel.HasErrors);
        //        Assert.Equal(100, Flywheel.FailureCount);
        //    });
        //}

        #endregion

    }
}
