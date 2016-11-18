using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IO.Tests.Blobs.Block.AsJson
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override bool Equals(object obj) => Equals((Contact)obj);

        public override int GetHashCode() => $"{FirstName} {LastName}".GetHashCode();

        public bool Equals(Contact contact) => FirstName == contact.FirstName && LastName == contact.LastName;

    }
}
