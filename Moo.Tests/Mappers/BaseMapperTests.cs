using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moo.Core;
using Moo.Mappers;
using NUnit.Framework;
using Shouldly;
using System.Collections;

namespace Moo.Tests.Mappers
{
    [TestFixture]
    public class BaseMapperTests
    {
        private class ThrowerMapper : BaseMapper<TestClassC, TestClassB>
        {
            protected internal override IEnumerable<MemberMappingInfo<TestClassC, TestClassB>> GetMappings()
            {
                throw new ApplicationException();
            }
        }

        private class FakeMapper : BaseMapper<TestClassC, TestClassB>
        {
            protected internal override IEnumerable<MemberMappingInfo<TestClassC, TestClassB>> GetMappings()
            {
                var i = 1;
                return new MemberMappingInfo<TestClassC, TestClassB>[]
                {
                    new DelegateMappingInfo<TestClassC, TestClassB>("source", "target", (source, target) =>
                    {
                        target.InnerClassCode = i++;
                    } ),
                };
            }

            public void ClearTypeMapping()
            {
                TypeMapping = null;
            }
        }

        [Test]
        public void Map_InnerException_WrapsAndThrows()
        {
            var sut = new ThrowerMapper();
            var exc = Should.Throw<MappingException>(() => sut.Map(new TestClassC()));
            exc.InnerException.ShouldBeOfType<ApplicationException>();
        }

        [Test]
        public void Map_TypeMappingNull_LazyLoadsAndMaps()
        {
            var sut = new FakeMapper();
            sut.ClearTypeMapping();
            var res = sut.Map(new TestClassC());
            sut.TypeMapping.ShouldNotBe(null);
            res.InnerClassCode.ShouldBe(1);
        }

        [Test]
        public void Map_NonGeneric_MapsAll()
        {
            var sut = new FakeMapper();
            // ReSharper disable once SuspiciousTypeConversion.Global
            var source = (IEnumerable)new TestClassC[5];
            var result = sut.MapMultiple(source);
            sut.TypeMapping.ShouldNotBe(null);
            sut.ShouldNotBeAssignableTo<IEnumerable<TestClassB>>();
            result
                .Cast<TestClassB>()
                .Select(x => x.InnerClassCode)
                .ShouldBe(new int[] { 1, 2, 3, 4, 5 });
        }
    }
}
