//-----------------------------------------------------------------------
// <copyright file="MappingRepository.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo
{
    using System;
    using System.Collections.Generic;
    using Moo.Mappers;

    /// <summary>
    /// Repository for mapper objects.
    /// </summary>
    public class MappingRepository
    {
        /// <summary>
        /// Support field for the "Default" static repository instance.
        /// </summary>
        private static readonly MappingRepository defaultInstance = new MappingRepository();

        /// <summary>
        /// The mapping options to be used by all child mappers.
        /// </summary>
        private MappingOptions options;

        /// <summary>
        /// Private collection of mappers. Used to avoid a costly re-generation of mappers.
        /// </summary>
        private Dictionary<string, object> mappers = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository"/> class.
        /// </summary>
        public MappingRepository()
            : this(new MappingOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository"/> class.
        /// </summary>
        /// <param name="options">The mapping options to be used by this repository's mappers.</param>
        public MappingRepository(MappingOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Gets the default instance for the mapping repository.
        /// </summary>
        public static MappingRepository Default
        {
            get { return MappingRepository.defaultInstance; }
        }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "No can do - the generic parameter is only used on the method return.")]
        public IExtensibleMapper<TSource, TTarget> ResolveMapper<TSource, TTarget>()
        {
            IExtensibleMapper<TSource, TTarget> res = TryGetMapper<TSource, TTarget>();
            if (res == null)
            {
                lock (this.options)
                {
                    List<BaseMapper<TSource, TTarget>> innerMappers = new List<BaseMapper<TSource, TTarget>>();

                    var mapperTypes = this.options.MapperOrder;

                    foreach (var t in mapperTypes)
                    {
                        Type targetType = t;
                        if (targetType.IsGenericType)
                        {
                            targetType = targetType.GetGenericTypeDefinition();
                            targetType = targetType.MakeGenericType(new Type[] { typeof(TSource), typeof(TTarget) });
                        }

                        BaseMapper<TSource, TTarget> m = (BaseMapper<TSource, TTarget>)Activator.CreateInstance(targetType);

                        innerMappers.Add(m);
                    }

                    res = new CompositeMapper<TSource, TTarget>(innerMappers.ToArray());
                    AddMapper(res);
                }
            }

            return res;
        }

        /// <summary>
        /// Clears this instance, removing all mappers within it.
        /// </summary>
        public void Clear()
        {
            this.mappers.Clear();
        }

        /// <summary>
        /// Adds the specified mapper targetType the repository.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper targetType be added.</param>
        public void AddMapper<TSource, TTarget>(IExtensibleMapper<TSource, TTarget> mapper)
        {
            this.mappers[GetKey<TSource, TTarget>()] = mapper;
        }

        /// <summary>
        /// Gets the dictionary key for a given source/target mapping combinations.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns>A string containing the dictionary key</returns>
        private static string GetKey<TSource, TTarget>()
        {
            // TODO: why not override GetHashCode in TypeMappingInfo and just use a HashSet here?
            return typeof(TSource).AssemblyQualifiedName + ">" + typeof(TTarget).AssemblyQualifiedName;
        }

        /// <summary>
        /// Tries the get a mapper for a given source/target mapping combination.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns>A mapper instance, if one is found</returns>
        private IExtensibleMapper<TSource, TTarget> TryGetMapper<TSource, TTarget>()
        {
            string key = GetKey<TSource, TTarget>();
            object mapper;
            if (this.mappers.TryGetValue(key, out mapper))
            {
                return (IExtensibleMapper<TSource, TTarget>)mapper;
            }

            return null;
        }
    }
}
