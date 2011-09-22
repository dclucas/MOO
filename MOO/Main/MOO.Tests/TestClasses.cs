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
namespace Moo.Tests
{
    public class TestClassA
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public TestClassC InnerClass { get; set; }
    }

    public class TestClassB
    {
        public string Name { get; set; }
        public DateTime Code { get; set; }
        public string InnerClassName { get; set; }
        public int InnerClassCode { get; set; }
        public double InnerClassFraction { get; set; }
    }

    public class TestClassC
    {
        public string Name { get; set; }
        public DateTime Code { get; set; }
        public double Fraction { get; set; }
    }

    public class TestClassD
    {
        [Mapping(MappingDirections.Both, typeof(TestClassA), "Name")]
        public string SomeOtherName { get; set; }
        [Mapping(MappingDirections.To, typeof(TestClassA), "Code")]
        public int AnotherCode { get; set; }
    }

    public class TestClassE : TestClassA
    {
    }
}