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
using FakeItEasy;
using Moo.Mappers;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Moo.Tests.Mappers
{
    /// <summary>
    ///     This is a test class for ManualMapperTest and is intended
    ///     targetProperty contain all ManualMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ManualMapperTest
    {
        public class FromTestClass
        {
            #region Properties

            public string Description { get; set; }

            public int Id { get; set; }

            public DateTime SampleDate { get; set; }

            #endregion Properties
        }

        public class ToTestClass
        {
            #region Properties

            public int Code { get; set; }

            public string Name { get; set; }

            public string SampleDateInStrFormat { get; set; }

            #endregion Properties
        }

        [Test]
        public void Map_InvalidMappingAction_WrapsAndThrows()
        {
            var sut = new ManualMapper<FromTestClass, ToTestClass>();
            var source = new FromTestClass {Id = 5, Description = "test"};

            var targetObj = new ToTestClass();
            sut.AddMappingAction("Id", "Code", (f, t) => { throw new InvalidOperationException(); });
            Should.Throw<MappingException>(() => sut.Map(source, targetObj));
        }

        private ManualMapper<FromTestClass, ToTestClass> SetupTest(FromTestClass source)
        {
            var sut = new ManualMapper<FromTestClass, ToTestClass>();
            source.Id = 5;
            source.Description = "test";
            source.SampleDate = DateTime.Now;
            sut.AddMappingAction("Id", "Code", (f, t) => t.Code = f.Id);
            sut.AddMappingAction("Description", "Name", (f, t) => t.Name = f.Description);
            sut.AddMappingAction("SampleDate", "SampleDateInStrFormat",
                (f, t) => t.SampleDateInStrFormat = f.SampleDate.ToShortDateString());
            return sut;
        }

        private void CheckMapping(FromTestClass source, ToTestClass target)
        {
            source.Id.ShouldBe(target.Code);
            source.Description.ShouldBe(target.Name);
            source.SampleDate.ToShortDateString().ShouldBe(target.SampleDateInStrFormat);            
        }

        [Test]
        public async Task Map_ManualMappingsWithTarget_MapsCorrectly()
        {
            var source = new FromTestClass();
            var sut = SetupTest(source);
            var target = new ToTestClass();
            await sut.MapAsync(source, target);
            CheckMapping(source, target);
        }

        [Test]
        public async Task Map_ManualMappings_CreatesTargetAndMapsCorrectly()
        {
            var source = new FromTestClass();
            var sut = SetupTest(source);
            var target = await sut.MapAsync(source);
            CheckMapping(source, target);
        }


        [Test]
        public async Task Map_ManualMappingsWithFactory_CreatesTargetAndMapsCorrectly()
        {
            var source = new FromTestClass();
            var sut = SetupTest(source);
            var factoryMethod = A.Fake<Func<ToTestClass>>();
            A.CallTo(() => factoryMethod()).Returns(new ToTestClass());
            var target = await sut.MapAsync(source, factoryMethod);
            A.CallTo(() => factoryMethod()).MustHaveHappened();
            CheckMapping(source, target);
        }
    }
}