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

using System.Collections;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Moo.Mappers;
using NUnit.Framework;

namespace Moo.Tests
{
    /// <summary>
    ///     This is a test class for MappingRepositoryTest and is intended
    ///     targetProperty contain all MappingRepositoryTest Unit Tests
    /// </summary>
    [TestFixture(TypeArgs = new[] {typeof (TestClassA), typeof (TestClassF)})]
    public class MappingRepositoryTest<TSource, TTarget>
    {
        [Test]
        public void AddMapperTest()
        {
            IExtensibleMapper<TSource, TTarget> expected = new ManualMapper<TSource, TTarget>();
            var target = new MappingRepository();
            target.AddMapper(expected);
            IExtensibleMapper<TSource, TTarget> actual = target.ResolveMapper<TSource, TTarget>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddMappingGeneric_DefaultCase_RedirectsToMapper()
        {
            var target = new MappingRepository();
            var mapperMock = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            target.AddMapper(mapperMock);
            string sourceMemberName = "source";
            string targetMemberName = "target";
            var mappingAction = new MappingAction<TSource, TTarget>((s, t) => { });

            target.AddMappingAction(sourceMemberName, targetMemberName, mappingAction);

            A.CallTo(() => mapperMock.AddMappingAction(sourceMemberName, targetMemberName, mappingAction))
                .MustHaveHappened();
        }

        [Test]
        public void ClearTest()
        {
            IExtensibleMapper<TSource, TTarget> mapper = new ManualMapper<TSource, TTarget>();
            var target = new MappingRepository();
            target.AddMapper(mapper);
            target.Clear();
            FieldInfo mappersField = target.GetType()
                .GetField("_mappers", BindingFlags.NonPublic | BindingFlags.Instance);
            var mappers = (IEnumerable) mappersField.GetValue(target);
            Assert.IsFalse(mappers.Cast<object>().Any());
        }

        [Test]
        public void GetDefaultTest()
        {
            Assert.IsNotNull(MappingRepository.Default);
        }

        [Test]
        public void ResolveMapper2_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            IMapper mapper = target.ResolveMapper(typeof (TTarget), typeof (TSource));
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOf<CompositeMapper<TTarget, TSource>>(mapper);
            Assert.IsTrue(((CompositeMapper<TTarget, TSource>) mapper).InnerMappers.Any());
        }

        [Test]
        public void ResolveMapper_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            IExtensibleMapper<TTarget, TSource> mapper = target.ResolveMapper<TTarget, TSource>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOf<CompositeMapper<TTarget, TSource>>(mapper);
            Assert.IsTrue(((CompositeMapper<TTarget, TSource>) mapper).InnerMappers.Any());
        }
    }
}