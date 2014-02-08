using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables.Async;

namespace AzureDataEaseOfUse
{
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

        /// <summary>
        /// Creates a, b, and c partitions with 1, 2, and 3 rows
        /// </summary>
        public async static Task<CloudTable> FilledTable()
        {
            var table = await Simulate.Table();
            var batch = table.Batch<ExamplePost>();

            for (int x = 1; x <= 3; x++)
                batch.Add(Simulate.Post("a", x.ToString()));

            for (int x = 1; x <= 3; x++)
                batch.Add(Simulate.Post("b", x.ToString()));

            for (int x = 1; x <= 3; x++)
                batch.Add(Simulate.Post("c", x.ToString()));

            batch.Execute();

            return table;
        
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
