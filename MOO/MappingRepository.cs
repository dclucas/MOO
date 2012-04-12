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
    using System.Collections.Generic;
    using System.Text;

    using Moo.Core;
    using Moo.Mappers;

    /// <summary>
    /// Repository for mapper objects.
    /// </summary>
    public class MappingRepository : Moo.IMappingRepository
    {
        #region Fields (3)

        /// <summary>
        /// Support field for the "Default" static repository instance.
        /// </summary>
        private static readonly MappingRepository defaultInstance = new MappingRepository();

        /// <summary>
        /// Private collection of mappers. Used to avoid a costly re-generation of mappers.
        /// </summary>
        private readonly Dictionary<string, object> mappers = new Dictionary<string, object>();

        /// <summary>
        /// The mapping options to be used by all child mappers.
        /// </summary>
        private readonly MappingOptions options;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository"/> class.
        /// </summary>
        /// <param name="options">The mapping options to be used by this repository's mappers.</param>
        public MappingRepository(MappingOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository"/> class.
        /// </summary>
        public MappingRepository()
            : this(new MappingOptions())
        {
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the default instance for the mapping repository.
        /// </summary>
        public static MappingRepository Default
        {
            get { return defaultInstance; }
        }

        #endregion Properties

        #region Methods (8)

        // Public Methods (4) 

        /// <summary>
        /// Adds the specified mapper to the repository.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper to be added.</param>
        public void AddMapper<TSource, TTarget>(IExtensibleMapper<TSource, TTarget> mapper)
        {
            this.mappers[GetKey<TSource, TTarget>()] = mapper;
        }

        /// <summary>
        /// Clears this instance, removing all mappers within it.
        /// </summary>
        public void Clear()
        {
            this.mappers.Clear();
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
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>
        /// An instance of a <see>IExtensibleMapper</see> object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "No can do - the generic parameter is only used on the method return.")]
        public IExtensibleMapper<TSource, TTarget> ResolveMapper<TSource, TTarget>(params MapperInclusion[] mapperInclusions)
        {
            var res = TryGetMapper<TSource, TTarget>(mapperInclusions);
            if (res == null)
            {
                lock (this.options)
                {
                    var innerMappers = new List<IMapper<TSource, TTarget>>();

                    var mapperTypes = this.options.MapperOrder;

                    foreach (var t in mapperTypes)
                    {
                        var targetType = t;
                        if (targetType.IsGenericType)
                        {
                            targetType = targetType.GetGenericTypeDefinition();
                            targetType = targetType.MakeGenericType(new Type[] { typeof(TSource), typeof(TTarget) });
                        }

                        var m = this.CreateMapper<TSource, TTarget>(targetType, mapperInclusions);

                        innerMappers.Add(m);
                    }

                    res = new CompositeMapper<TSource, TTarget>(innerMappers.ToArray());
                    AddMapper(res);
                }
            }

            return res;
        }

        /// <summary>
        /// Creates an instance of the specified mapper class
        /// </summary>
        /// <param name="targetType">
        /// The target mapper type.
        /// </param>
        /// <param name="includedMappers">
        /// The included mappers.
        /// </param>
        /// <typeparam name="TSource">
        /// Type of the mapping source.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// Type of the mapping target.
        /// </typeparam>
        /// <returns>
        /// A new mapper object, of the specified type.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does this check.")]
        protected virtual IMapper<TSource, TTarget> CreateMapper<TSource, TTarget>(
            Type targetType, IEnumerable<MapperInclusion> includedMappers)
        {
            Guard.CheckArgumentNotNull(targetType, "targetType");
            if (targetType.GetConstructor(new Type[] { typeof(MapperConstructorInfo) }) != null)
            {
                var info = new MapperConstructorInfo(this, includedMappers);
                return (IMapper<TSource, TTarget>)Activator
                    .CreateInstance(targetType, new object[] { info });
            }

            return (IMapper<TSource, TTarget>)Activator.CreateInstance(targetType);
        }

        /// <summary>
        /// Returns a mapper object for the two provided types, by
        /// either creating a new instance or by getting an existing
        /// one sourceMemberName the cache.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>
        /// An instance of a <see>IExtensibleMapper</see> object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "No can do - the generic parameter is only used on the method return.")]
        public IMapper ResolveMapper(Type sourceType, Type targetType, params MapperInclusion[] mapperInclusions)
        {
            var res = TryGetMapper(sourceType, targetType, mapperInclusions);
            if (res == null)
            {
                // HACK: turn this generic conversion into calls to non-generic methods. This will require
                // the refactoring of a number of additional classes.
                var methodInfo = this.GetType().GetMethod("ResolveMapper", new Type[] { typeof(MapperInclusion[]) });
                var genMethodInfo = methodInfo.MakeGenericMethod(sourceType, targetType);
                res = (IMapper)genMethodInfo.Invoke(this, new object[] { mapperInclusions });
            }

            return res;
        }

        // Private Methods (4) 

        /// <summary>
        /// Gets the dictionary key for a given source/target mapping combinations.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>A string containing the dictionary key</returns>
        private static string GetKey<TSource, TTarget>(params MapperInclusion[] mapperInclusions)
        {
            return GetKey(typeof(TSource), typeof(TTarget), mapperInclusions);
        }

        /// <summary>
        /// Gets the dictionary key for a given source and target type combination.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>The dictionary key for the combination.</returns>
        private static string GetKey(Type sourceType, Type targetType, params MapperInclusion[] mapperInclusions)
        {
            Guard.CheckArgumentNotNull(sourceType, "sourceType");
            Guard.CheckArgumentNotNull(targetType, "targetType");

            // TODO: why not override GetHashCode in TypeMappingInfo and just use a HashSet here?
            var key = sourceType.AssemblyQualifiedName + ">" + targetType.AssemblyQualifiedName + AppendInclusions(mapperInclusions);
            return key;
        }

        private static string AppendInclusions(MapperInclusion[] mapperInclusions)
        {
            if (mapperInclusions.Length > 0)
            {
                var sb = new StringBuilder();
                foreach (var m in mapperInclusions)
                {
                    sb.AppendFormat("+{0}>{1}", m.SourceType.Name, m.TargetType.Name);
                }

                return sb.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Tries the get a mapper for a given source/target mapping combination.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>A mapper instance, if one is found</returns>
        private IExtensibleMapper<TSource, TTarget> TryGetMapper<TSource, TTarget>(params MapperInclusion[] mapperInclusions)
        {
            return (IExtensibleMapper<TSource, TTarget>)TryGetMapper(typeof(TSource), typeof(TTarget), mapperInclusions);
        }

        /// <summary>
        /// Tries the get a mapper for a given source/target mapping combination.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="mapperInclusions">A list of additional, internal mappers to include.</param>
        /// <returns>
        /// A mapper instance, if one is found.
        /// </returns>
        public IMapper TryGetMapper(Type sourceType, Type targetType, params MapperInclusion[] mapperInclusions)
        {
            var key = GetKey(sourceType, targetType, mapperInclusions);
            object mapper;
            if (this.mappers.TryGetValue(key, out mapper))
            {
                return (IMapper)mapper;
            }

            return null;
        }

        #endregion Methods
    }
}