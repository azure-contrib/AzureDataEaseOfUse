using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;
using AzureDataEaseOfUse;

namespace AzureDataEaseOfUse
{

    public enum FillTable { Empty, OnePartition, TwoPartitions }

    public static class Simulate
    {

        public static string TableName()
        {
            return "t" + Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }

        public async static Task<CloudTable> Table()
        {
            return await Storage.Connect().Table(TableName());
        }

        public static TableFlywheel<ExamplePost> Flywheel()
        {
            var tableTask = Table();

            tableTask.Wait();

            var flywheel = tableTask.Result.Flywheel<ExamplePost>();

            return flywheel;
        }


        public static List<ExamplePost> Posts(int partitionCount, int rowCount)
        {
            var items = new List<ExamplePost>();

            for (int b = 1; b <= partitionCount; b++)
                for (int x = 1; x <= rowCount; x++)
                    items.Add(Post(b.ToString(), x.ToString()));

            return items;
        }

        public static ExamplePost Post(int partition, int row)
        {
            return Post(partition.ToString(), row.ToString());
        }

        public static ExamplePost Post(string blogId = null, string title = "Cool Beans")
        {
            return new ExamplePost()
            {
                BlogId = string.IsNullOrEmpty(blogId) ? System.Guid.NewGuid().ToString() : blogId,
                Posted = DateTime.UtcNow,
                Title = title,
                Amount = 3
            };
        }

        public static TableFlywheel<ExamplePost> Filled(this TableFlywheel<ExamplePost> flywheel, int partitions, int rows)
        {
            flywheel.Table.Filled(partitions, rows);

            return flywheel;
        }

        public static CloudTable Filled(this CloudTable table, int partitions, int rows)
        {
            var flywheel = table.Flywheel<ExamplePost>();

            for (var partition = 1; partition <= partitions; partition++)
                for (int row = 1; row <= rows; row++)
                    flywheel.Insert(Simulate.Post(partition.ToString(), row.ToString()));

            flywheel.Flush();

            return table;
        }

        /// <summary>
        /// Creates a, b, and c partitions with 1, 2, and 3 rows
        /// </summary>
        public async static Task<CloudTable> FilledTable()
        {
            var table = await Simulate.Table();

            var flywheel = table.Flywheel<ExamplePost>();

            for (int x = 1; x <= 3; x++)
                flywheel.Insert(Simulate.Post("a", x.ToString()));

            for (int x = 1; x <= 3; x++)
                flywheel.Insert(Simulate.Post("b", x.ToString()));

            for (int x = 1; x <= 3; x++)
                flywheel.Insert(Simulate.Post("c", x.ToString()));

            flywheel.Flush();

            return table;
        
        }


        public async static void  CleanUp()
        {
            var azure = Storage.Connect();

            foreach (var table in azure.Tables())
                await table.Delete(true);
        }

    }

    public class ExamplePost : TableEntity, IAzureStorageTable
    {

        public string BlogId { get; set; }

        public DateTime Posted { get; set; }

        public string Title { get; set; }

        public int Amount { get; set; }

        public string GetPartitionKey()
        {
            return BlogId;
        }

        public string GetRowKey()
        {
            return Title;
        }
    }

}
