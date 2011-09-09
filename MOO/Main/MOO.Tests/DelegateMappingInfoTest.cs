using Moo.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moo;

namespace Moo.Tests
{
    
    
    /// <summary>
    /// This is a test class for DelegateMappingInfoTest and is intended
    /// targetMemberName contain all DelegateMappingInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DelegateMappingInfoTest
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
    }
}
