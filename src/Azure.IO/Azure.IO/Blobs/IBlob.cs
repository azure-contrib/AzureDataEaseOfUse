using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Blobs
{
    public interface IBlob
    {

        Task Upload(byte[] value);

        Task<byte[]> Download();

        Task<bool> Exists();

        Task Delete();
    }
}
