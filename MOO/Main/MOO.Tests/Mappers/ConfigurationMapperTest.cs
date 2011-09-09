using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Configuration;
using Moo.Mappers;
using System;

namespace Moo.Tests.Mappers
{
    
    
    /// <summary>
    /// This is a test class for ConfigurationMapperTest and is intended
    /// targetMemberName contain all ConfigurationMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationMapperTest
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


        [TestMethod()]
        public void GetTypeMappingTest()
        {
            var actual = ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping();
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetTypeMappingNoSectionTest()
        {
            var actual = ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping("thisConfigDoesNotExist");
            Assert.IsNull(actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTypeMappingNullSectionTest()
        {
            ConfigurationMapper<TestClassA, TestClassB>.GetTypeMapping(null);
        }

        [TestMethod]
        public void MapTest()
        {
            ConfigurationMapper<TestClassA, TestClassB> target = new ConfigurationMapper<TestClassA, TestClassB>();
            TestClassA from = new TestClassA() { Name = "test" };
            TestClassB to = new TestClassB();
            target.Map(from, to);
            Assert.AreEqual(from.Name, to.InnerClassName);
        }

    }
}
