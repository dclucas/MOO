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
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    using Moo.Mappers;

    using NUnit.Framework;
    using Moq;

    /// <summary>
    /// This is a test class for MappingRepositoryTest and is intended
    /// targetProperty contain all MappingRepositoryTest Unit Tests
    /// </summary>
    [TestFixture(TypeArgs = new Type[] { typeof(TestClassA), typeof(TestClassF) })]
    public class MappingRepositoryTest<TSource, TTarget>
    {
        #region Methods (5)

        // Public Methods (5) 

        [Test]
        public void AddMapperTest()
        {
            IExtensibleMapper<TSource, TTarget> expected = new ManualMapper<TSource, TTarget>();
            var target = new MappingRepository();
            target.AddMapper(expected);
            var actual = target.ResolveMapper<TSource, TTarget>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ClearTest()
        {
            IExtensibleMapper<TSource, TTarget> mapper = new ManualMapper<TSource, TTarget>();
            var target = new MappingRepository();
            target.AddMapper(mapper);
            target.Clear();
            var mappersField = target.GetType().GetField("mappers", BindingFlags.NonPublic | BindingFlags.Instance);
            var mappers = (IEnumerable)mappersField.GetValue(target);
            Assert.IsFalse(mappers.Cast<object>().Any());
        }

        [Test]
        public void GetDefaultTest()
        {
            Assert.IsNotNull(MappingRepository.Default);
        }

        [Test]
        public void ResolveMapper_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper<TTarget, TSource>();
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOf<CompositeMapper<TTarget, TSource>>(mapper);
            Assert.IsTrue(((CompositeMapper<TTarget, TSource>)mapper).InnerMappers.Any());
        }

        [Test]
        public void ResolveMapper2_ExistingMapper_ResolvesCorrectly()
        {
            var target = new MappingRepository();
            var mapper = target.ResolveMapper(typeof(TTarget), typeof(TSource));
            Assert.IsNotNull(mapper);
            Assert.IsInstanceOf<CompositeMapper<TTarget, TSource>>(mapper);
            Assert.IsTrue(((CompositeMapper<TTarget, TSource>)mapper).InnerMappers.Any());
        }

        [Test]
        public void ResolveMapper_WithConstructorInfo_PassesArgument()
        {
            var mappers = new Type[] { typeof(Mapper1<TSource, TTarget>), typeof(Mapper2<TSource, TTarget>) };
            var options = new MappingOptions(mappers);
            var target = new MappingRepository(options);
            var inclusions = new MapperInclusion[]
                {
                    new MapperInclusion<TestClassE, TestClassF>()
                };

            var result = (CompositeMapper<TSource, TTarget>)target.ResolveMapper<TSource, TTarget>(inclusions);

            Assert.IsNotNull(result);
            var innerMappers = result.InnerMappers.ToArray();
            CollectionAssert.IsNotEmpty(innerMappers);
            Assert.AreEqual(mappers.Length, innerMappers.Length);
            for (var i = 0; i < mappers.Length; ++i)
            {
                Assert.AreEqual(mappers[i], innerMappers[i].GetType());
            }

            var mapper2 = (Mapper2<TSource, TTarget>)innerMappers[1];
            CollectionAssert.AreEqual(inclusions, mapper2.ConstructionInfo.IncludedMappers);
            Assert.AreEqual(target, mapper2.ConstructionInfo.ParentRepo);
        }

        [Test]
        public void AddMappingGeneric_DefaultCase_RedirectsToMapper()
        {
            var target = new MappingRepository();
            var mapperMock = new Mock<IExtensibleMapper<TSource, TTarget>>();
            target.AddMapper<TSource, TTarget>(mapperMock.Object);
            var sourceMemberName = "source";
            var targetMemberName = "target";
            var mappingAction = new MappingAction<TSource, TTarget>((s, t) => { });
            mapperMock.Setup(m => m.AddMappingAction(sourceMemberName, targetMemberName, mappingAction));

            target.AddMappingAction(sourceMemberName, targetMemberName, mappingAction);

            mapperMock.VerifyAll();
        }

        #endregion Methods
    }
}