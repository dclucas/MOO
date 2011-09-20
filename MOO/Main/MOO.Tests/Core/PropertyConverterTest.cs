using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Core;

namespace Moo.Tests.Core
{
    /// <summary>
    /// This is a test class for PropertyMatcherTest and is intended
    /// targetProperty contain all PropertyMatcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PropertyConverterTest
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

        #region Methods (8)

        // Public Methods (7) 

        [TestMethod()]
        public void ConvertTestComplex()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            source.InnerClass = new TestClassC();
            var expected = "test";
            source.InnerClass.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("InnerClass");
            TestClassB targetObject = new TestClassB();
            targetObject.InnerClassName = "wrongString";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("InnerClassName");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.InnerClassName);
            Assert.AreEqual(expected, source.InnerClass.Name);
        }

        /// <summary>
        /// A test for Convert
        ///</summary>
        [TestMethod()]
        public void ConvertTestSimple()
        {
            PropertyConverter target = new PropertyConverter();
            TestClassA source = new TestClassA();
            var expected = "test";
            source.Name = expected;
            PropertyInfo fromProperty = typeof(TestClassA).GetProperty("Name");
            TestClassB targetObject = new TestClassB();
            targetObject.Name = "wrongString";
            PropertyInfo toProperty = typeof(TestClassB).GetProperty("Name");
            target.Convert(source, fromProperty, targetObject, toProperty);
            Assert.AreEqual(expected, targetObject.Name);
            Assert.AreEqual(expected, source.Name);
        }

        [TestMethod]
        public void PropertiesMatchFlattenNegativeTest()
        {
            TestMatch("InnerClass", "InnerClassCode", false);
        }

        [TestMethod]
        public void PropertiesMatchFlattenTest()
        {
            TestMatch("InnerClass", "InnerClassName", true);
        }

        [TestMethod]
        public void PropertiesMatchSimpleNegativeTest()
        {
            TestMatch("Code", "Code", false);
        }

        /// <summary>
        /// A test for PropertiesMatch
        ///</summary>
        [TestMethod()]
        public void PropertiesMatchSimpleTest()
        {
            TestMatch("Name", "Name", true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyConverterNegativeTest()
        {
            PropertyConverter target = new PropertyConverter();
            var source = new TestClassA();
            var sourceProperty = source.GetType().GetProperty("InnerClass");
            var result = new TestClassB();
            var resultProperty = result.GetType().GetProperty("InnerClassCode");
            source.InnerClass = new TestClassC();
            target.Convert(
                source,
                sourceProperty,
                target,
                resultProperty,
                false);
        }

        // Private Methods (1) 

        private void TestMatch(string fromProp, string toProp, bool expected)
        {
            PropertyConverter target = new PropertyConverter();
            PropertyInfo from = typeof(TestClassA).GetProperty(fromProp);
            PropertyInfo to = typeof(TestClassB).GetProperty(toProp);
            bool actual;
            string finalName;
            actual = target.CanConvert(from, to, out finalName);
            Assert.AreEqual(expected, actual);
        }

        #endregion Methods
    }
}