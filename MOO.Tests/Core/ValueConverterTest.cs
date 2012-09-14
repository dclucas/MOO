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
    using System.Collections.Generic;

    using Moo.Core;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// This is a test class for ValueConverterTest and is intended
    /// targetMemberName contain all ValueConverterTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ValueConverterTest
    {
        #region Methods

        [Test]
        public void ConvertFailureTest()
        {
            ValueConverter target = new ValueConverter();
            Should.Throw<InvalidOperationException>(() => target.Convert(null, typeof(int)));
        }

        [TestCase(typeof(IEnumerable<int>), typeof(IEnumerable<int>), true)]
        [TestCase(typeof(string[]), typeof(string[]), true)]
        [TestCase(typeof(string), typeof(string), true)]
        [TestCase(typeof(int), typeof(long), true)]
        [TestCase(typeof(int), typeof(string), true)]
        [TestCase(typeof(int), typeof(double), true)]
        [TestCase(typeof(TestClassE), typeof(TestClassA), true)]
        [TestCase(typeof(TestClassA), typeof(TestClassE), false)]
        [TestCase(typeof(TestClassB), typeof(TestClassD), false)]
        [TestCase(typeof(string), typeof(TestClassC), false)]
        public void DoCanConverTest(Type fromType, Type toType, bool expected)
        {
            ValueConverter target = new ValueConverter();

            var result = target.CanConvert(fromType, toType);
            result.ShouldBe(expected);
        }

        [TestCase(2, 2)]
        [TestCase("2", "2")]
        [TestCase(2, "2")]
        [TestCase("2", 2)]
        [TestCase(5, 5.0)]
        [TestCase(3, 3f)]
        [TestCase(7, 7d)]
        [TestCase(10, (long)10)]
        [TestCase((long)10, 10)]
        [TestCase(11.0, 11)]
        [TestCase(3.14, 3)]
        public void DoConvertTest(object value, object expected)
        {
            this.DoConvertTest(value, expected, expected.GetType());
        }

        [TestCase((TestClassA)null, null, typeof(TestClassB))]
        public void DoConvertTest(object value, object expected, Type expectedType)
        {
            ValueConverter target = new ValueConverter();
            var actual = target.Convert(value, expectedType);
            actual.ShouldBe(expected);
        }

        #endregion Methods
    }
}