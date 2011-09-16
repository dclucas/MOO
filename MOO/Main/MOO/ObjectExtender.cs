using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo
{
    public static class ObjectExtender
    {
        public static TTarget MapTo<TSource, TTarget>(this Object source, IMapper<TSource, TTarget> mapper)
        {
            return (TTarget)mapper.Map(source);
        }
    }
}