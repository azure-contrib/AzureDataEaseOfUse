using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Serializers
{
    public class XmlSerializer : ISerializer
    {
        public T DeserializeObject<T>(byte[] value) => XmlConvert.DeserializeObject<T>(StringConvert.DeserializeObject(value));

        public byte[] SerializeObject(object value) => StringConvert.SerializeObject(XmlConvert.SerializeObject(value));
    }
}
