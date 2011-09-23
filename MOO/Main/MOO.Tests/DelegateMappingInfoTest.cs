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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo;
using Moo.Core;

namespace Moo.Tests
{
    /// <summary>
    /// This is a test class for DelegateMappingInfoTest and is intended
    /// targetMemberName contain all DelegateMappingInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DelegateMappingInfoTest
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

        #region Methods (1)

        // Public Methods (1) 

        [TestMethod()]
        public void MapTest()
        {
            bool executed = false;

            var target = new DelegateMappingInfo<TestClassC, TestClassD>(
                "sourceMemberName", "targetMemberName",
                (f, t) => executed = true);
            target.Map(new TestClassC(), new TestClassD());

            Assert.IsTrue(executed);
        }

        #endregion Methods
    }
}