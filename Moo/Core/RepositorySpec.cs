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
// along Moo.  If not, see http:// www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Moo.Core
{
    /// <summary>Repository setup specification.</summary>
    internal class RepositorySpec : IRepositorySpec
    {
        /// <summary>Initializes a new instance of the RepositorySpec class.</summary>
        public RepositorySpec()
        {
            MapperOrder = new MapperStartSpec(this);
        }

        /// <summary>Gets the mapper order.</summary>
        /// <value>The mapper order.</value>
        public IMapperStartSpec MapperOrder { get; private set; }

        /// <summary>Gets the resulting mapping options.</summary>
        /// <returns>The resulting mapping options.</returns>
        public MappingOptions GetOptions()
        {
            var order = (MapperStartSpec) MapperOrder;
            return new MappingOptions(order.MapperSequence.Reverse());
        }
    }
}