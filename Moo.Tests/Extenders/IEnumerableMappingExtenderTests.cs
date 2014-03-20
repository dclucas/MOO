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

using System;
using System.Collections.Generic;
using FakeItEasy;
using Moo.Extenders;
using NUnit.Framework;

namespace Moo.Tests.Extenders
{
    [TestFixture]
    public class IEnumerableMappingExtenderTests
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void MapAll_NullMapper_Throws()
        {
            var target = new TestClassC[1];
            target.MapAll((IMapper<TestClassC, TestClassA>) null);
        }

        [Test]
        public void MapAll_ValidInput_CallsDefaultRepository()
        {
            MappingRepository.Default.Clear();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassE, TestClassC>>();
            var target = new TestClassE[10];
            var output = new TestClassC[10];
            A.CallTo(() => mapperMock.MapMultiple(target)).Returns(output);
            MappingRepository.Default.AddMapper(mapperMock);
            IEnumerable<TestClassC> result = target.MapAll<TestClassE, TestClassC>();
            Assert.AreEqual(output, result);
            MappingRepository.Default.Clear();
        }

        [Test]
        public void MapAll_ValidInput_CallsMapper()
        {
            var mapperMock = A.Fake<IMapper<TestClassA, TestClassB>>();
            var target = new TestClassA[10];
            var output = new TestClassB[10];
            A.CallTo(() => mapperMock.MapMultiple(target)).Returns(output);
            IEnumerable<TestClassB> result = target.MapAll(mapperMock);
            Assert.AreEqual(output, result);
        }

        [Test]
        public void MapAll_ValidInput_CallsRepository()
        {
            var mapperMock = A.Fake<IExtensibleMapper<TestClassE, TestClassC>>();
            var target = new TestClassE[10];
            var output = new TestClassC[10];
            A.CallTo(() => mapperMock.MapMultiple(target)).Returns(output);
            var repoMock = A.Fake<IMappingRepository>();
            A.CallTo(() => repoMock.ResolveMapper<TestClassE, TestClassC>()).Returns(mapperMock);
            IEnumerable<TestClassC> result = target.MapAll<TestClassE, TestClassC>(repoMock);
            Assert.AreEqual(output, result);
        }
    }
}