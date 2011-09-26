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
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo.Mappers;

    /// <summary>
    /// This is a test class for MappingRepositoryTest and is intended
    /// targetProperty contain all MappingRepositoryTest Unit Tests
    /// </summary>
    [TestClass]
    public class MappingRepositoryTest
    {
        #region Methods (5)

        // Public Methods (5) 

        [TestMethod]
        public void AddMapperTest()
        {
            IExtensibleMapper<TestClassA, TestClassB> expected = new ManualMapper<TestClassA, TestClassB>();
            var target = new MappingRepository();
            target.AddMapper(expected);
            var actual = target.ResolveMapper<TestClassA, TestClassB>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            IExtensibleMapper<TestClassA, TestClassB> mapper = new ManualMapper<TestClassA, TestClassB>();
            var target = new MappingRepository();
            target.AddMapper(mapper);
            target.Clear();
            var mappersField = target.GetType().GetField("mappers", BindingFlags.NonPublic | BindingFlags.Instance);
            var mappers = (IEnumerable)mappersField.GetValue(target);
            Assert.IsFalse(mappers.Cast<object>().Any());
        }

        [TestMethod]
        public void GetDefaultTest()
        {
            Assert.IsNotNull(MappingRepository.Default);
        }

        [TestMethod]
        public void ResolveMapper_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper<TestClassB, TestClassA>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        [TestMethod]
        public void ResolveMapper2_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper(typeof(TestClassB), typeof(TestClassA));
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        #endregion Methods
    }
}