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

using System;
using System.Linq.Expressions;
using System.Reflection;
using Moo.Core;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Shouldly;

namespace Moo.Tests.Core
{
    /// <summary>
    ///     This is a test class for PropertyMatcherTest and is intended
    ///     targetProperty contain all PropertyMatcherTest Unit Tests
    /// </summary>
    [TestFixture]
    public class PropertyConverterTests
    {
        [TestCase(typeof (TestClassA), typeof (TestClassB), "InnerClass", "InnerClassName", null)]
        public void CreateConvertExpression_MultipleCases_ConvertsCorrectly(
            Type sourceType,
            Type targetType,
            string sourcePropertyName,
            string targetPropertyName,
            object sourcePropertyValue)
        {
            var target = new PropertyConverter();
            var fixture = new Fixture();
            MethodInfo createMethod = fixture.GetType().GetMethod("CreateAnonymous");
            object sourceObject = Activator.CreateInstance(sourceType);
            object targetObject = Activator.CreateInstance(targetType);
            ParameterExpression sourceParam = Expression.Parameter(sourceType);
            ParameterExpression targetParam = Expression.Parameter(targetType);
            PropertyInfo sourceProp = sourceType.GetProperty(sourcePropertyName);
            PropertyInfo targetProp = targetType.GetProperty(targetPropertyName);
            if (sourcePropertyValue != null)
            {
                sourceProp.SetValue(sourceObject, sourcePropertyValue, null);
            }

            var expr = target.CreateConvertExpression(
                sourceProp,
                targetProp,
                sourceParam,
                targetParam);

            var actionType = typeof (MappingAction<,>).MakeGenericType(sourceType, targetType);
            var lambda = Expression.Lambda(actionType, expr, sourceParam, targetParam);
            var action = lambda.Compile();
            action.DynamicInvoke(sourceObject, targetObject);
            var resultProp = targetProp.GetValue(targetObject, null);
            resultProp.ShouldBe(sourcePropertyValue);
        }

        private void TestMatch(string fromProp, string toProp, bool expected)
        {
            var target = new PropertyConverter();
            var from = typeof (TestClassA).GetProperty(fromProp);
            var to = typeof (TestClassB).GetProperty(toProp);
            var actual = target.CanConvert(@from, to);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTestComplex()
        {
            var target = new PropertyConverter();
            var source = new TestClassA {InnerClass = new TestClassC()};
            const string expected = "test";
            source.InnerClass.Name = expected;
            var fromProperty = typeof (TestClassA).GetProperty("InnerClass");
            var targetObject = new TestClassB {InnerClassName = "wrongstring"};
            var toProperty = typeof (TestClassB).GetProperty("InnerClassName");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.InnerClassName);
            Assert.AreEqual(expected, source.InnerClass.Name);
        }

        /// <summary>
        ///     A test for Convert
        /// </summary>
        [Test]
        public void ConvertTestSimple()
        {
            var target = new PropertyConverter();
            var source = new TestClassA();
            const string expected = "test";
            source.Name = expected;
            var fromProperty = typeof (TestClassA).GetProperty("Name");
            var targetObject = new TestClassB();
            targetObject.Name = "wrongstring";
            var toProperty = typeof (TestClassB).GetProperty("Name");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.Name);
            Assert.AreEqual(expected, source.Name);
        }

        [Test]
        public void PropertiesMatchFlattenNegativeTest()
        {
            TestMatch("InnerClass", "InnerClassCode", false);
        }

        [Test]
        public void PropertiesMatchFlattenTest()
        {
            TestMatch("InnerClass", "InnerClassName", true);
        }

        [Test]
        public void PropertiesMatchSimpleNegativeTest()
        {
            TestMatch("Code", "Code", false);
        }

        /// <summary>
        ///     A test for PropertiesMatch
        /// </summary>
        [Test]
        public void PropertiesMatchSimpleTest()
        {
            TestMatch("Name", "Name", true);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void PropertyConverterNegativeTest()
        {
            var target = new PropertyConverter();
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
    }
}