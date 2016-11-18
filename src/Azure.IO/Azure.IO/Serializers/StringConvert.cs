using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Serializers
{
    public static class StringConvert
    {
        public static byte[] ToByteArray(this string value) => SerializeObject(value);

        public static string FromByteArray(this byte[] value) => DeserializeObject(value);


        public static byte[] SerializeObject(string value) => Encoding.UTF8.GetBytes(value);

        public static string DeserializeObject(byte[] value) => Encoding.UTF8.GetString(value);


    }
}
