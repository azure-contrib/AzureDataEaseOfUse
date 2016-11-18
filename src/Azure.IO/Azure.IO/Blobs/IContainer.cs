using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Blobs
{
    public interface IContainer
    {
        Task<IBlob> GetBlob(string path);

        Task<List<string>> List();

        Task Init();
    }
}
