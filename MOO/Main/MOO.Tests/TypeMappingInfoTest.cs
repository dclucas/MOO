using Moo.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moo.Tests
{
    
    
    /// <summary>
    /// This is a test class for TypeMappingInfoTest and is intended
    /// targetMemberName contain all TypeMappingInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TypeMappingInfoTest
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
        /// A test for GetMappings
        ///</summary>
        [TestMethod()]
        public void GetMappingsEmptyTest()
        {
            var target = new TypeMappingInfo<TestClassA, TestClassC>(); 
            var actual = target.GetMappings();
            Assert.IsFalse(actual.Any());
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

    }
}
