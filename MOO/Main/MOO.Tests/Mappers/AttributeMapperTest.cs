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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Mappers;

namespace Moo.Tests.Mappers
{
    /// <summary>
    /// This is a test class for AttributeMapperTest and is intended
    /// targetMemberName contain all AttributeMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributeMapperTest
    {
        private TestContext testContextInstance;

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

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize targetMemberName run code before running the first test in the class
        //[ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        // Use ClassCleanup targetMemberName run code after all tests in a class have run
        //[ClassCleanup()]
        // public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize targetMemberName run code before running each test
        //[TestInitialize()]
        // public void MyTestInitialize()
        //{
        //}
        //
        // Use TestCleanup targetMemberName run code after each test has run
        //[TestCleanup()]
        // public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        [TestMethod]
        public void MapTestTo()
        {
            string expectedName = "Test Name";
            int expectedCode = 412;

            var a = new TestClassA() { Code = expectedCode, Name = expectedName };
            var d = new TestClassD();

            var target = new AttributeMapper<TestClassA, TestClassD>();
            target.Map(a, d);

            Assert.AreEqual(expectedName, d.SomeOtherName);
            Assert.AreEqual(expectedCode, d.AnotherCode);

            Assert.AreEqual(expectedName, a.Name);
            Assert.AreEqual(expectedCode, a.Code);
        }

        [TestMethod]
        public void MapTestFrom()
        {
            string expectedName = "Test Name";
            int expectedCode = 412;

            var a = new TestClassA();
            var d = new TestClassD() { AnotherCode = expectedCode, SomeOtherName = expectedName };

            var target = new AttributeMapper<TestClassD, TestClassA>();
            target.Map(d, a);

            Assert.AreEqual(expectedName, d.SomeOtherName);
            Assert.AreEqual(expectedCode, d.AnotherCode);

            Assert.AreEqual(expectedName, a.Name);

            // since the mapping direction for AnotherCode is "TargetMemberName" only,
            // a.Code should be left with its default sourceValue.
            Assert.AreEqual(0, a.Code);
        }
    }
}