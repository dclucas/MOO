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
namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
