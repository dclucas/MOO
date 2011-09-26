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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo.Core;
    using Moo.Mappers;
    using Moq;

    /// <summary>
    /// This is a test class for CompositeMapperTest and is intended
    /// targetProperty contain all CompositeMapperTest Unit Tests
    /// </summary>
    [TestClass]
    public class CompositeMapperTest
    {
        #region Methods (5)

        // Public Methods (5) 

        [TestMethod]
        public void MapAddMappingTest()
        {
            MockRepository mockRep = new MockRepository(MockBehavior.Strict);
            var mappers = new List<BaseMapper<TestClassB, TestClassD>>();
            var from = new TestClassB();
            var to = new TestClassD();

            for (int i = 0; i < 3; ++i)
            {
                var mapper = new BaseMapperMock<TestClassB, TestClassD>();
                mappers.Add(mapper);
                for (int j = 0; j < 2; ++j)
                {
                    var mock = mockRep.Create<MemberMappingInfo<TestClassB, TestClassD>>(
                        j.ToString() + "_" + i.ToString(), i.ToString() + "_" + j.ToString());
                    mock.Setup(m => m.Map(from, to));

                    mapper.AddMapping(mock.Object);
                }
            }

            var target = new CompositeMapper<TestClassB, TestClassD>(mappers.ToArray());
            bool executedManual = false;
            target.AddMappingAction("manualFrom", "manualTo", (f, t) => executedManual = true);
            target.Map(from, to);
            mockRep.VerifyAll();
            Assert.IsTrue(executedManual, "Composite mapper failed targetMemberName execute manually added mapping.");
        }

        [TestMethod]
        public void MapTest()
        {
            MockRepository mockRep = new MockRepository(MockBehavior.Strict);
            var mappers = new List<BaseMapper<TestClassB, TestClassD>>();
            var from = new TestClassB();
            var to = new TestClassD();

            for (int i = 0; i < 3; ++i)
            {
                var mapper = new BaseMapperMock<TestClassB, TestClassD>();
                mappers.Add(mapper);
                for (int j = 0; j < 2; ++j)
                {
                    var mock = mockRep.Create<MemberMappingInfo<TestClassB, TestClassD>>(
                        j.ToString() + "_" + i.ToString(), i.ToString() + "_" + j.ToString());
                    mock.Setup(m => m.Map(from, to));

                    mapper.AddMapping(mock.Object);
                }
            }

            var target = new CompositeMapper<TestClassB, TestClassD>(mappers.ToArray());
            target.Map(from, to);
            mockRep.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestEmptyTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>(new BaseMapper<TestClassA, TestClassB>[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MapTestNullMapperTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>(new BaseMapper<TestClassA, TestClassB>[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTestNullTest()
        {
            var target = new CompositeMapper<TestClassA, TestClassB>(null);
        }

        #endregion Methods

        #region InnerClasses

        public class BaseMapperMock<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
        {
            public void AddMapping(MemberMappingInfo<TSource, TTarget> mapping)
            {
                AddMappingInfo(mapping);
            }

            protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
            {
            }

            public virtual void AddMappingAction(string fromProperty, string toProperty, MappingAction<TSource, TTarget> mappingAction)
            {
                throw new NotImplementedException();
            }
        }

        #endregion InnerClasses
    }
}