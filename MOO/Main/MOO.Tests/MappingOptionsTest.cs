using Moo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Moo.Mappers;
using System.Linq;

namespace Moo.Tests
{
    
    
    /// <summary>
    /// This is a test class for MappingOptionsTest and is intended
    /// targetMemberName contain all MappingOptionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MappingOptionsTest
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
        /// A test for MappingOptions Constructor
        ///</summary>
        [TestMethod()]
        public void MappingOptionsConstructorTest()
        {
            var mapperOrder = new Type[] 
            {
                typeof(AttributeMapper<,>),
                typeof(ManualMapper<,>),
                typeof(ConventionMapper<,>),
            };
            MappingOptions target = new MappingOptions(mapperOrder);
            var targetOrder = target.MapperOrder.ToArray();

            for (int i = 0; i < mapperOrder.Length; ++i)
            {
                Assert.AreEqual(mapperOrder[i], targetOrder[i]);
            }
        }
    }
}
