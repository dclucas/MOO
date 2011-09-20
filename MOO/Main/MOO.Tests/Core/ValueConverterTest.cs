using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Core;

namespace Moo.Tests.Core
{
    /// <summary>
    /// This is a test class for ValueConverterTest and is intended
    /// targetMemberName contain all ValueConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValueConverterTest
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

        #region Methods (7)

        // Public Methods (4) 

        /// <summary>
        /// A test for CanConvert
        ///</summary>
        [TestMethod()]
        public void CanConvertNegativeTest()
        {
            ValueConverter target = new ValueConverter();
            DoCanConverTest(typeof(TestClassA), typeof(TestClassE), false);
            DoCanConverTest(typeof(TestClassB), typeof(TestClassD), false);
            DoCanConverTest(typeof(string), typeof(TestClassC), false);
        }

        /// <summary>
        /// A test for CanConvert
        ///</summary>
        [TestMethod()]
        public void CanConvertTest()
        {
            DoCanConverTest(typeof(string), typeof(string), true);
            DoCanConverTest(typeof(int), typeof(long), true);
            DoCanConverTest(typeof(int), typeof(string), true);
            DoCanConverTest(typeof(int), typeof(double), true);
            DoCanConverTest(typeof(TestClassE), typeof(TestClassA), true);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void ConvertFailureTest()
        {
            ValueConverter target = new ValueConverter();
            var res = target.Convert(null, typeof(int));
        }

        /// <summary>
        /// A test for Convert
        ///</summary>
        [TestMethod()]
        public void ConvertTest()
        {
            DoConvertTest(2, 2);
            DoConvertTest("2", "2");
            DoConvertTest(2, "2");
            DoConvertTest("2", 2);
            DoConvertTest(5, 5.0);
            DoConvertTest(3, 3f);
            DoConvertTest(7, 7d);
            DoConvertTest(10, (long)10);
            DoConvertTest((long)10, 10);
            DoConvertTest(11.0, 11);
            DoConvertTest(3.14, 3);
            DoConvertTest((TestClassA)null, null, typeof(TestClassB));
        }

        // Private Methods (3) 

        private void DoCanConverTest(Type fromType, Type toType, bool expected)
        {
            ValueConverter target = new ValueConverter();

            Assert.AreEqual(
                expected,
                target.CanConvert(fromType, toType),
                String.Format("CanConvert should have returned {0} for types {1} and {2}",
                    expected, fromType, toType));
        }

        private void DoConvertTest(object value, object expected)
        {
            DoConvertTest(value, expected, expected.GetType());
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