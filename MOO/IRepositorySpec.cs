using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo
{
    public interface IRepositorySpec
    {
        MappingOptions GetOptions();

        IMapperStartSpec MapperOrder { get; }
    }

    public interface IMapperStartSpec
    {
        IMapperSequenceSpec Use<TMapper>() where TMapper : IMapper;

        IRepositorySpec UseJust<TMapper>() where TMapper : IMapper;

        IRepositorySpec SetSequence(params Type[] mapperTypes);
    }

    public interface IMapperSequenceSpec
    {
        IMapperSequenceSpec Then<TMapper>() where TMapper : IMapper;

        IRepositorySpec Finally<TMapper>() where TMapper : IMapper;
    }
}
