using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureDataEaseOfUse.Tables;
using AzureDataEaseOfUse.Tests;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace AzureDataEaseOfUse.Tests.Tables
{
    public static class Simulate
    {
        public static TableManager<T> TableManager<T>(Mock<IConnectionManager> connectionManager, TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var table = new TableManager<T>(connectionManager.Object);

            return table;
        }

        public static TableFlywheel<T> TableFlywheel<T>(Mock<IConnectionManager> connectionManager, TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var tableManager = TableManager<T>(connectionManager, timeMachine);
            var flywheel = new TableFlywheel<T>(tableManager);

            return flywheel;
        }

        public static List<Example> NewDataSet(int count)
        {
            var items = new List<Example>();

            for(int x=0; x<count; x++)
                items.Add(new Example(){PartitionKey = "a", RowKey = x.ToString()});

            return items;
        }


        public static List<Example> ExistingDataSet(int count)
        {
            var items = new List<Example>();

            for (int x = 0; x < count; x++)
                items.Add(new Example() { PartitionKey = "a", RowKey = x.ToString(), ETag = "a" });

            return items;
        }


        public static Mock<IConnectionManager> SuccessfulConnectionManager<T>(TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute<T>(It.IsAny<string>(), It.IsAny<TableOperation>()))
                .Returns((string tableName, TableOperation operation) => timeMachine.ScheduleSuccess(SuccessfulTableResult<T>(operation)));

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns((string tableName, TableBatchOperation batch) => timeMachine.ScheduleSuccess(SuccessfulTableResult(batch)));

            return cm;
        }

        public static Mock<IConnectionManager> FastReturnConnectionManager<T>(TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute<T>(It.IsAny<string>(), It.IsAny<TableOperation>()))
                .Returns((string tableName, TableOperation operation) => FastReturnTask(SuccessfulTableResult<T>(operation)));

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns((string tableName, TableBatchOperation batch) => FastReturnTask(SuccessfulTableResult(batch)));

            return cm;
        }


        public static Mock<IConnectionManager> FailingConnectionManager<T>(TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute<T>(It.IsAny<string>(), It.IsAny<TableOperation>()))
                .Returns((string tableName, TableOperation operation) => timeMachine.ScheduleSuccess(FailingTableResult<T>(operation)));

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns((string tableName, TableBatchOperation batch) => timeMachine.ScheduleSuccess(FailingTableResult(batch)));

            return cm;
        }

        public static Mock<IConnectionManager> EvenSuccessOddFailingConnectionManager(TimeMachine timeMachine)
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns((string tableName, TableBatchOperation batch) => timeMachine.ScheduleSuccess(EvenSuccessOddFailingTableResult(batch)));

            return cm;
        }

        public static Mock<IConnectionManager> CancellingTaskConnectionManager<T>(TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute<T>(It.IsAny<string>(), It.IsAny<TableOperation>()))
                .Returns((string tableName, TableOperation operation) => timeMachine.ScheduleCancellation<TableOperationResult<T>>());

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns((string tableName, TableBatchOperation batch) => timeMachine.ScheduleCancellation<TableBatchResult>());

            return cm;
        }

        public static Mock<IConnectionManager> FaultingTaskConnectionManager<T>(TimeMachine timeMachine) where T : AzureDataTableEntity<T>
        {
            var cm = new Mock<IConnectionManager>();

            cm.Setup(i => i.TableExecute<T>(It.IsAny<string>(), It.IsAny<TableOperation>()))
                .Returns(timeMachine.ScheduleFault<TableOperationResult<T>>(new Exception()));

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableBatchOperation>()))
                .Returns(timeMachine.ScheduleFault<TableBatchResult>(new Exception()));

            return cm;
        }

        public static Task<T> FastReturnTask<T>(T data)
        {
            var tsc = new TaskCompletionSource<T>();

            tsc.SetResult(data);

            return tsc.Task;
        }

        public static TableOperationResult<T> SuccessfulTableResult<T>(TableOperation operation) where T : AzureDataTableEntity<T>
        {
            var result = new TableResult {HttpStatusCode = 200};

            return new TableOperationResult<T>(operation, result);

        }

        public static TableOperationResult<T> FailingTableResult<T>(TableOperation operation) where T : AzureDataTableEntity<T>
        {
            var result = new TableResult { HttpStatusCode = 400 };

            return new TableOperationResult<T>(operation, result);
        }

        public static TableBatchResult SuccessfulTableResult(TableBatchOperation batch)
        {
            var items = new List<TableResult>();

            for (int x = 0; x < batch.Count; x++)
                items.Add(new TableResult {HttpStatusCode = 200});

            var result = new TableBatchResult(batch, items);

            return result;
        }

        public static TableBatchResult FailingTableResult(TableBatchOperation batch)
        {
            var items = new List<TableResult>();

            for (int x = 0; x < batch.Count; x++)
                items.Add(new TableResult { HttpStatusCode = 400 });

            var result = new TableBatchResult(batch, items);

            return result;
        }


        public static TableBatchResult EvenSuccessOddFailingTableResult(TableBatchOperation batch)
        {
            var items = new List<TableResult>();

            for (int x = 0; x < batch.Count; x++)
            {
                if (x % 2 == 0)
                    items.Add(new TableResult { HttpStatusCode = 200 });
                else
                    items.Add(new TableResult { HttpStatusCode = 400 });
            }

            var result = new TableBatchResult(batch, items);

            return result;
        }


    }

    public class Example : AzureDataTableEntity<Example>
    {
        public string Title { get; set; }
    }

    [TableName("A-Table")]
    public class ExampleWithNameOverride : AzureDataTableEntity<ExampleWithNameOverride>
    {
        public string Title { get; set; }
    }
}