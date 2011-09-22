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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;

namespace Moo.Tests.Mappers
{
    /// <summary>
    /// This is a test class for ConventionMapperTest and is intended
    /// targetProperty contain all ConventionMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConventionMapperTest
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

        #region Methods (11)

        // Public Methods (7) 

        [TestMethod]
        public void Map_NonGeneric_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateSource();
            var result = new TestClassB();
            target.Map((object)source, (object)result);
            CheckMapping(source, result);
        }

        [TestMethod]
        public void Map_NOTarget_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateSource();
            var result = (TestClassB)target.Map((object)source);
            CheckMapping(source, result);
        }

        [TestMethod]
        public void Map_NullInner_PropertySkipped()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var mapSource = new TestClassA()
            {
                Name = "TestName",
                Code = 123,
                InnerClass = null, // this needs to be null for this scenario. Making it explicit here.
            };

            var result = target.Map(mapSource);

            Assert.IsNull(result.InnerClassName);
            Assert.AreEqual(mapSource.Name, result.Name);
        }

        [TestMethod]
        public void Map_WithFactory_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateSource();
            var result = new TestClassB();
            target.Map((object)source, () => result);
            CheckMapping(source, result);
        }

        [TestMethod]
        public void MapMultipleFunctionTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateList(50).ToList();
            var defaultDate = new DateTime(1789, 7, 14);
            var result = target.MapMultiple(source, () => new TestClassB() { Code = defaultDate }).ToList();
            CheckLists(source, result);
            Assert.IsTrue(result.All(r => r.Code == defaultDate));
        }

        [TestMethod]
        public void MapMultipleTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = GenerateList(50).ToList();
            var result = target.MapMultiple(source).ToList();
            CheckLists(source, result);
        }

        [TestMethod]
        public void MapTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var from = new TestClassA();
            var to = new TestClassB();
            from.Code = 123;
            var expectedName = "Test123";
            from.Name = expectedName;
            from.InnerClass = new TestClassC();
            var expectedInnerName = "InnerName234";
            var expectedInnerFraction = Math.PI;
            from.InnerClass.Name = expectedInnerName;
            from.InnerClass.Fraction = expectedInnerFraction;
            target.Map(from, to);
            Assert.AreEqual(expectedName, to.Name);
            Assert.AreEqual(expectedInnerName, to.InnerClassName);
            Assert.AreEqual(expectedInnerFraction, to.InnerClassFraction);

            // making sure the mapping didn't mess with the "sourceMemberName" object.
            Assert.AreEqual(expectedName, from.Name);
            Assert.AreEqual(expectedInnerName, from.InnerClass.Name);
            Assert.AreEqual(expectedInnerFraction, from.InnerClass.Fraction);
        }

        // Private Methods (4) 

        private void CheckLists(IList<TestClassA> sourceList, IList<TestClassB> resultList)
        {
            for (int i = 0; i < sourceList.Count; ++i)
            {
                CheckMapping(sourceList[i], resultList[i]);
            }
        }

        private void CheckMapping(TestClassA source, TestClassB target)
        {
            Assert.AreEqual(source.Name, target.Name);
            Assert.AreEqual(source.InnerClass.Name, target.InnerClassName);
            Assert.AreEqual(source.InnerClass.Fraction, target.InnerClassFraction);
        }

        private IEnumerable<TestClassA> GenerateList(int count)
        {
            while (count-- > 0)
            {
                yield return GenerateSource();
            }
        }

        private TestClassA GenerateSource()
        {
            Random rnd = new Random();
            TestClassA source = new TestClassA()
            {
                Name = "Name" + rnd.Next(1000).ToString(),
                InnerClass = new TestClassC()
                {
                    Name = "InnerName" + rnd.Next(1000).ToString(),
                    Fraction = rnd.NextDouble()
                }
            };

            return source;
        }

        #endregion Methods
    }
}