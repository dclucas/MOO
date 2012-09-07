namespace Moo.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;
    using Moo;
    using Moo.Tests.Integration.MappedClasses.ViewModels;
    using Moo.Tests.Integration.MappedClasses.DomainModels;
    using Shouldly;
    using FakeItEasy;

    [TestFixture]
    public class IExtensibleMapperExtenderTest
    {
        [Test]
        public void AddMapping_DefaultCase_AddsToMapper()
        {
            var mapperMock = A.Fake <IExtensibleMapper<Person, PersonIndexModel>>();
            var targetMember = "Name";
            A.CallTo(() => mapperMock.AddMappingAction(
                A<string>.Ignored,
                targetMember,
                A<MappingAction<Person, PersonIndexModel>>.Ignored))
                .Invokes(o =>
                    {
                        var action = (MappingAction<Person, PersonIndexModel>)o.Arguments[2];
                        var s = new Person() { FirstName = "John", LastName = "Doe" };
                        var t = new PersonIndexModel();
                        action(s, t);
                        t.Name.ShouldBe(s.FirstName + s.LastName);
                    });

            mapperMock.AddMapping()
                .From(s => s.FirstName + s.LastName)
                .To(t => t.Name);

            A.CallTo(() => mapperMock.AddMappingAction(
                A<string>.Ignored,
                A<string>.Ignored,
                A<MappingAction<Person, PersonIndexModel>>.Ignored))
                .MustHaveHappened();
        }
    }
}
