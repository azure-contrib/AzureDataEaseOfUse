using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Azure.IO.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public T DeserializeObject<T>(byte[] value) => JsonConvert.DeserializeObject<T>(StringConvert.DeserializeObject(value));

        public byte[] SerializeObject(object value) => StringConvert.SerializeObject(JsonConvert.SerializeObject(value));
    }
}
