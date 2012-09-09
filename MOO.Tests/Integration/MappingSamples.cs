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
    using System.Linq;
    using System.Text;
    using Moo.Tests.Integration.MappedClasses.DomainModels;
    using Moo.Tests.Integration.MappedClasses.ViewModels;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Shouldly;
    using Moo.Tests.Integration.MappedClasses.DataContracts;
    using Moo.Mappers;

    [TestFixture]
    public class MappingSamples
    {
        [Test]
        public void Map_SimpleCase_MapsCorrectly()
        {
            var source = this.CreateSource();
            var mapper = MappingRepository.Default.ResolveMapper<Person, PersonEditModel>();

            var result = mapper.Map(source);

            result.ShouldNotBe(null);
            result.Id.ShouldBe(source.Id);
        }

        [Test]
        public void ExtensionMap_SimpleCase_MapsCorrectly()
        {
            var source = this.CreateSource();

            var result = source.MapTo<PersonEditModel>();

            this.CheckMapping(source, result);
        }

        [Test]
        public void ExtensionMapMultiple_SimpleCase_MapsCorrectly()
        {
            var source = this.CreateMany();

            var result = source.MapAll<Person, PersonEditModel>();

            result.ShouldNotBe(null);
        }

        [Test]
        public void ExtensionMap_CustomMappingActions_MapsCorrectly()
        {
            var source = this.CreateSource();
            MappingRepository.Default
                .AddMappingAction<Person, PersonEditModel>(
                "FirstName + LastName", "Name", (s, t) => t.Name = s.FirstName + s.LastName);
            var result = source.MapTo<PersonEditModel>();

            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
        }
       
        [Test]
        public void FluentMapping_AddedViaMapper_MapsCorrectly()
        {
            var mappingRepo = new MappingRepository();
            var source = this.CreateSource();
            var mapper = mappingRepo.ResolveMapper<Person, PersonEditModel>();
            mapper.AddMapping()
                .From(p => p.FirstName + p.LastName)
                .To(pi => pi.Name);
            
            var result = mapper.Map(source);

            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
        }

        [Test]
        public void FluentMapping_AddedViaRepo_MapsCorrectly()
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
            // TODO: this should be made thread-safe for parallel mapping execution.
            MappingRepository.Default.Clear();
            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
            result.Email.ShouldBe(source.Contacts.First().Email);
        }

        [Test]
        public void FluentMapping_WithInnerMappers_MapsCorrectly()
        {
            var source = this.CreateSource();

            MappingRepository.Default
                .AddMapping<Person, PersonDetailsDataContract>()
                .UseMapperFor<Account, AccountDataContract>()
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
        public void MapperSequence_OverridenDefault_CreatesCorrectly()
        {
            ////var source = this.CreateSource();

            ////var repo = new MappingRepository(o =>
            ////    o.MapperOrder
            ////        .Use<ConventionMapper<object, object>>()
            ////        .Then<ManualMapper<object, object>>()
            ////        .Finally<AttributeMapper<object, object>>());
            
            ////repo.AddMapping<Person, PersonEditModel>()
            ////    .From(s => 123)
            ////    .To(t => t.Id);

            ////var mapper = repo.ResolveMapper<Person, PersonEditModel>();

            ////var result = mapper.Map(source);
            ////result.ShouldNotBe(null);
        }

        public void WorkInProgress()
        {
            /*
            var source = this.CreateSource();

            MappingRepository.Default
                .AddMapping<Person, PersonEditModel>()
                // use of mappers for internal properties -- default overload takes no arguments, need to think if additional ones would be required
                .UseMapperFor<Account, AccountEditModel>()
                .From(p => p.FirstName + p.LastName)
                .To(pe => pe.Name)
                // allow chaining of multiple From/To calls
                .From(p => MessyProp)
                .To(pe => pe.SomeOtherProp);

            var repo = new MappingRepository(
                          MappingOptions 
                             .UseFirst<ConfigurationMapper<,>>
                             .Then<ManualMapper<,>>
                             .Then<AttributeMapper<,>>
                             .Then<ConventionMapper<,>>
                              // TODO: find a syntax to go back to mapping options
            // RENAME MANUALMAPPER TO CODEMAPPER?!?
            
            var result = source.MapTo<PersonEditModel>();

            // cleaning up so there are no side effects on other tests
            // TODO: this should be made thread-safe for parallel mapping execution.
            MappingRepository.Default.Clear();
            result.ShouldNotBe(null);
            this.CheckMapping(source, result);
            result.Name.ShouldBe(source.FirstName + source.LastName);
            */
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
    }
}
