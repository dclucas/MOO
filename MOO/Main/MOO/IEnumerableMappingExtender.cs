/*-----------------------------------------------------------------------------
Copyright 2010 Diogo Lucas

This file is part of Moo.

Foobar is free software: you can redistribute it and/or modify it under the 
terms of the GNU General Public License as published by the Free Software 
Foundation, either version 3 of the License, or (at your option) any later 
version.

Moo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with 
Moo. If not, see http://www.gnu.org/licenses/.
---------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moo.Core;

namespace Moo
{
    /// <summary>
    /// Extends IEnumerable objects, providing mapping functionalities.
    /// </summary>
    public static class IEnumerableMappingExtender
    {
        /// <summary>
        /// Maps all source elements to an enumerable of target elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="mapper">The mapper object.</param>
        /// <returns>An enumerable of target elements</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard does that.")]
        public static IEnumerable<TTarget> MapAll<TSource, TTarget>(this IEnumerable<TSource> source, IMapper<TSource, TTarget> mapper)
        {
            Guard.CheckArgumentNotNull(mapper, "mapper");
            return mapper.MapMultiple(source);
        }

        /// <summary>
        /// Maps all source elements to an enumerable of target elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="repo">The mapping repository.</param>
        /// <returns>An enumerable of target elements</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard does that.")]
        public static IEnumerable<TTarget> MapAll<TSource, TTarget>(this IEnumerable<TSource> source, IMappingRepository repo)
        {
            Guard.CheckArgumentNotNull(repo, "repo");
            var mapper = repo.ResolveMapper<TSource, TTarget>();
            return MapAll<TSource, TTarget>(source, mapper);
        }

        /// <summary>
        /// Maps all source elements to an enumerable of target elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>An enumerable of target elements</returns>
        /// <remarks>
        /// This overload uses the default repository to resolve the mapper dependency.
        /// </remarks>
        public static IEnumerable<TTarget> MapAll<TSource, TTarget>(this IEnumerable<TSource> source)
        {
            return MapAll<TSource, TTarget>(source, MappingRepository.Default);
        }
    }
}