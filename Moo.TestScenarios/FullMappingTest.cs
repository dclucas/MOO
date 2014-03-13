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
namespace Moo.TestScenarios
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Moo.Mappers;
    using Moo.TestScenarios.MappedClasses.DomainModels;
    using Moo.TestScenarios.MappedClasses.ViewModels;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture(TypeArgs = new Type[] { typeof(Person), typeof(PersonIndexModel) })]
    [TestFixture(TypeArgs = new Type[] { typeof(Person), typeof(PersonEditModel) })]
    [TestFixture(TypeArgs = new Type[] { typeof(PersonEditModel), typeof(Person) })]
    [TestFixture(TypeArgs = new Type[] { typeof(PersonIndexModel), typeof(Person) })]
    [Category("Integrated")]
    public class FullMappingTest<TSource, TTarget>
        where TSource : new()
    {
        [TestCase(new object[] { null })]
        public void FullMap_MapsCorrectly(Type[] innerMappers)
        {
            var target = this.CreateMapper(innerMappers);
            var sourceObj = this.CreateSourceObject();

            var targetObj = target.Map(sourceObj);

            this.CheckMappings(sourceObj, targetObj, innerMappers);
        }

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
                                mapper = m.Attribute("Type").Value,
                                srcProp = mp.Attribute("SourceProp").Value,
                                trgProp = mp.Attribute("TargetProp").Value,
                            };

            var mappings = new Dictionary<string, string>();
            foreach (var p in props)
            {
                mappings[p.trgProp] = p.srcProp;
            }

            foreach (var p in mappings)
            {
                try
                {
                    Trace.TraceInformation("Comparing properties {0}.{1} and {2}.{3}.", srcType, p.Value, trgType, p.Key);
                    var srcVal = this.GetValue(p.Value, sourceObj);
                    var trgVal = this.GetValue(p.Key, targetObj);
                    Trace.TraceInformation("Values are {0} and {1}", srcVal, trgVal);
                    Assert.AreEqual(srcVal, trgVal);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        string.Format(
                        "Error when formatting from {0}.{1} to {2}.{3}", 
                        typeof(TSource).FullName,
                        p.Value,
                        typeof(TTarget).FullName,
                        p.Key), 
                        e);
                }
            }
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

            var mapper = repo.ResolveMapper<TSource, TTarget>();
            this.AddMappingActions(mapper);
            return mapper;
        }

        protected virtual TSource CreateSourceObject()
        {
            var fixture = new Fixture();
            fixture.Register(() => (IEnumerable<Contact>)fixture.CreateMany<Contact>());
            fixture.Register(() => (IEnumerable<Person>)fixture.CreateMany<Person>());
            fixture.Register(() => (DateTime?)null);
            var result = fixture
                .Build<Manager>()
                .Without(m => m.Managees)
                .Without(m => m.Manager)
                .CreateAnonymous<TSource>();
            return result;
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

        private void AddMappingActions(IExtensibleMapper<TSource, TTarget> mapper)
        {
            var doc = XDocument.Load("MappingExpectations.xml");
            var srcType = typeof(TSource);
            var trgType = typeof(TTarget);
            var props = from m in doc.Descendants("Mapper")
                        where m.Attribute("Type").Value == "ManualMapper"
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
                mapper.AddMappingAction(
                    p.srcProp,
                    p.trgProp,
                    (source, target) =>
                    {
                        var srcVal = typeof(TSource).GetProperty(p.srcProp).GetValue(source, null);
                        typeof(TTarget).GetProperty(p.trgProp).SetValue(target, srcVal, null);
                    });
            }
        }
    }
}
