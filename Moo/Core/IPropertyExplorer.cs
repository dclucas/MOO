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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Moo.Core
{
    /// <summary>Interface for property explorer.</summary>
    public interface IPropertyExplorer
    {
        /// <summary>Enumerates get matches in this collection.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TTarget">Type of the target.</typeparam>
        /// <param name="checkAction">
        ///     A check function, that receives a source and a target property and determines if they
        ///     match.
        /// </param>
        /// <returns>
        ///     A list of all matches between the source and target types.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "No can do.")]
        IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetMatches<TSource, TTarget>(
            Func<PropertyInfo, PropertyInfo, bool> checkAction);

        /// <summary>Gets all properties for a given source type.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        IEnumerable<PropertyInfo> GetSourceProps<TSource>();

        /// <summary>Gets all properties for a given target type.</summary>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        IEnumerable<PropertyInfo> GetTargetProps<TTarget>();
    }
}