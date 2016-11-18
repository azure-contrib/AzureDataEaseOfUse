using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Azure.IO.Tests.Blobs.Block.AsJson
{
    //public class InMemoryScenario : Tests
    //{
    //    public InMemoryScenario()
    //    {
    //        Source = new Dictionary<string, byte[]>();
    //        Container = new Azure.IO.Blobs.InMemoryContainer(Source);
    //        IO = new Azure.IO.Blobs.Block.IO(Container);
    //        Adapter = new Azure.IO.Blobs.Block.AsJson<Contact>(IO);
    //    }

    //    private readonly IDictionary<string, byte[]> Source;

    //    protected override async Task<Contact> GetDirectValue(string key) => await Adapter.Load(key);//JsonConvert.DeserializeObject<Contact>(Encoding.UTF8.GetString(await Task.FromResult(Source[key])));

    //}
}
