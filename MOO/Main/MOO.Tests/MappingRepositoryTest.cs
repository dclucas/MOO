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

using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;

namespace Moo.Tests
{
    /// <summary>
    /// This is a test class for MappingRepositoryTest and is intended
    /// targetProperty contain all MappingRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MappingRepositoryTest
    {
        #region Fields (1)

        private TestContext testContextInstance;

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion Properties

        #region Methods (5)

        // Public Methods (5) 

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

        [TestMethod()]
        public void ResolveMapper_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper<TestClassB, TestClassA>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOfType(mapper, typeof(CompositeMapper<TestClassB, TestClassA>));
            Assert.IsTrue(((CompositeMapper<TestClassB, TestClassA>)mapper).InnerMappers.Count() > 0);
        }

        [TestMethod()]
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