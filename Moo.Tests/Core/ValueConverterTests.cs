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
using System.Collections.Generic;
using Moo.Core;
using NUnit.Framework;
using Shouldly;

namespace Moo.Tests.Core
{
    /// <summary>
    ///     This is a test class for ValueConverterTest and is intended
    ///     targetMember contain all ValueConverterTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ValueConverterTest
    {
        [TestCase(typeof (IEnumerable<string>), typeof (IEnumerable<object>), true)]
        [TestCase(typeof (TestClassE[]), typeof (TestClassA[]), true)]
        [TestCase(typeof (IEnumerable<int>), typeof (IEnumerable<int>), true)]
        [TestCase(typeof (int[]), typeof (IEnumerable<int>), true)]
        [TestCase(typeof (string[]), typeof (string[]), true)]
        [TestCase(typeof (string), typeof (string), true)]
        [TestCase(typeof (int), typeof (long), true)]
        [TestCase(typeof (int), typeof (string), true)]
        [TestCase(typeof (int), typeof (double), true)]
        [TestCase(typeof (TestClassE), typeof (TestClassA), true)]
        [TestCase(typeof (TestClassA), typeof (TestClassE), false)]
        [TestCase(typeof (TestClassB), typeof (TestClassD), false)]
        [TestCase(typeof (string), typeof (TestClassC), false)]
        [TestCase(typeof (DateTime?), typeof (DateTime?), true)]
        public void DoCanConverTest(Type fromType, Type toType, bool expected)
        {
            var target = new ValueConverter();

            bool result = target.CanConvert(fromType, toType);
            result.ShouldBe(expected);
        }

        [TestCase(2, 2)]
        [TestCase("2", "2")]
        [TestCase(2, "2")]
        [TestCase("2", 2)]
        [TestCase(5, 5.0)]
        [TestCase(3, 3f)]
        [TestCase(7, 7d)]
        [TestCase(10, (long) 10)]
        [TestCase((long) 10, 10)]
        [TestCase(11.0, 11)]
        [TestCase(3.14, 3)]
        public void DoConvertTest(object value, object expected)
        {
            DoConvertTest(value, expected, expected.GetType());
        }

        [TestCase((TestClassA) null, null, typeof (TestClassB))]
        public void DoConvertTest(object value, object expected, Type expectedType)
        {
            var target = new ValueConverter();
            object actual = target.Convert(value, expectedType);
            actual.ShouldBe(expected);
        }

        [TestCase(typeof (DateTime))]
        [TestCase(typeof (int))]
        [TestCase(typeof (double))]
        [TestCase(typeof (decimal))]
        [TestCase(typeof (byte))]
        public void DoConvert_NullableNull_ConvertsToNull(Type innerType)
        {
            var target = new ValueConverter();
            Type genericNullableType = typeof (Nullable<>);
            Type concreteType = genericNullableType.MakeGenericType(innerType);
            object value = Activator.CreateInstance(concreteType);
            object actual = target.Convert(value, concreteType);
            actual.ShouldBe(null);
        }

        [Test]
        public void ConvertFailureTest()
        {
            var target = new ValueConverter();
            Should.Throw<InvalidOperationException>(() => target.Convert(null, typeof (int)));
        }

        [Test]
        public void DoConvert_NullableNotNull_ConvertsToNull()
        {
            var target = new ValueConverter();
            DateTime? value = DateTime.Now;
            object actual = target.Convert(value, typeof (DateTime?));
            actual.ShouldBe(value);
            actual.ShouldNotBeSameAs(value);
        }
    }
}