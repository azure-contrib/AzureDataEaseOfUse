using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Tests.Blobs.Block.AsBytes
{

    public class InMemory : Tests
    { public InMemory() : base(new IO.Blobs.Block.AsBytes(containerName: "InMemory")) { } }

    public class InMemoryWithFormatString : Tests
    { public InMemoryWithFormatString() : base(new IO.Blobs.Block.AsBytes(containerName: "InMemory", formatString: "{0}.dat")) { } }

    public class InMemoryWithPathFormatter : Tests
    { public InMemoryWithPathFormatter() : base(new IO.Blobs.Block.AsBytes(containerName: "InMemory", pathFormatter: path => $"{path[0]}\\{path}.dat")) { } }



    public class Emulator : Tests
    { public Emulator() : base(new IO.Blobs.Block.AsBytes(containerName: "Emulator")) { } }

    public class EmulatorWithFormatString : Tests
    { public EmulatorWithFormatString() : base(new IO.Blobs.Block.AsBytes(containerName: "Emulator", formatString: "{0}.dat")) { } }

    public class EmulatorWithPathFormatter : Tests
    { public EmulatorWithPathFormatter() : base(new IO.Blobs.Block.AsBytes(containerName: "Emulator", pathFormatter: path => $"{path[0]}\\{path}.dat")) { } }


}
