/*-----------------------------------------------------------------------------
Copyright 2010 Diogo Lucas

This file is part of Moo.

Foobar is free software: you can redistribute it and/or modify it under the
terms of the GNU General Public License as published by the Free Software
Foundation, either version 3 of the License, or (at your option) any later
version.

Moo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Moo. If not, see http://www.gnu.org/licenses/.
---------------------------------------------------------------------------- */

using System;
using System.ComponentModel;

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