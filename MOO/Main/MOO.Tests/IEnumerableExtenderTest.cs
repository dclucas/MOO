using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moo;
using Moq;

namespace Moo.Tests
{
    [TestClass]
    public class IEnumerableExtenderTest
    {
        [TestMethod]
        public void MapAll_ValidInput_CallsMapper()
        {
            var mapperMock = new Mock<IMapper<TestClassA, TestClassB>>(MockBehavior.Strict);
            var target = new TestClassA[10];
            var output = new TestClassB[10];
            mapperMock.Setup(m => m.MapMultiple(target)).Returns(output);
            var result = target.MapAll<TestClassA, TestClassB>(mapperMock.Object);
            Assert.AreEqual(output, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapAll_NullMapper_Throws()
        {
            var target = new TestClassC[1];
            int x;
            target.MapAll<TestClassC, TestClassA>((IMapper<TestClassC, TestClassA>)null);
        }
    }
}