using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Core
{
    internal class RepositorySpec : IRepositorySpec
    {
        public RepositorySpec()
        {
            MapperOrder = new MapperStartSpec(this);
        }

        public IMapperStartSpec MapperOrder { get; private set; }

        public MappingOptions GetOptions()
        {
            var order = (MapperStartSpec)MapperOrder;
            return new MappingOptions(order.MapperSequence);
        }
    }
}
