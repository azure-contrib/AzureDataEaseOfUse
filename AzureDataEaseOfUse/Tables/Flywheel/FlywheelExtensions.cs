using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDataEaseOfUse.Tables.Async
{
    public static class FlywheelExtensions
    {
        public static bool IsSuccessful(this TableResult result)
        {
            return (200 <= result.HttpStatusCode && result.HttpStatusCode < 300);
        }

        public static bool IsConflict(this TableResult result)
        {
            return (409 == result.HttpStatusCode);
        }

        public static bool IsSuccessful<T>(this Task<T> item)
        {
            return item.IsCompleted && !item.IsCanceled && !item.IsFaulted;
        }

    }
}
