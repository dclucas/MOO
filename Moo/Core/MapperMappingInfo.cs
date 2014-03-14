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
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Member mapping info for internal mapper usage.
    /// </summary>
    /// <typeparam name="TSource">
    /// Type of the source object.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Type of the target object.
    /// </typeparam>
    /// <remarks>
    /// This class exists for internal usage only. Its usage by client code
    /// is not recommended.
    /// </remarks>
    internal class MapperMappingInfo<TSource, TTarget> : MemberMappingInfo<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperMappingInfo{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="mapper">
        /// The internal mapper.
        /// </param>
        /// <param name="sourceProperty">
        /// The source property.
        /// </param>
        /// <param name="targetProperty">
        /// The target property.
        /// </param>
        public MapperMappingInfo(IMapper mapper, PropertyInfo sourceProperty, PropertyInfo targetProperty)
        {
            this.Mapper = mapper;
            this.SourceProperty = sourceProperty;
            this.TargetProperty = targetProperty;
            this.SourceMemberName = sourceProperty.Name;
            this.TargetMemberName = targetProperty.Name;

            var enumerableType = typeof(IEnumerable);
            this.MapMultiple = enumerableType.IsAssignableFrom(sourceProperty.PropertyType)
                && enumerableType.IsAssignableFrom(targetProperty.PropertyType);
        }

        /// <summary>Gets or sets a value indicating whether the map multiple items instead of just one.</summary>
        /// <value>true if map multiple, false if not.</value>
        public bool MapMultiple { get; set; }

        /// <summary>
        /// Gets or sets the internal Mapper.
        /// </summary>
        private IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets the source property.
        /// </summary>
        private PropertyInfo SourceProperty { get; set; }

        /// <summary>
        /// Gets or sets target property.
        /// </summary>
        private PropertyInfo TargetProperty { get; set; }

        /// <summary>
        /// Maps a given class member from the source to the target object.
        /// </summary>
        /// <param name="source">Mapping sourceMember object</param>
        /// <param name="target">Mapping targetMember object</param>
        public override void Map(TSource source, TTarget target)
        {
            var src = this.SourceProperty.GetValue(source, null);
            object val = null;
            val = this.Mapper.Map(src);
            this.TargetProperty.SetValue(target, val, null);
        }
    }
}
