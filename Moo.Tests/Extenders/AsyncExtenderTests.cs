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

namespace Moo.Tests.Extenders
{
    using System;
    using FakeItEasy;
    using Moo.Extenders;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class AsyncExtenderTest
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullMapper_Throws_1()
        {
            var source = new TestClassE();
            await source.MapToAsync<TestClassB>((IMapper) null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullMapper_Throws_2()
        {
            var source = new TestClassE();
            await source.MapToAsync(new TestClassB(), (IMapper) null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullRepo_Throws_1()
        {
            var source = new TestClassD();
            await source.MapToAsync(new TestClassA(), (IMappingRepository) null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullRepo_Throws_2()
        {
            var source = new TestClassD();
            await source.MapToAsync<TestClassA>((IMappingRepository) null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullSource_Throws_1()
        {
            TestClassA source = null;
            await source.MapToAsync(new TestClassB());
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullSource_Throws_2()
        {
            TestClassA source = null;
            await source.MapToAsync<TestClassB>();
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullSource_Throws_3()
        {
            TestClassA source = null;
            await source.MapToAsync<TestClassB>(A.Fake<IMapper>());
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullSource_Throws_4()
        {
            TestClassA source = null;
            await source.MapToAsync<TestClassB>(A.Fake<IMappingRepository>());
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullSource_Throws_5()
        {
            TestClassA source = null;
            await source.MapToAsync(new TestClassB(), A.Fake<IMapper>());
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullTarget_Throws_1()
        {
            var source = new TestClassB();
            await source.MapToAsync((TestClassC) null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullTarget_Throws_2()
        {
            var source = new TestClassB();
            await source.MapToAsync((TestClassC) null, A.Fake<IMapper>());
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task MapTo_NullTarget_Throws_3()
        {
            var source = new TestClassB();
            await source.MapToAsync((TestClassC) null, A.Fake<IMappingRepository>());
        }

        [Test]
        public async Task MapTo_ValidInputAndTarget_UsesDefaultRepo()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map(source, (object) expected)).Returns(expected);
            MappingRepository.Default.Clear();
            MappingRepository.Default.AddMapper(mapperMock);
            TestClassB actual = await source.MapToAsync(expected);
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }

        [Test]
        public async Task MapTo_ValidInputAndTarget_UsesProvidedMapper()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map(source, (object) expected)).Returns(expected);
            TestClassB actual = await source.MapToAsync(expected, mapperMock);
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }

        [Test]
        public async Task MapTo_ValidInputAndTarget_UsesProvidedRepo()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map(source, (object) expected)).Returns(expected);
            var repoMock = A.Fake<IMappingRepository>();
            A.CallTo(() => repoMock.ResolveMapper(typeof (TestClassC), typeof (TestClassB))).Returns(mapperMock);
            TestClassB actual = await source.MapToAsync(expected, repoMock);
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }

        [Test]
        public async Task MapTo_ValidInput_UsesDefaultRepo()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map((object) source)).Returns(expected);
            MappingRepository.Default.Clear();
            MappingRepository.Default.AddMapper(mapperMock);
            var actual = await source.MapToAsync<TestClassB>();
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }

        [Test]
        public async Task MapTo_ValidInput_UsesProvidedMapper()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map((object) source)).Returns(expected);
            var actual = await source.MapToAsync<TestClassB>(mapperMock);
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }

        [Test]
        public async Task MapTo_ValidInput_UsesProvidedRepo()
        {
            var source = new TestClassC();
            var mapperMock = A.Fake<IExtensibleMapper<TestClassC, TestClassB>>();
            var expected = new TestClassB();
            A.CallTo(() => mapperMock.Map((object) source)).Returns(expected);
            var repoMock = A.Fake<IMappingRepository>();
            A.CallTo(() => repoMock.ResolveMapper(typeof (TestClassC), typeof (TestClassB))).Returns(mapperMock);
            var actual = await source.MapToAsync<TestClassB>(repoMock);
            Assert.AreEqual(expected, actual);
            MappingRepository.Default.Clear();
        }
    }
}