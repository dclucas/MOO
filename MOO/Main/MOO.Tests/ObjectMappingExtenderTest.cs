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
    public class ObjectMappingExtenderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws_1()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>(new TestClassB());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws_2()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws_3()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>(new Mock<IMapper>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws_4()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>(new Mock<IMappingRepository>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullSource_Throws_5()
        {
            TestClassA source = null;
            source.MapTo<TestClassB>(new TestClassB(), new Mock<IMapper>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullTarget_Throws_1()
        {
            var source = new TestClassB(); ;
            source.MapTo<TestClassC>((TestClassC)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullTarget_Throws_2()
        {
            var source = new TestClassB(); ;
            source.MapTo<TestClassC>((TestClassC)null, new Mock<IMapper>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullTarget_Throws_3()
        {
            var source = new TestClassB(); ;
            source.MapTo<TestClassC>((TestClassC)null, new Mock<IMappingRepository>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullRepo_Throws_1()
        {
            var source = new TestClassD(); ;
            source.MapTo<TestClassA>(new TestClassA(), (IMappingRepository)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullRepo_Throws_2()
        {
            var source = new TestClassD(); ;
            source.MapTo<TestClassA>((IMappingRepository)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullMapper_Throws_1()
        {
            var source = new TestClassE(); ;
            source.MapTo<TestClassB>((IMapper)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTo_NullMapper_Throws_2()
        {
            var source = new TestClassE(); ;
            source.MapTo<TestClassB>(new TestClassB(), (IMapper)null);
        }

        [TestMethod]
        public void MapTo_ValidInput_UsesDefaultRepo()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source)).Returns(expected);
            MappingRepository.Default.Clear();
            MappingRepository.Default.AddMapper<TestClassC, TestClassB>(mapperMock.Object);
            var actual = source.MapTo<TestClassB>();
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            MappingRepository.Default.Clear();
        }

        [TestMethod]
        public void MapTo_ValidInputAndTarget_UsesDefaultRepo()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source, (object)expected)).Returns(expected);
            MappingRepository.Default.Clear();
            MappingRepository.Default.AddMapper<TestClassC, TestClassB>(mapperMock.Object);
            var actual = source.MapTo<TestClassB>(expected);
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            MappingRepository.Default.Clear();
        }

        [TestMethod]
        public void MapTo_ValidInput_UsesProvidedRepo()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source)).Returns(expected);
            var repoMock = new Mock<IMappingRepository>(MockBehavior.Strict);
            repoMock.Setup(m => m.ResolveMapper(typeof(TestClassC), typeof(TestClassB))).Returns(mapperMock.Object);
            var actual = source.MapTo<TestClassB>(repoMock.Object);
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            repoMock.VerifyAll();
            MappingRepository.Default.Clear();
        }

        [TestMethod]
        public void MapTo_ValidInputAndTarget_UsesProvidedRepo()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source, (object)expected)).Returns(expected);
            var repoMock = new Mock<IMappingRepository>(MockBehavior.Strict);
            repoMock.Setup(m => m.ResolveMapper(typeof(TestClassC), typeof(TestClassB))).Returns(mapperMock.Object);
            var actual = source.MapTo<TestClassB>(expected, repoMock.Object);
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            repoMock.VerifyAll();
            MappingRepository.Default.Clear();
        }

        [TestMethod]
        public void MapTo_ValidInput_UsesProvidedMapper()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source)).Returns(expected);
            var actual = source.MapTo<TestClassB>(mapperMock.Object);
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            MappingRepository.Default.Clear();
        }

        [TestMethod]
        public void MapTo_ValidInputAndTarget_UsesProvidedMapper()
        {
            var source = new TestClassC();
            var mapperMock = new Mock<IExtensibleMapper<TestClassC, TestClassB>>(MockBehavior.Strict);
            var expected = new TestClassB();
            mapperMock.Setup(m => m.Map((object)source, (object)expected)).Returns(expected);
            var actual = source.MapTo<TestClassB>(expected, mapperMock.Object);
            Assert.AreEqual(expected, actual);
            mapperMock.VerifyAll();
            MappingRepository.Default.Clear();
        }
    }
}