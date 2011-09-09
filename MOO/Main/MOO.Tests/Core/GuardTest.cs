using Moo.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moo.Tests
{
    
    
    /// <summary>
    /// This is a test class for GuardTest and is intended
    /// targetMemberName contain all GuardTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GuardTest
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
        #endregion


        /// <summary>
        /// A test for CheckArgumentNotNull
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckArgumentNotNullTest()
        {
            Guard.CheckArgumentNotNull(null, "argumentName");
        }

        /// <summary>
        /// A test for CheckEnumerableNotNullOrEmpty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckEnumerableNotNullOrEmptyNullTest()
        {
            Guard.CheckEnumerableNotNullOrEmpty(null, "argumentName");
        }

        /// <summary>
        /// A test for CheckEnumerableNotNullOrEmpty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckEnumerableNotNullOrEmptyEmptyTest()
        {
            Guard.CheckEnumerableNotNullOrEmpty(new object[0], "argumentName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrueForAllNegativeTest()
        {
            Guard.TrueForAll<bool>(
                new bool[] { true, true, true, false}, 
                "argumentName", 
                b => b);
        }

        [TestMethod]
        public void TrueForAllTest()
        {
            var passed = new bool[4];

            Guard.TrueForAll<int>(
                new int[] { 0, 1, 2, 3 },
                "argumentName",
                b => passed[b] = true);

            Assert.IsTrue(passed.All(b => b));
        }

    }
}
