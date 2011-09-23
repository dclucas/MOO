/*-----------------------------------------------------------------------------
Copyright 2010 Diogo Lucas

This file is part of Moo.

Foobar is free software: you can redistribute it and/or modify it under the
terms of the GNU General Public License as published by the Free Software
Foundation, either version 3 of the License, or (at your option) any later
version.

Moo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Moo. If not, see http://www.gnu.org/licenses/.
---------------------------------------------------------------------------- */

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
        #region Methods (18)

        // Public Methods (18) 

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

        #endregion Methods
    }
}