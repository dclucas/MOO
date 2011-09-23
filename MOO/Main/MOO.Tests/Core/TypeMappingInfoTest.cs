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
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo.Core;

    /// <summary>
    /// This is a test class for TypeMappingInfoTest and is intended
    /// targetMemberName contain all TypeMappingInfoTest Unit Tests
    /// </summary>
    [TestClass()]
    public class TypeMappingInfoTest
    {
        #region Fields (1)

        private TestContext testContextInstance;

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
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

        #region Methods (3)

        // Public Methods (3) 

        [TestMethod]
        public void AddOverwriteTest()
        {
            var m1 = new DelegateMappingInfo<TestClassD, TestClassC>("Prop1", "Prop2", (f, t) => { });
            var m2 = new DelegateMappingInfo<TestClassD, TestClassC>("Prop3", "Prop2", (f, t) => { });
            var target = new TypeMappingInfo<TestClassD, TestClassC>();
            target.Add(m1);
            target.Add(m2);
            Assert.IsFalse(target.GetMappings().Contains(m1));
            Assert.IsTrue(target.GetMappings().Contains(m2));
        }

        [TestMethod]
        public void AddTest()
        {
            var m1 = new DelegateMappingInfo<TestClassE, TestClassB>("Prop1", "Prop2", (f, t) => { });
            var m2 = new DelegateMappingInfo<TestClassE, TestClassB>("Prop1", "Prop3", (f, t) => { });
            var target = new TypeMappingInfo<TestClassE, TestClassB>();
            target.Add(m1);
            target.Add(m2);
            Assert.IsTrue(target.GetMappings().Contains(m1));
            Assert.IsTrue(target.GetMappings().Contains(m2));
        }

        /// <summary>
        /// A test for GetMappings
        /// </summary>
        [TestMethod()]
        public void GetMappingsEmptyTest()
        {
            var target = new TypeMappingInfo<TestClassA, TestClassC>();
            var actual = target.GetMappings();
            Assert.IsFalse(actual.Any());
        }

        #endregion Methods
    }
}