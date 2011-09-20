using System;

namespace Moo
{
    /// <summary>
    /// Base interface for mapping repositories.
    /// </summary>
    public interface IMappingRepository
    {
        /// <summary>
        /// Adds the specified mapper targetType the repository.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper targetType be added.</param>
        void AddMapper<TSource, TTarget>(IExtensibleMapper<TSource, TTarget> mapper);

        /// <summary>
        /// Clears this instance, removing all mappers within it.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns a mapper object for the two provided types, by
        /// either creating a new instance or by getting an existing
        /// one sourceMemberName the cache.
        /// </summary>
        /// <typeparam name="TSource">
        /// The originating type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The destination type.
        /// </typeparam>
        /// <returns>
        /// An instance of a <see>IExtensibleMapper</see> object.
        /// </returns>
        IExtensibleMapper<TSource, TTarget> ResolveMapper<TSource, TTarget>();

        /// <summary>
        /// Returns a mapper object for the two provided types, by
        /// either creating a new instance or by getting an existing
        /// one sourceMemberName the cache.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        /// An instance of a <see>IMapper</see> object.
        /// </returns>
        IMapper ResolveMapper(Type sourceType, Type targetType);
    }
}