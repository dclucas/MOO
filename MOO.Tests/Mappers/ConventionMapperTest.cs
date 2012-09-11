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
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Moo.Mappers;

    /// <summary>
    /// This is a test class for ConventionMapperTest and is intended
    /// targetProperty contain all ConventionMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ConventionMapperTest
    {
        #region Methods (11)

         

        [Test]
        public void Map_NonGeneric_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = this.GenerateSource();
            var result = new TestClassB();
            target.Map((object)source, (object)result);
            this.CheckMapping(source, result);
        }

        [Test]
        public void Map_NOTarget_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = this.GenerateSource();
            var result = (TestClassB)target.Map((object)source);
            this.CheckMapping(source, result);
        }

        [Test]
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

        [Test]
        public void Map_WithFactory_MappingWorks()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = this.GenerateSource();
            var result = new TestClassB();
            target.Map((object)source, () => result);
            this.CheckMapping(source, result);
        }

        [Test]
        public void MapMultipleFunctionTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = this.GenerateList(50).ToList();
            var defaultDate = new DateTime(1789, 7, 14);
            var result = target.MapMultiple(source, () => new TestClassB() { Code = defaultDate }).ToList();
            this.CheckLists(source, result);
            Assert.IsTrue(result.All(r => r.Code == defaultDate));
        }

        [Test]
        public void MapMultipleTest()
        {
            var target = new ConventionMapper<TestClassA, TestClassB>();
            var source = this.GenerateList(50).ToList();
            var result = target.MapMultiple(source).ToList();
            this.CheckLists(source, result);
        }

        [Test]
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

         

        private void CheckLists(IList<TestClassA> sourceList, IList<TestClassB> resultList)
        {
            for (int i = 0; i < sourceList.Count; ++i)
            {
                this.CheckMapping(sourceList[i], resultList[i]);
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
                yield return this.GenerateSource();
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