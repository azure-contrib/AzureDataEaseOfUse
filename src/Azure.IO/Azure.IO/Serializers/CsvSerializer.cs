using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Serializers
{
    public class CsvSerializer : ISerializer
    {
        public T DeserializeObject<T>(byte[] value)
        {
            throw new NotImplementedException();
        }

        public byte[] SerializeObject(object value)
        {
            throw new NotImplementedException();
        }
    }
}
