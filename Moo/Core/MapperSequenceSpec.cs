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
namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>Specifies a mapping sequence to use.</summary>
    internal class MapperSequenceSpec : IMapperSequenceSpec
    {
        /// <summary>Initializes a new instance of the MapperSequenceSpec class.</summary>
        /// <param name="parentSpec">Information describing the parent.</param>
        /// <param name="currentSequence">The current mapper sequence.</param>
        public MapperSequenceSpec(IRepositorySpec parentSpec, IList<Type> currentSequence)
        {
            Guard.CheckArgumentNotNull(parentSpec, "parentSpec");
            Guard.CheckArgumentNotNull(currentSequence, "currentSequence");
            this.ParentSpec = parentSpec;
            this.CurrentSequence = currentSequence;
        }

        /// <summary>Gets or sets the current mapper sequence.</summary>
        /// <value>The current mapper sequence.</value>
        private IList<Type> CurrentSequence { get; set; }

        /// <summary>Gets or sets the parent specification.</summary>
        /// <value>The parent specification.</value>
        private IRepositorySpec ParentSpec { get; set; }

        /// <summary>Gets the next mapper to use.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>An object allowing the fluent specification to continue.</returns>
        public IMapperSequenceSpec Then<TMapper>() where TMapper : IMapper
        {
            AddMapper(typeof(TMapper));
            return new MapperSequenceSpec(ParentSpec, CurrentSequence);
        }

        /// <summary>Adds a mapper to the existing sequence.</summary>
        /// <param name="mapperType">Type of the mapper to add.</param>
        private void AddMapper(Type mapperType)
        {
            CurrentSequence.Add(mapperType);
        }

        /// <summary>Gets the last mapper to use.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>
        /// An object allowing the return to the beginning of the fluent specification (the parent repo
        /// spec).
        /// </returns>
        public IRepositorySpec Finally<TMapper>() where TMapper : IMapper
        {
            AddMapper(typeof(TMapper));
            ParentSpec.MapperOrder.SetSequence(CurrentSequence.ToArray());
            return ParentSpec;
        }
    }
}
