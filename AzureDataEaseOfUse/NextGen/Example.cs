using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataEaseOfUse.NextGen
{

    public class example
    {
        private void test()
        {
            var table = new TableManager<Person>(null);

        }

    }

    [TableName("ThePerson")]
    public class Person : AzureDataTableEntity<Person>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public override string GetPartitionKey()
        {
            return LastName;
        }

        public override string GetRowKey()
        {
            return FirstName;
        }
    }
}
