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
    using System.Reflection;

    /// <summary>
    /// Provides property searching features.
    /// </summary>
    public class PropertyExplorer : IPropertyExplorer
    {
        /// <summary>Gets all properties for a given source type.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        public IEnumerable<PropertyInfo> GetSourceProps<TSource>()
        {
            return typeof(TSource).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public);
        }

        /// <summary>Gets all properties for a given target type.</summary>
        /// <typeparam name="TTarget">Type of the source.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        public IEnumerable<PropertyInfo> GetTargetProps<TTarget>()
        {
            return typeof(TTarget).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public);
        }

        /// <summary>Enumerates get matches in this collection.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TTarget">Type of the target.</typeparam>
        /// <param name="checkAction">
        /// A check function, that receives a source and a target property and determines if they match.
        /// </param>
        /// <returns>A list of all matches between the source and target types.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It's either that or using an array.")]
        public IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetMatches<TSource, TTarget>(Func<PropertyInfo, PropertyInfo, bool> checkAction)
        {
            return from s in GetSourceProps<TSource>()
                   from t in GetTargetProps<TTarget>()
                   where checkAction(s, t)
                   select new KeyValuePair<PropertyInfo, PropertyInfo>(s, t);
        }
    }
}
