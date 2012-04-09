// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Moo.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Moo;
    using Moq;

    [TestFixture]
    public class IEnumerableMappingExtenderTest
    {
        #region Methods (4)

        // Public Methods (4) 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapAll_NullMapper_Throws()
        {
            var target = new TestClassC[1];
            target.MapAll<TestClassC, TestClassA>((IMapper<TestClassC, TestClassA>)null);
        }

        [Test]
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

        [Test]
        public void MapAll_ValidInput_CallsMapper()
        {
            var mapperMock = new Mock<IMapper<TestClassA, TestClassB>>(MockBehavior.Strict);
            var target = new TestClassA[10];
            var output = new TestClassB[10];
            mapperMock.Setup(m => m.MapMultiple(target)).Returns(output);
            var result = target.MapAll<TestClassA, TestClassB>(mapperMock.Object);
            Assert.AreEqual(output, result);
        }

        [Test]
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

        #endregion Methods
    }
}