using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Azure.IO.Tests.Blobs.Block.AsString
{

    public class InMemory : Tests
    { public InMemory() : base(new IO.Blobs.Block.AsString(containerName: "InMemory")) { } }

    public class InMemoryWithFormatString : Tests
    { public InMemoryWithFormatString() : base(new IO.Blobs.Block.AsString(containerName: "InMemory", formatString: "{0}.txt")) { } }

    public class InMemoryWithPathFormatter : Tests
    { public InMemoryWithPathFormatter() : base(new IO.Blobs.Block.AsString(containerName: "InMemory", pathFormatter: path => $"{path[0]}\\{path}.txt")) { } }



    public class Emulator : Tests
    { public Emulator() : base(new IO.Blobs.Block.AsString(containerName: "Emulator")) { } }

    public class EmulatorWithFormatString : Tests
    { public EmulatorWithFormatString() : base(new IO.Blobs.Block.AsString(containerName: "Emulator", formatString: "{0}.txt")) { } }

    public class EmulatorWithPathFormatter : Tests
    { public EmulatorWithPathFormatter() : base(new IO.Blobs.Block.AsString(containerName: "Emulator", pathFormatter: path => $"{path[0]}\\{path}.txt")) { } }

}
