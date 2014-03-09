using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;
using Moq;
using AzureDataEaseOfUse;

namespace AzureDataEaseOfUse.NextGen7.Tests
{
    public class The_TableManager
    {
        public The_TableManager()
        {
            Table = Simulate.TableManager<Example>();
            NewData = new Example();
            ExistingData = new Example() {ETag = "abc"};

            Operations = new List<TableOperation>
            {
                TableOperation.Insert(NewData),
                TableOperation.Replace(ExistingData)
            };

            BatchOperations = new List<TableBatchOperation> {new TableBatchOperation(), new TableBatchOperation()};
        }

        private readonly TableManager<Example> Table;
        private readonly Example NewData;
        private readonly Example ExistingData;
        private List<TableOperation> Operations;
        private List<TableBatchOperation> BatchOperations;
            
            [Fact]
        public void Can_Create_Table_Connection()
        {
            var connectionManager = new Mock<IConnectionManager>();
            var table = new TableManager<Example>(connectionManager.Object);
        }

        [Fact]
        public void Can_Get_Table_Name_Property_From_Type()
        {
            var result = Table.GetTableName();

            Assert.Equal("Example", result);
        }

        [Fact]
        public void Can_Get_Table_Name_Property_From_Attribute_Override()
        {
            var table = Simulate.TableManager<ExampleWithNameOverride>();

            var result = table.GetTableName();

            Assert.Equal("A-Table", result);
        }

        #region Execution Operations

        [Fact]
        public void Can_Execute_An_Operation()
        {
            var operation = TableOperation.Insert(NewData);

            var result = Table.Execute(operation);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Execute_A_Batch_Operation()
        {
            var batch = new TableBatchOperation();

            var result = Table.Execute(batch);

            Assert.NotNull(result);
        }

        #endregion

        #region Change Operations

        [Fact]
        public void Can_Insert()
        {
            var result = Table.Insert(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Replace()
        {
            var result = Table.Replace(ExistingData);

            Assert.NotNull(result);
        }


        [Fact]
        public void Can_Delete()
        {
            var result = Table.Delete(ExistingData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrReplace_New_Data()
        {
            var result = Table.InsertOrReplace(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrReplace_Existing_Data()
        {
            var result = Table.InsertOrReplace(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Merge()
        {
            var result = Table.Merge(ExistingData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrMerge_New_Data()
        {
            var result = Table.InsertOrMerge(NewData);

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_InsertOrMerge_Existing_Data()
        {
            var result = Table.InsertOrMerge(ExistingData);

            Assert.NotNull(result);
        }

        #endregion

        #region Aggregators - Operations

        [Fact]
        public void Can_Execute_IEnumerable_List_Of_Operations()
        {
            var result = Table.Execute(Operations);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public void Can_Execute_List_Of_Operations_Via_Params()
        {
            var result = Table.Execute(Operations[0], Operations[1]);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        #endregion

        #region Aggregators - Batch Operations

        [Fact]
        public void Can_Execute_IEnumerable_List_Of_Batch_Operations()
        {
            var result = Table.Execute(BatchOperations);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Can_Execute_List_Of_Batch_Operations_Via_Params()
        {
            var result = Table.Execute(BatchOperations[0], BatchOperations[1]);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        #endregion


    }


    public static class Simulate
    {
        public static TableManager<T> TableManager<T>() where T : AzureDataTableEntity<T>
        {
            var connectionManager = ConnectionManager();
            var table = new TableManager<T>(connectionManager.Object);

            return table;
        }


        public static Mock<IConnectionManager> ConnectionManager()
        {
            var cm = new Mock<IConnectionManager>();

            var taskResult = new Task<TableResult>(() => new TableResult(){ HttpStatusCode = 200 });

            cm.Setup(i => i.TableExecute(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns(taskResult);

            return cm;
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
