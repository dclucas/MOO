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
namespace Moo.Tests
{
    using System;

    using Moo.Core;

    public class Mapper1<TSource, TTarget> : IMapper<TSource, TTarget>
    {
        public TTarget Map(TSource source, TTarget target)
        {
            throw new NotImplementedException();
        }

        public TTarget Map(TSource source)
        {
            throw new NotImplementedException();
        }

        public TTarget Map(TSource source, Func<TTarget> createTarget)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TTarget> MapMultiple(System.Collections.Generic.IEnumerable<TSource> sourceList)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TTarget> MapMultiple(System.Collections.Generic.IEnumerable<TSource> sourceList, Func<TTarget> createTarget)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object target)
        {
            throw new NotImplementedException();
        }

        public object Map(object source)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Func<object> createTarget)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IEnumerable MapMultiple(System.Collections.IEnumerable sourceList)
        {
            throw new NotImplementedException();
        }
    }

    public class Mapper2<TSource, TTarget> : Mapper1<TSource, TTarget>
    {
        public Mapper2(MapperConstructionInfo constructionInfo)
        {
            this.ConstructionInfo = constructionInfo;
        }

        public MapperConstructionInfo ConstructionInfo { get; private set; }
    }
}
