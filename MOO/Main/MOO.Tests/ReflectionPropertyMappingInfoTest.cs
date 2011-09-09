using Moo.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Moo.Tests
{
    /// <summary>
    /// This is a test class for ReflectionPropertyMappingInfoTest and is intended
    /// targetMemberName contain all ReflectionPropertyMappingInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReflectionPropertyMappingInfoTest
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
            TestClassA a = new TestClassA();
            TestClassC c = new TestClassC();
            var fromProp = typeof(TestClassA).GetProperty("Name");
            var toProp = typeof(TestClassA).GetProperty("Name");
            ConverterMock mock = new ConverterMock();
            bool executed = false;
            // TODO: refactor this to use Moq
            mock.ConvertAction = (f, fp, t, tp, s) =>
                {
                    Assert.AreEqual(c, f);
                    Assert.AreEqual(fromProp, fp);
                    Assert.AreEqual(a, t);
                    Assert.AreEqual(toProp, tp);
                    Assert.IsTrue(s);
                    executed = true;
                };
            var target = new ReflectionPropertyMappingInfo<TestClassC, TestClassA>(
                fromProp, toProp, true, mock);
            target.Map(c, a);

            Assert.IsTrue(executed);
        }

        private class ConverterMock : PropertyConverter
        {
            public Action<object, PropertyInfo, object, PropertyInfo, bool>  ConvertAction { get; set; }

            public override void Convert(object source, PropertyInfo fromProperty, object target, PropertyInfo toProperty, bool strict)
            {
                ConvertAction(source, fromProperty, target, toProperty, strict);
            }
        }
    }
}
