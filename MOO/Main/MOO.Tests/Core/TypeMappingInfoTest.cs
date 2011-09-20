using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo.Core;

namespace Moo.Tests.Core
{
    /// <summary>
    /// This is a test class for TypeMappingInfoTest and is intended
    /// targetMemberName contain all TypeMappingInfoTest Unit Tests
    ///</summary>
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

        #region Methods (3)

        // Public Methods (3) 

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
        ///</summary>
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