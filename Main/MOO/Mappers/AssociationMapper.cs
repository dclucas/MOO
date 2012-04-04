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


namespace Moo.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Moo.Core;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AssociationMapper<TSource, TTarget> : ConventionMapper<TSource, TTarget> 
    {
        private IMappingRepository mappingRepo;

        public AssociationMapper()
            : this(MappingRepository.Default)
        {
        }

        public AssociationMapper(IMappingRepository mappingRepo)
        {
            this.mappingRepo = mappingRepo;
        }

        public AssociationMapper<TSource, TTarget> Include<T1, T2>()
        {
            mappingRepo.ResolveMapper<T1, T2>();
            return this;
        }

        protected override MemberMappingInfo<TSource, TTarget> CreateInfo(PropertyInfo fromProp, PropertyInfo toProp)
        {
            var mapper = mappingRepo.TryGetMapper(fromProp.PropertyType, toProp.PropertyType);
            if (mapper != null)
            {
                //new DelegateMappingInfo<TSource, TTarget>()
            }

            return base.CreateInfo(toProp, fromProp);
        }
    }
}
