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
    public class IEnumerableMappingExtenderTest
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
            target.MapAll<TestClassC, TestClassA>((IMapper<TestClassC, TestClassA>)null);
        }

        [TestMethod]
        public void MapAll_ValidInput_CallsRepository()
        {
            var mapperMock = new Mock<IExtensibleMapper<TestClassE, TestClassC>>(MockBehavior.Strict);
            var target = new TestClassE[10];
            var output = new TestClassC[10];
            mapperMock.Setup(m => m.MapMultiple(target)).Returns(output);
            var repoMock = new Mock<IMappingRepository>(MockBehavior.Strict);
            repoMock.Setup(m => m.ResolveMapper<TestClassE, TestClassC>()).Returns(mapperMock.Object);
            var result = target.MapAll<TestClassE, TestClassC>(repoMock.Object);
            Assert.AreEqual(output, result);
            repoMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [TestMethod]
        public void MapAll_ValidInput_CallsDefaultRepository()
        {
            MappingRepository.Default.Clear();
            var mapperMock = new Mock<IExtensibleMapper<TestClassE, TestClassC>>(MockBehavior.Strict);
            var target = new TestClassE[10];
            var output = new TestClassC[10];
            mapperMock.Setup(m => m.MapMultiple(target)).Returns(output);
            MappingRepository.Default.AddMapper<TestClassE, TestClassC>(mapperMock.Object);
            var result = target.MapAll<TestClassE, TestClassC>();
            Assert.AreEqual(output, result);
            mapperMock.VerifyAll();
            MappingRepository.Default.Clear();
        }
    }
}