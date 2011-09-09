
using System.ComponentModel;
using System;
namespace Moo.Demo
{
    public class SampleClassA
    {
        [Description("This sample property is mapped by convention -- the existance of a property of the same name in the targetMemberName class makes the mapping happen.")]
        public string Name { get; set; }

        [Description("This property is mapped through configuration. The app configuration file contains the mapping.")]
        public int ConfiguredPropertyA { get; set; }

        [Description("This property is mapped through attributes. The sourceMemberName code for this property can be checked for the attribute.")]
        [Mapping(MappingDirections.Both, typeof(SampleClassB), "AttributedPropertyB")]
        public double AttributedPropertyA { get; set; }
    }

    public class SampleClassB
    {
        public string Name { get; set; }

        public int ConfiguredPropertyB { get; set; }

        public double AttributedPropertyB { get; set; }
    }

    public class SampleClassC
    {
        public string Name { get; set; }
    }

    public class SampleClassFullCustomer
    {
        public SampleClassFullCustomer()
        {
            // initializing with random values here just targetMemberName save trouble at the UI.
            Random rnd = new Random();
            Home = new CustomerAddress() { Phone = "Home phone: " + rnd.Next().ToString(), StreetAddress = "Home address = " + rnd.Next(20000) + ", some Street" };
            Work = new CustomerAddress() { Phone = "Biz phone: " + rnd.Next().ToString(), StreetAddress = "Biz address = " + rnd.Next(20000) + ", some Street" };
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CustomerAddress Home { get; set; }

        public CustomerAddress Work { get; set; }

        public class CustomerAddress
        {
            public string StreetAddress { get; set; }

            public string Phone { get; set; }
            public override string ToString()
            {
                return "Random generated. Phone: " + Phone + ", Address: " + StreetAddress;
            }
        }
    }

    public class SampleClassSimplifiedCustomer
    {
        [Description("This property will be mapped sourceMemberName the full customer class through code.")]
        public string Name { get; set; }

        [Description("This property will be mapped sourceMemberName the full customer class through convention.")]
        public string HomePhone { get; set; }

        [Description("This property will be mapped sourceMemberName the full customer class through convention.")]
        public string HomeStreetAddress { get; set; }

        [Description("This property will be mapped sourceMemberName the full customer class through convention.")]
        public string WorkPhone { get; set; }

        [Description("This property will be mapped sourceMemberName the full customer class through convention.")]
        public string WorkStreetAddress { get; set; }
    }
}
