using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Core;

namespace Moo.Tests.Core
{
    /// <summary>
    /// This is a test class for GuardTest and is intended
    /// targetMemberName contain all GuardTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GuardTest
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

        #region Methods (5)

        // Public Methods (5) 

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
        [ExpectedException(typeof(ArgumentException))]
        public void CheckEnumerableNotNullOrEmptyEmptyTest()
        {
            Guard.CheckEnumerableNotNullOrEmpty(new object[0], "argumentName");
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrueForAllNegativeTest()
        {
            Guard.TrueForAll<bool>(
                new bool[] { true, true, true, false },
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

        #endregion Methods
    }
}