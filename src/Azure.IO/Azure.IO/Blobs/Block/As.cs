using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Blobs.Block
{
    public abstract class As
    {

        public As(string containerName) : this("{0}", containerName) { }

        public As(string formatString, string containerName) : this(x => string.Format(formatString, x), containerName) { }

        public As(Func<string, string> pathFormatter, string containerName)
        {
            PathFormatter = pathFormatter;
            IO = IOFactory.GetBlockIO(containerName, pathFormatter);
        }



        public Func<string, string> PathFormatter { get; }

        protected IIO IO { get; }

        public async Task<bool> Exists(string path) => await IO.Exists(path);

        public async Task Delete(string path) => await IO.Delete(path);

        //public async Task<List<string>> List() => await IO.List();

    }
}
