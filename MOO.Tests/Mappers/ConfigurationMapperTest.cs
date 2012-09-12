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
namespace Moo.Tests.Mappers
{
    using System;
    using System.Reflection;
    using NUnit.Framework;
    using Moo.Configuration;
    using Moo.Mappers;

    /// <summary>
    /// This is a test class for ConfigurationMapperTest and is intended
    /// targetMemberName contain all ConfigurationMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ConfigurationMapperTest
    {
        #region Methods

        [Test]
        public void GetTypeMappingNoSectionTest()
        {
            var actual = ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping("thisConfigDoesNotExist");
            Assert.IsNull(actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTypeMappingNullSectionTest()
        {
            ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping(null);
        }

        [Test]
        public void GetTypeMappingTest()
        {
            var actual = ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping();
            Assert.IsNotNull(actual);
        }

        [Test]
        public void MapTest()
        {
            ConfigurationMapper<TestClassA, TestClassB> target = new ConfigurationMapper<TestClassA, TestClassB>();
            TestClassA from = new TestClassA() { Name = "test" };
            TestClassB to = new TestClassB();
            target.Map(from, to);
            Assert.AreEqual(from.Name, to.InnerClassName);
        }

        #endregion Methods
    }
}