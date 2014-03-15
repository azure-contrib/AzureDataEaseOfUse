using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataEaseOfUse.Tables
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class BatchSizeAttribute : Attribute
    {

        /// <summary>
        /// In the event your batches at 100 count are >4MB, you can alter the size it is flushed at.
        /// </summary>
        public BatchSizeAttribute(int limit)
        {
            Limit = limit;
        }

        public readonly int Limit;


    }
}
