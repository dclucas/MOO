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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo.Core;

    /// <summary>
    /// This is a test class for PropertyMatcherTest and is intended
    /// targetProperty contain all PropertyMatcherTest Unit Tests
    /// </summary>
    [TestClass]
    public class PropertyConverterTest
    {
        #region Methods (8)

        // Public Methods (7) 

        [TestMethod]
        public void ConvertTestComplex()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            source.InnerClass = new TestClassC();
            var expected = "test";
            source.InnerClass.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("InnerClass");
            TestClassB targetObject = new TestClassB();
            targetObject.InnerClassName = "wrongString";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("InnerClassName");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.InnerClassName);
            Assert.AreEqual(expected, source.InnerClass.Name);
        }

        /// <summary>
        /// A test for Convert
        /// </summary>
        [TestMethod]
        public void ConvertTestSimple()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            var expected = "test";
            source.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("Name");
            TestClassB targetObject = new TestClassB();
            targetObject.Name = "wrongString";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("Name");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.Name);
            Assert.AreEqual(expected, source.Name);
        }

        [TestMethod]
        public void PropertiesMatchFlattenNegativeTest()
        {
            TestMatch("InnerClass", "InnerClassCode", false);
        }

        [TestMethod]
        public void PropertiesMatchFlattenTest()
        {
            TestMatch("InnerClass", "InnerClassName", true);
        }

        [TestMethod]
        public void PropertiesMatchSimpleNegativeTest()
        {
            TestMatch("Code", "Code", false);
        }

        /// <summary>
        /// A test for PropertiesMatch
        /// </summary>
        [TestMethod]
        public void PropertiesMatchSimpleTest()
        {
            TestMatch("Name", "Name", true);
        }

        [TestMethod]
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

        // Private Methods (1) 

        private void TestMatch(string fromProp, string toProp, bool expected)
        {
            PropertyConverter target = new PropertyConverter();
            PropertyInfo from = typeof(TestClassA).GetProperty(fromProp);
            PropertyInfo to = typeof(TestClassB).GetProperty(toProp);
            bool actual;
            string finalName;
            actual = target.CanConvert(from, to, out finalName);
            Assert.AreEqual(expected, actual);
        }

        #endregion Methods
    }
}