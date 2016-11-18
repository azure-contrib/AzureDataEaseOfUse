using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Serializers
{
    public interface ISerializer
    {

        byte[] SerializeObject(object value);

        T DeserializeObject<T>(byte[] value);

    }
}
