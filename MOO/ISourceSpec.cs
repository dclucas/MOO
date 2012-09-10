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

namespace Moo
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Base interface for fluent mapping of source objects.
    /// </summary>
    /// <typeparam name="TSource">Type of the source object.</typeparam>
    /// <typeparam name="TTarget">Type of the target object.</typeparam>
    public interface ISourceSpec<TSource, TTarget>
    {
        /// <summary>
        /// Adds a mapping source
        /// </summary>
        /// <param name="argument">Expression to fetch data from the source object.</param>
        /// <returns>
        /// A ITargetSpec, allowing to define the mapping target.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Easier said than done")]
        ITargetSpec<TSource, TTarget, TInnerSource> From<TInnerSource>(Expression<Func<TSource, TInnerSource>> argument);

        /// <summary>
        /// Instructs Moo to use an internal mapper for properties of <paramref name="TInnerTarget"/> type.
        /// </summary>
        /// <typeparam name="TInnerSource">
        /// Type of the source property to map.
        /// </typeparam>
        /// <typeparam name="TInnerTarget">
        /// Type of the target property to map.
        /// </typeparam>
        /// <returns>
        /// A ISourceSpec, allowing to define further mappings.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Easier said than done")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Easier said than done")]
        ISourceSpec<TSource, TTarget> UseMapperFor<TInnerSource, TInnerTarget>();
    }
}
