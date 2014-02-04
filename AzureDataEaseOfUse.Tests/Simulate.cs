using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using AzureDataEaseOfUse.Tables;

namespace AzureDataEaseOfUse
{
    public static class Simulate
    {

        public static string TableName()
        {
            return "t" + Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }

        public static ExamplePost Post(string blogId = null, string title = "Cool Beans")
        {
            return new ExamplePost()
            {
                BlogId = string.IsNullOrEmpty(blogId) ? System.Guid.NewGuid().ToString() : blogId,
                Posted = DateTime.UtcNow,
                Title = title
            };
        }
    }

    public class ExamplePost : TableEntity, IAzureStorageTable
    {
        public string BlogId { get; set; }
        public DateTime Posted { get; set; }
        public string Title { get; set; }


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
