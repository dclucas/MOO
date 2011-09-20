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
    }
}