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

using System;
using System.Collections.Generic;
using Moo.Core;
using Moo.Mappers;

namespace Moo
{
    /// <summary>
    ///     Contains mapping options
    /// </summary>
    public class MappingOptions
    {
        /// <summary>
        ///     Backing field containing the internal mappers, in order.
        /// </summary>
        private IEnumerable<Type> _mapperOrder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingOptions" /> class.
        /// </summary>
        public MappingOptions()
        {
            MapperOrder = new[]
            {
                typeof (ConventionMapper<,>),
                typeof (AttributeMapper<,>),
                typeof (ManualMapper<,>)
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingOptions" /> class.
        /// </summary>
        /// <param name="mapperOrder">The internal mappers, in order.</param>
        public MappingOptions(IEnumerable<Type> mapperOrder)
        {
            MapperOrder = mapperOrder;
        }

        /// <summary>
        ///     Gets the list of internal mappers, in order.
        /// </summary>
        public IEnumerable<Type> MapperOrder
        {
            get { return _mapperOrder; }

            private set
            {
                Guard.TrueForAll(
                    value,
                    "sourceValue",
                    t => typeof (IMapper).IsAssignableFrom(t),
                    "All types must implement the IMapper interface.");
                _mapperOrder = value;
            }
        }
    }
}