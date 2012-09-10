/*
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
    using NUnit.Framework;

    using Moo.Core;
    using Moo.Mappers;
    using Moq;

    /// <summary>
    /// Summary description for AssociationMapperTest
    /// </summary>
    [TestFixture]
    public class AssociationMapperTest
    {
        [Test]
        public void Map_UsesIncludedMasters()
        {
            var inclusions = new MapperInclusion[] { new MapperInclusion<TestClassC, TestClassB>() };
            var mockRepo = new MockRepository(MockBehavior.Strict);
            var repo = mockRepo.Create<IMappingRepository>();
            var mapper = mockRepo.Create<IMapper>();
            repo.Setup(r => r.ResolveMapper(typeof(TestClassC), typeof(TestClassB))).Returns(mapper.Object);
            var sourceObj = new TestClassA() { Code = 325, InnerClass = new TestClassC(), Name = "foo" };
            var mapperReturn = new TestClassB();
            mapper.Setup(m => m.Map(sourceObj.InnerClass)).Returns(mapperReturn);
            var target = new AssociationMapper<TestClassA, TestClassF>(
                new MapperConstructionInfo(repo.Object, inclusions));

            var result = target.Map(sourceObj);

            Assert.IsNotNull(result);
            Assert.AreEqual(mapperReturn, result.InnerClass);
        }
    }
}
*/