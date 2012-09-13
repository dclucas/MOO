namespace Moo.Tests.Integration.MappedClasses.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PersonDetailsDataContract
    {
        public string Name { get; set; }

        public IEnumerable<ContactDataContract> PersonContacts { get; set; }

        public AccountDataContract Account { get; set; }
    }
}
