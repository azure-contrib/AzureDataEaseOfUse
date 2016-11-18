using Azure.IO.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Examples
{
    public class MyAppRepo
    {

        public Blobs.Block.AsBytes Pictures1 = new Blobs.Block.AsBytes();

        public Blobs.Block.AsBytes Pictures2 = new Blobs.Block.AsBytes("{0}.png");

        public Blobs.Block.AsBytes Pictures3 = new Blobs.Block.AsBytes(x => $"{x[0]}\\{x}.png");

        void blah()
        {
            Pictures3.Save("abc", new byte[1]).Wait();

            Pictures1.Update("abc", x => x[0] = 3).Wait();



            /*
             * pics.Load("woifnwo");  can load from static location or dynamic location
             * pics.Exists("wefwef"); can detect from static location or dynamic location
             *      pics.exists({value}); 
             * pics.Delete("wfwef"); can delete from static location or dynamic location
             *      pics.Delete({value});
             * pics.Update("wefwef"); can load from static location or dynamic location
             * 
             * 
             * 
             */


        }

        private void Pictures1_Saved(string path, byte[] value)
        {
            throw new NotImplementedException();
        }
    }

}
