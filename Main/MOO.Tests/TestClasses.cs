// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Moo.Tests
{
    using System;

    public class TestClassA
    {
        #region Properties (3)

        public int Code { get; set; }

        public TestClassC InnerClass { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }

    public class TestClassB
    {
        #region Properties (5)

        public DateTime Code { get; set; }

        public int InnerClassCode { get; set; }

        public double InnerClassFraction { get; set; }

        public string InnerClassName { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }

    public class TestClassC
    {
        #region Properties (3)

        public DateTime Code { get; set; }

        public double Fraction { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }

    public class TestClassD
    {
        #region Properties (2)

        [Mapping(MappingDirections.To, typeof(TestClassA), "Code")]
        public int AnotherCode { get; set; }

        [Mapping(MappingDirections.Both, typeof(TestClassA), "Name")]
        public string SomeOtherName { get; set; }

        #endregion Properties
    }

    public class TestClassE : TestClassA
    {
    }

    public class TestClassF
    {
        public TestClassB InnerClass { get; set; }
    }
}