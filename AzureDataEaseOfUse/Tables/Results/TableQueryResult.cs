using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureDataEaseOfUse.Tables
{
    public class TableQueryResult<T> where T : AzureDataTableEntity<T>, new()
    {
        public TableQueryResult(Expression<Func<T, bool>> predicate, List<T> values)
        {
            this.Predicate = predicate;
            this.Values = values;
        }

        public readonly Expression<Func<T, bool>> Predicate;
        public readonly List<T> Values;

    }
}
