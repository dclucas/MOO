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
    using NUnit.Framework;
    using Moo.Core;

    /// <summary>
    /// This is a test class for ValueConverterTest and is intended
    /// targetMemberName contain all ValueConverterTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ValueConverterTest
    {
        #region Methods

        /// <summary>
        /// A test for CanConvert
        /// </summary>
        [Test]
        public void CanConvertNegativeTest()
        {
            ValueConverter target = new ValueConverter();
            this.DoCanConverTest(typeof(TestClassA), typeof(TestClassE), false);
            this.DoCanConverTest(typeof(TestClassB), typeof(TestClassD), false);
            this.DoCanConverTest(typeof(string), typeof(TestClassC), false);
        }

        /// <summary>
        /// A test for CanConvert
        /// </summary>
        [Test]
        public void CanConvertTest()
        {
            this.DoCanConverTest(typeof(string), typeof(string), true);
            this.DoCanConverTest(typeof(int), typeof(long), true);
            this.DoCanConverTest(typeof(int), typeof(string), true);
            this.DoCanConverTest(typeof(int), typeof(double), true);
            this.DoCanConverTest(typeof(TestClassE), typeof(TestClassA), true);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ConvertFailureTest()
        {
            ValueConverter target = new ValueConverter();
            var res = target.Convert(null, typeof(int));
        }

        /// <summary>
        /// A test for Convert
        /// </summary>
        [Test]
        public void ConvertTest()
        {
            this.DoConvertTest(2, 2);
            this.DoConvertTest("2", "2");
            this.DoConvertTest(2, "2");
            this.DoConvertTest("2", 2);
            this.DoConvertTest(5, 5.0);
            this.DoConvertTest(3, 3f);
            this.DoConvertTest(7, 7d);
            this.DoConvertTest(10, (long)10);
            this.DoConvertTest((long)10, 10);
            this.DoConvertTest(11.0, 11);
            this.DoConvertTest(3.14, 3);
            this.DoConvertTest((TestClassA)null, null, typeof(TestClassB));
        }

        private void DoCanConverTest(Type fromType, Type toType, bool expected)
        {
            ValueConverter target = new ValueConverter();

            Assert.AreEqual(
                expected,
                target.CanConvert(fromType, toType),
                "CanConvert should have returned {0} for types {1} and {2}",
                expected,
                fromType,
                toType);
        }

        private void DoConvertTest(object value, object expected)
        {
            this.DoConvertTest(value, expected, expected.GetType());
        }

        private void DoConvertTest(object value, object expected, Type expectedType)
        {
            ValueConverter target = new ValueConverter();
            var actual = target.Convert(value, expectedType);
            Assert.AreEqual(expected, actual);
        }

        #endregion Methods
    }
}