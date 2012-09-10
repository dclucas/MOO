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
namespace Moo.Tests.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using NUnit.Framework;
    using Moo.Core;
    using Moo.Mappers;
    using Ploeh.AutoFixture;
    using Shouldly;

    /// <summary>
    /// This is a test class for CompositeMapperTest and is intended
    /// targetProperty contain all CompositeMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class CompositeMapperTest
    {
        #region Methods (5)

        // Public Methods (5) 

        [Test]
        public void Map_MockedInternalMappers_Redirects()
        {
            var mapperMocks = A.CollectionOfFake<BaseMapper<TestClassB, TestClassD>>(5).ToArray();
            var results = new List<IEnumerable<MemberMappingInfo<TestClassB, TestClassD>>>();
            for (int i = 0; i < mapperMocks.Length; ++i)
            {
                var res = new MemberMappingInfo<TestClassB, TestClassD>[]
                {
                    A.Fake<MemberMappingInfo<TestClassB, TestClassD>>(
                        o => o.WithArgumentsForConstructor(
                            new object[] 
                            {
                                Guid.NewGuid().ToString(), 
                                Guid.NewGuid().ToString()
                            }))
                };

                results.Add(res);
                A.CallTo(() => mapperMocks[i].GetMappings())
                    .Returns(res);
            }
            var target = new CompositeMapper<TestClassB, TestClassD>(mapperMocks);
            var source = new TestClassB();
            var result = new TestClassD();

            target.Map(source, result);

            foreach (var m in results.SelectMany(e => e))
            {
                A.CallTo(() => m.Map(source, result)).MustHaveHappened();
            }
        }

        [Test]
        public void AddMapping_MockedInternalMappers_Redirects()
        {
            var mapperMocks = A.CollectionOfFake<BaseMapper<TestClassB, TestClassD>>(5).ToArray();
            var extMock = A.Fake<BaseMapper<TestClassB, TestClassD>>(
                o => o.Implements(typeof(IExtensibleMapper<TestClassB, TestClassD>)));
            var results = new List<IEnumerable<MemberMappingInfo<TestClassB, TestClassD>>>();
            var rnd = NUnit.Framework.Randomizer.GetRandomizer(typeof(CompositeMapper<,>).GetMethod("AddMapping"));
            var extPos = rnd.Next(mapperMocks.Length);
            for (int i = 0; i < mapperMocks.Length; ++i)
            {
                var res = new MemberMappingInfo<TestClassB, TestClassD>[]
                {
                    A.Fake<MemberMappingInfo<TestClassB, TestClassD>>(
                        o => o.WithArgumentsForConstructor(
                            new object[] 
                            {
                                Guid.NewGuid().ToString(), 
                                Guid.NewGuid().ToString()
                            }))
                };

                if (i == extPos)
                {
                    mapperMocks[i] = extMock;
                }
                results.Add(res);
                A.CallTo(() => mapperMocks[i].GetMappings())
                    .Returns(res);
            }
            var target = new CompositeMapper<TestClassB, TestClassD>(mapperMocks);
            var source = new TestClassB();
            var result = new TestClassD();
            MappingAction<TestClassB, TestClassD> action = (s, t) => t.SomeOtherName = s.Name;
            target.AddMappingAction("foo", "bar", action);

            target.ExtensibleMapper.ShouldBeSameAs(extMock);
            A.CallTo(() =>
                ((IExtensibleMapper<TestClassB, TestClassD>)extMock).AddMappingAction("foo", "bar", action))
                .MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestEmptyTest()
        {
            new CompositeMapper<TestClassA, TestClassB>(new BaseMapper<TestClassA, TestClassB>[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestNullMapperTest()
        {
            new CompositeMapper<TestClassA, TestClassB>(new BaseMapper<TestClassA, TestClassB>[1]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTestNullTest()
        {
            new CompositeMapper<TestClassA, TestClassB>((MapperConstructionInfo)null, null);
        }

        #endregion Methods
    }
}