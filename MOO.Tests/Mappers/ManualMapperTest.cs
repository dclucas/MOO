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
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;
    using Moo.Mappers;
    using Shouldly;

    /// <summary>
    /// This is a test class for ManualMapperTest and is intended
    /// targetProperty contain all ManualMapperTest Unit Tests
    /// </summary>
    [TestFixture]
    public class ManualMapperTest
    {
        #region Methods (4)

        [ExpectedException(typeof(MappingException))]
        [Test]
        public void ManualMapperMapErrorTest()
        {
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            var source = new FromTestClass() { Id = 5, Description = "test" };

            var targetObj = new ToTestClass();
            target.AddMappingAction("Id", "Code", (f, t) => { throw new InvalidOperationException(); });
            target.Map(source, targetObj);
        }

        [Test]
        public void ManualMapperMapTest()
        {
            var target = new ManualMapper<FromTestClass, ToTestClass>();
            FromTestClass source = new FromTestClass() { Id = 5, Description = "test" };
            var targetObj = new ToTestClass();
            target.AddMappingAction("Id", "Code", (f, t) => t.Code = f.Id);
            target.AddMappingAction("Description", "Name", (f, t) => t.Name = f.Description);
            target.AddMappingAction("SampleDate", "SampleDateInStrFormat", (f, t) => t.SampleDateInStrFormat = f.SampleDate.ToShortDateString());
            target.Map(source, targetObj);
            Assert.AreEqual(source.Id, targetObj.Code);
            Assert.AreEqual(source.Description, targetObj.Name);
            Assert.AreEqual(source.SampleDate.ToShortDateString(), targetObj.SampleDateInStrFormat);
        }

        #endregion Methods

        #region Nested Classes (2)

        public class FromTestClass
        {
            #region Properties (3)

            public string Description { get; set; }

            public int Id { get; set; }

            public DateTime SampleDate { get; set; }

            #endregion Properties
        }

        public class ToTestClass
        {
            #region Properties (3)

            public int Code { get; set; }

            public string Name { get; set; }

            public string SampleDateInStrFormat { get; set; }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}