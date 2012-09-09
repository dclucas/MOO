using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Core
{
    internal class MapperSequenceSpec : IMapperSequenceSpec
    {
        private IList<Type> CurrentSequence { get; set; }

        private IRepositorySpec ParentSpec { get; set; }

        public MapperSequenceSpec(IRepositorySpec parentSpec, IList<Type> currentSequence)
        {
            Guard.CheckArgumentNotNull(parentSpec, "parentSpec");
            Guard.CheckArgumentNotNull(currentSequence, "currentSequence");
            this.ParentSpec = parentSpec;
            this.CurrentSequence = currentSequence;
        }

        public IMapperSequenceSpec Then<TMapper>() where TMapper : IMapper
        {
            AddMapper(typeof(TMapper));
            return new MapperSequenceSpec(ParentSpec, CurrentSequence);
        }

        private void AddMapper(Type mapperType)
        {
            var finalType = GetFinalType(mapperType);
            CurrentSequence.Add(mapperType);
        }

        internal Type GetFinalType(Type mapperType)
        {
            var finalType = mapperType;
            if (finalType.IsGenericType && !finalType.IsGenericTypeDefinition)
            {
                finalType = finalType.GetGenericTypeDefinition();
            }

            return finalType;
        }

        public IRepositorySpec Finally<TMapper>() where TMapper : IMapper
        {
            AddMapper(typeof(TMapper));
            ParentSpec.MapperOrder.SetSequence(CurrentSequence.ToArray());
            return ParentSpec;
        }
    }
}
