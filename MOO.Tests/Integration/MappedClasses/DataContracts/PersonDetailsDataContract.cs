using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Tests.Integration.MappedClasses.DataContracts
{
    public class PersonDetailsDataContract
    {
        public string Name { get; set; }

        public IEnumerable<ContactDataContract> Contacts { get; set; }

        public AccountDataContract Account { get; set; }
    }
}
