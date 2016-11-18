using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Azure.IO
{
    public static class XmlConvert
    {

        public static string ToXml<T>(this T value) => SerializeObject(value);
        public static T FromXml<T>(this string value) => DeserializeObject<T>(value);


        public static string SerializeObject<T>(T value)
        {
            var xml = new XmlSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                xml.Serialize(ms, value);

                return ms.ToString();
            }
        }

        public static T DeserializeObject<T>(string value)
        {
            var xml = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(value))
            {
                return (T)xml.Deserialize(sr);
            }
        }




    }
}
