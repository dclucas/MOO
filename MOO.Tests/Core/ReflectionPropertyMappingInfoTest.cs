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
    /// This is a test class for ReflectionPropertyMappingInfoTest and is intended
    /// targetMemberName contain all ReflectionPropertyMappingInfoTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ReflectionPropertyMappingInfoTest
    {
        #region Methods

        [Test]
        public void MapTest()
        {
            TestClassA a = new TestClassA();
            TestClassC c = new TestClassC();
            var fromProp = typeof(TestClassA).GetProperty("Name");
            var toProp = typeof(TestClassA).GetProperty("Name");
            ConverterMock mock = new ConverterMock();
            bool executed = false;

            // TODO: refactor this to use FakeItEasy
            mock.ConvertAction = (f, fp, t, tp, s) =>
                {
                    Assert.AreEqual(c, f);
                    Assert.AreEqual(fromProp, fp);
                    Assert.AreEqual(a, t);
                    Assert.AreEqual(toProp, tp);
                    Assert.IsTrue(s);
                    executed = true;
                };
            var target = new ReflectionPropertyMappingInfo<TestClassC, TestClassA>(
                fromProp, toProp, true, mock);
            target.Map(c, a);

            Assert.IsTrue(executed);
        }

        #endregion Methods

        #region Nested Classes

        private class ConverterMock : PropertyConverter
        {
            #region Properties

            public Action<object, PropertyInfo, object, PropertyInfo, bool> ConvertAction { get; set; }

            #endregion Properties

            #region Methods

            public override void Convert(object source, PropertyInfo fromProperty, object target, PropertyInfo toProperty, bool strict)
            {
                this.ConvertAction(source, fromProperty, target, toProperty, strict);
            }

            #endregion Methods
        }

        #endregion Nested Classes
    }
}