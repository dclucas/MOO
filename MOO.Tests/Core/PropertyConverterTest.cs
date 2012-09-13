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
namespace Moo.Tests.Core
{
    using System;
    using System.Reflection;
    using NUnit.Framework;
    using Moo.Core;

    /// <summary>
    /// This is a test class for PropertyMatcherTest and is intended
    /// targetProperty contain all PropertyMatcherTest Unit Tests
    /// </summary>
    [TestFixture]
    public class PropertyConverterTest
    {
        #region Methods

        [Test]
        public void ConvertTestComplex()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            source.InnerClass = new TestClassC();
            var expected = "test";
            source.InnerClass.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("InnerClass");
            TestClassB targetObject = new TestClassB();
            targetObject.InnerClassName = "wrongstring";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("InnerClassName");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.InnerClassName);
            Assert.AreEqual(expected, source.InnerClass.Name);
        }

        /// <summary>
        /// A test for Convert
        /// </summary>
        [Test]
        public void ConvertTestSimple()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            var expected = "test";
            source.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("Name");
            TestClassB targetObject = new TestClassB();
            targetObject.Name = "wrongstring";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("Name");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.Name);
            Assert.AreEqual(expected, source.Name);
        }

        [Test]
        public void PropertiesMatchFlattenNegativeTest()
        {
            this.TestMatch("InnerClass", "InnerClassCode", false);
        }

        [Test]
        public void PropertiesMatchFlattenTest()
        {
            this.TestMatch("InnerClass", "InnerClassName", true);
        }

        [Test]
        public void PropertiesMatchSimpleNegativeTest()
        {
            this.TestMatch("Code", "Code", false);
        }

        /// <summary>
        /// A test for PropertiesMatch
        /// </summary>
        [Test]
        public void PropertiesMatchSimpleTest()
        {
            this.TestMatch("Name", "Name", true);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyConverterNegativeTest()
        {
            PropertyConverter target = new PropertyConverter();
            var source = new TestClassA();
            var sourceProperty = source.GetType().GetProperty("InnerClass");
            var result = new TestClassB();
            var resultProperty = result.GetType().GetProperty("InnerClassCode");
            source.InnerClass = new TestClassC();
            target.Convert(
                source,
                sourceProperty,
                target,
                resultProperty,
                false);
        }

        private void TestMatch(string fromProp, string toProp, bool expected)
        {
            PropertyConverter target = new PropertyConverter();
            PropertyInfo from = typeof(TestClassA).GetProperty(fromProp);
            PropertyInfo to = typeof(TestClassB).GetProperty(toProp);
            bool actual;
            actual = target.CanConvert(from, to);
            Assert.AreEqual(expected, actual);
        }

        #endregion Methods
    }
}