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
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    using Moo.Extenders;
    using Moo.Mappers;
    using Moo.TestScenarios.MappedClasses.DomainModels;
    using Moo.TestScenarios.MappedClasses.ViewModels;
    using Moo.TestScenarios.MappedClasses.DataContracts;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Shouldly;
    using Moo.Initialization;

    [TestFixture]
    [Category("Integrated")]
    public class MappingSamples
    {
        [Test]
        public void Sample_SimpleCase_MapsCorrectly()
        {
            var source = this.CreateSource();
            var mapper = MappingRepository.Default.ResolveMapper<Person, PersonEditModel>();

            var result = mapper.Map(source);

            result.ShouldNotBe(null);
            result.Id.ShouldBe(source.Id);
        }

        [Test]
        public void Sample_ExtensionMap_MapsCorrectly()
        {
            var source = this.CreateSource();

            var result = source.MapTo<PersonEditModel>();

            this.CheckMapping(source, result);
        }

        [Test]
        public void Sample_ExtensionMapMultiple_MapsCorrectly()
        {
            var sourceEnumerable = this.CreateMany();

            var result = sourceEnumerable.MapAll<Person, PersonEditModel>();

            result.ShouldNotBe(null);
        }

        [Test]
        public void Sample_CustomMappingActions_MapsCorrectly()
        {
            var source = this.CreateSource();
            MappingRepository.Default
                .AddMappingAction<Person, PersonEditModel>(
                "FirstName + LastName", "Name", (s, t) => t.Name = s.FirstName + s.LastName);
            var result = source.MapTo<PersonEditModel>();

            result.ShouldNotBe(null);
            MappingRepository.Default.Clear();
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
        }
       
        [Test]
        public void Sample_FluentMappingViaMapper_MapsCorrectly()
        {
            var mappingRepo = new MappingRepository();
            var source = this.CreateSource();
            var mapper = mappingRepo.ResolveMapper<Person, PersonEditModel>();
            mapper.AddMapping()
                .From(p => p.FirstName + p.LastName)
                .To(pi => pi.Name);
            
            var result = mapper.Map(source);

            MappingRepository.Default.Clear();
            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
        }

        [Test]
        public void Sample_FluentMappingViaRepo_MapsCorrectly()
        {
            var source = this.CreateSource();
            
            MappingRepository.Default
                .AddMapping<Person, PersonEditModel>()
                .From(p => p.FirstName + p.LastName)
                .To(pe => pe.Name)
                .From(p => p.Contacts.First().Email)
                .To(pe => pe.Email);

            var result = source.MapTo<PersonEditModel>();

            // cleaning up so there are no side effects on other tests
            MappingRepository.Default.Clear();
            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
            result.Email.ShouldBe(source.Contacts.First().Email);
        }

        [Test]
        public void Sample_FluentMappingWithInnerMappers_MapsCorrectly()
        {
            var source = this.CreateSource();
            MappingRepository.Default
                .AddMapping<Person, PersonDetailsDataContract>()
                .UseMapperFrom(p => p.Account)
                .To(pd => pd.Account)
                .From(p => p.FirstName + p.LastName)
                .To(pd => pd.Name);

            var result = source.MapTo<PersonDetailsDataContract>();

            // cleaning up so there are no side effects on other tests
            // TODO: this should be made thread-safe for parallel mapping execution.
            MappingRepository.Default.Clear();
            result.ShouldNotBe(null);
            result.Name.ShouldBe(source.FirstName + source.LastName);
            result.Account.ShouldNotBe(null);
            result.Account.Login.ShouldBe(source.Account.Login);
        }

        [Test]
        public void Sample_MapperSequenceOverride_CreatesCorrectly()
        {
            var source = this.CreateSource();

            var repo = new MappingRepository(o =>
                o.MapperOrder
                    .Use<ConventionMapper<object, object>>()
                    .Then<ManualMapper<object, object>>()
                    .Finally<AttributeMapper<object, object>>());

            repo.AddMapping<Person, PersonEditModel>()
                .From(s => 111)
                .To(t => t.Id);

            var mapper = repo.ResolveMapper<Person, PersonEditModel>();

            var result = mapper.Map(source);

            result.ShouldNotBe(null);
            result.Id.ShouldBe(source.Id);
        }

        public void Sample_ErrorHandling_NoTest()
        {
            var source = this.CreateSource();

            try
            {
                var result = source.MapTo<PersonEditModel>();
            }
            catch (MappingException ohno)
            {
                // Do your exception handling here -- mapping exception will
                // contain sourceEnumerable and target information (their types, 
                // properties being mapped, etc)
                Trace.TraceError(
                    "Got an error when mapping. Source: {0}. Target: {1}. Error: {1}",
                    ohno.SourceType,
                    ohno.TargetType,
                    ohno.Message);
            }
        }

        [Test]
        public void Sample_InnerMapperForEnumerable_UsesInnerMapper()
        {
            var source = this.CreateSource();

            MappingRepository.Default
                .AddMapping<Person, PersonDetailsDataContract>()
                .UseMapperFrom(p => p.Contacts)
                .To(pe => pe.PersonContacts);

            var result = source.MapTo<PersonDetailsDataContract>();

            result.PersonContacts.ShouldNotBe(null);
            var sourceContacts = source.Contacts.ToArray();
            var targetContacts = result.PersonContacts.ToArray();
            targetContacts[0].Email.ShouldBe(sourceContacts[0].Email);
            targetContacts[0].Telephone.ShouldBe(sourceContacts[0].Telephone);
            targetContacts[0].StreetAddress.ShouldBe(sourceContacts[0].StreetAddress);
        }

        [Test]
        public void Sample_WithInitializers_UsesInitializer()
        {
            var source = this.CreateSource();
            var repo = new MappingRepository();
            repo.InitializeMappings();

            SampleInitializer.Repos.ShouldContain(repo);
        }

        private void CheckMapping(Person p, PersonEditModel pe)
        {
            pe.ShouldNotBe(null);
            pe.Id.ShouldBe(p.Id);
        }

        private Person CreateSource()
        {
            return new Person()
            {
                Id = 1234,
                LastName = "Doe",
                FirstName = "John",
                Contacts = new Contact[]
                {
                    new Contact() { Email = "john.doe@email.com" }
                },
                Account = new Account() { Login = "FakeLogin", Password = "FakePassword" }
            };
        }

        private IEnumerable<Person> CreateMany()
        {
            return Enumerable.Range(0, 5).Select(i => this.CreateSource());
        }

        public class SampleInitializer : IMappingInitializer
        {
            public static HashSet<IMappingRepository> Repos = new HashSet<IMappingRepository>();

            public void InitializeMappings(IMappingRepository mappingRepo)
            {
                Repos.Add(mappingRepo);
            }
        } 
    }
}
