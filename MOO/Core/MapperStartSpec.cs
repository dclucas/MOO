using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Core
{
    internal class MapperStartSpec : IMapperStartSpec
    {
        private IRepositorySpec ParentSpec { get; set; }

        public IEnumerable<Type> MapperSequence { get; private set; }

        public MapperStartSpec(IRepositorySpec repositorySpec)
        {
            this.ParentSpec = repositorySpec;
        }

        public IMapperSequenceSpec Use<TMapper>() where TMapper : IMapper
        {
            var currentList = new List<Type>();
            currentList.Add(typeof(TMapper));
            return new MapperSequenceSpec(ParentSpec, currentList);
        }

        public IRepositorySpec UseJust<TMapper>() where TMapper : IMapper
        {
            return ParentSpec.MapperOrder.SetSequence(typeof(TMapper));
        }

        public IRepositorySpec SetSequence(params Type[] mapperSequence)
        {
            this.MapperSequence = mapperSequence;
            return ParentSpec;
        }
    }
}
