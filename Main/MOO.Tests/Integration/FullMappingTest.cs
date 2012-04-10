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

namespace Moo.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Moo.Mappers;
    using Moo.Tests.Integration.MappedClasses.DomainModels;
    using Moo.Tests.Integration.MappedClasses.ViewModels;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    [TestFixture(typeof(Person), typeof(PersonEditModel))]
    [TestFixture(typeof(Person), typeof(PersonIndexModel))]
    [TestFixture(typeof(PersonEditModel), typeof(Person))]
    [TestFixture(typeof(PersonIndexModel), typeof(Person))]
    public class FullMappingTest<TSource, TTarget>
        where TSource : new()
    {
        protected void CheckMappings(TSource sourceObj, TTarget targetObj, Type[] innerMappers)
        {
            var doc = XDocument.Load("MappingExpectations.xml");
            var srcType = typeof(TSource);
            var trgType = typeof(TTarget);
            var props = from m in doc.Descendants("Mapper")
                        from p in m.Descendants("Pair")
                        where srcType.Name.Equals(p.Attribute("SourceType").Value)
                        where trgType.Name.Equals(p.Attribute("TargetType").Value)
                        from mp in p.Descendants("Property")
                        select new
                            {
                                srcProp = mp.Attribute("SourceProp").Value,
                                trgProp = mp.Attribute("TargetProp").Value,
                            };

            foreach (var p in props)
            {
                Debug.WriteLine("Comparing properties {0}.{1} and {2}.{3}", srcType, p.srcProp, trgType, p.trgProp);
                var srcVal = GetValue(p.srcProp, sourceObj);
                var trgVal = GetValue(p.trgProp, targetObj);
                Debug.WriteLine("Values are {0} and {1}", srcVal, trgVal);
                Assert.AreEqual(srcVal, trgVal);
            }
        }

        private object GetValue(string propName, object obj)
        {
            var fp = propName.IndexOf('.');
            if (fp >= 0)
            {
                var part = propName.Substring(0, fp);
                var inner = this.GetValue(part, obj);
                return this.GetValue(propName.Substring(fp + 1), inner);
            }

            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        protected virtual IMapper<TSource, TTarget> CreateMapper(Type[] innerMappers)
        {
            IMappingRepository repo = null;
            if (innerMappers != null)
            {
                var options = new MappingOptions(innerMappers);
                repo = new MappingRepository(options);
            }
            else
            {
                repo = new MappingRepository();
            }

            return repo.ResolveMapper<TSource, TTarget>();
        }
        
        protected virtual TSource CreateSourceObject()
        {
            var fixture = new Fixture();
            fixture.Register(() => (IEnumerable<Contact>)fixture.CreateMany<Contact>());
            fixture.Register(() => (IEnumerable<Person>)fixture.CreateMany<Person>());
            //fixture.Build<Manager>().Without(m => m.Managees);
            //fixture.Build<Manager>().Without(m => m.Manager);
            var result = fixture
                .Build<Manager>()
                .Without(m => m.Managees)
                .Without(m => m.Manager)
                .CreateAnonymous<TSource>();
            return result;
        }

        [TestCase(new object[] { null })]
        ////[TestCase(new object[] { new Type[] { typeof(ConventionMapper<,>) } })]
        public void FullMap_MapsCorrectly(Type[] innerMappers)
        {
            var target = CreateMapper(innerMappers);
            var sourceObj = CreateSourceObject();

            var targetObj = target.Map(sourceObj);

            CheckMappings(sourceObj, targetObj, innerMappers);
        }
    }
}
