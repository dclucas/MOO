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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Moo.Core;

namespace Moo.Mappers
{
    /// <summary>
    ///     Uses naming and type conversion convention to create mappings between
    ///     two classes.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ConventionMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConventionMapper&lt;TSource, TTarget&gt;" /> class.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification = "For the time being, this is the desired behavior.")]
        public ConventionMapper()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConventionMapper{TSource,TTarget}" /> class.
        /// </summary>
        /// <param name="constructionInfo">Mapper construction information.</param>
        public ConventionMapper(MapperConstructionInfo constructionInfo)
            : base(constructionInfo)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Creates a member mapping info object to map the selected properties
        /// </summary>
        /// <param name="sourceProperty">
        ///     The source property
        /// </param>
        /// <param name="targetProperty">
        ///     The target property
        /// </param>
        /// <returns>
        ///     A new member mapping info object.
        /// </returns>
        protected virtual ReflectionPropertyMappingInfo<TSource, TTarget> CreateInfo(PropertyInfo sourceProperty,
            PropertyInfo targetProperty)
        {
            var mappingInfo = new ReflectionPropertyMappingInfo<TSource, TTarget>(sourceProperty, targetProperty, false);
            return mappingInfo;
        }

        /// <summary>Enumerates get mappings in this collection.</summary>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process get mappings in this collection.
        /// </returns>
        protected internal override IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            PropertyConverter checker = GetPropertyConverter();

            return from p in PropertyExplorer.GetMatches<TSource, TTarget>(checker.CanConvert)
                select CreateInfo(p.Key, p.Value);
        }

        #endregion Methods
    }
}