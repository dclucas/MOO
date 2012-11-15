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

    /// <summary>Mapper sequence start specification.</summary>
    internal class MapperStartSpec : IMapperStartSpec
    {
        /// <summary>Initializes a new instance of the MapperStartSpec class.</summary>
        /// <param name="repositorySpec">Information describing the repository.</param>
        public MapperStartSpec(IRepositorySpec repositorySpec)
        {
            this.ParentSpec = repositorySpec;
        }

        /// <summary>Gets or sets the parent specs.</summary>
        /// <value>The parent specs.</value>
        private IRepositorySpec ParentSpec { get; set; }

        /// <summary>Gets the mapper sequence.</summary>
        /// <value>The mapper sequence.</value>
        public IEnumerable<Type> MapperSequence { get; private set; }

        /// <summary>Specifies a mapper to use in the sequence.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>An object allowing to specify the target (To) of the mapping.</returns>
        public IMapperSequenceSpec Use<TMapper>() where TMapper : IMapper
        {
            var currentList = new List<Type>();
            currentList.Add(typeof(TMapper));
            return new MapperSequenceSpec(ParentSpec, currentList);
        }

        /// <summary>Specifies that a single mapper should be used in a sequence.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>The parent repo spec. Useful for chaining other "fluent" setup.</returns>
        public IRepositorySpec UseJust<TMapper>() where TMapper : IMapper
        {
            return ParentSpec.MapperOrder.SetSequence(typeof(TMapper));
        }

        /// <summary>Sets a mapper sequence.</summary>
        /// <param name="mapperSequence">List of types of the mappers.</param>
        /// <returns>Sets the final ordered list of mappers.</returns>
        public IRepositorySpec SetSequence(params Type[] mapperSequence)
        {
            this.MapperSequence = mapperSequence;
            return ParentSpec;
        }
    }
}
