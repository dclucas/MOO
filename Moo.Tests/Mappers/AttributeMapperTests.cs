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

using Moo.Mappers;
using NUnit.Framework;

namespace Moo.Tests.Mappers
{
    /// <summary>
    ///     This is a test class for AttributeMapperTest and is intended
    ///     targetMember contain all AttributeMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class AttributeMapperTest
    {
        [Test]
        public void Map_MappedSource_SkipsMapping()
        {
            string expectedName = "Test Name";
            int expectedCode = 412;

            var a = new TestClassA();
            var d = new TestClassD {AnotherCode = expectedCode, SomeOtherName = expectedName};

            var target = new AttributeMapper<TestClassD, TestClassA>();
            target.Map(d, a);

            Assert.AreEqual(expectedName, d.SomeOtherName);
            Assert.AreEqual(expectedCode, d.AnotherCode);

            Assert.AreEqual(expectedName, a.Name);

            // since the mapping direction for AnotherCode is "TargetMemberName" only,
            // a.Code should be left with its default sourceValue.
            Assert.AreEqual(0, a.Code);
        }

        [Test]
        public void Map_MapperTarget_MapsCorrectly()
        {
            string expectedName = "Test Name";
            int expectedCode = 412;

            var a = new TestClassA {Code = expectedCode, Name = expectedName};
            var d = new TestClassD();

            var target = new AttributeMapper<TestClassA, TestClassD>();
            target.Map(a, d);

            Assert.AreEqual(expectedName, d.SomeOtherName);
            Assert.AreEqual(expectedCode, d.AnotherCode);

            Assert.AreEqual(expectedName, a.Name);
            Assert.AreEqual(expectedCode, a.Code);
        }
    }
}