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
    public static class IEnumerableExtender
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
        public static IEnumerable<TTarget> MapAll<TSource, TTarget>(this IEnumerable<TSource> source, MappingRepository repo)
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