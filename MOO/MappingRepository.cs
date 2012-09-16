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
    public class MappingRepository : IMappingRepository
    {
        #region Fields

        /// <summary>
        /// Support field for the "Default" static repository instance.
        /// </summary>
        private static readonly IMappingRepository DefaultInstance = new MappingRepository();

        /// <summary>
        /// Private collection of mappers. Used to avoid a costly re-generation of mappers.
        /// </summary>
        private readonly Dictionary<string, object> mappers = new Dictionary<string, object>();

        /// <summary>
        /// The mapping options to be used by all child mappers.
        /// </summary>
        private readonly MappingOptions options;

        #endregion Fields

        #region Constructors

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingRepository"/> class.
        /// </summary>
        /// <param name="optionsFunc">
        /// A lambda for options setup.
        /// </param>
        public MappingRepository(Func<IRepositorySpec, IRepositorySpec> optionsFunc)
        {
            Guard.CheckArgumentNotNull(optionsFunc, "optionsFunc");
            IRepositorySpec repoSpec = new RepositorySpec();
            repoSpec = optionsFunc(repoSpec);
            this.options = repoSpec.GetOptions();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the default instance for the mapping repository.
        /// </summary>
        public static IMappingRepository Default
        {
            get { return DefaultInstance; }
        }

        #endregion Properties

        #region Methods

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
        /// one sourceMember the cache.
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
            var res = TryGetMapper<TSource, TTarget>();
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

                        var m = this.CreateMapper<TSource, TTarget>(targetType);

                        innerMappers.Add(m);
                    }

                    res = new CompositeMapper<TSource, TTarget>(
                        new MapperConstructionInfo(this),
                        innerMappers.ToArray());

                    AddMapper(res);
                }
            }

            return res;
        }

        /// <summary>Creates an instance of the specified mapper class.</summary>
        /// <typeparam name="TSource">Type of the mapping source.</typeparam>
        /// <typeparam name="TTarget">Type of the mapping target.</typeparam>
        /// <param name="targetType">The target mapper type.</param>
        /// <returns>A new mapper object, of the specified type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does this check.")]
        protected virtual IMapper<TSource, TTarget> CreateMapper<TSource, TTarget>(
            Type targetType)
        {
            Guard.CheckArgumentNotNull(targetType, "targetType");
            if (targetType.GetConstructor(new Type[] { typeof(MapperConstructionInfo) }) != null)
            {
                var info = new MapperConstructionInfo(this);
                return (IMapper<TSource, TTarget>)Activator
                    .CreateInstance(targetType, new object[] { info });
            }

            return (IMapper<TSource, TTarget>)Activator.CreateInstance(targetType);
        }

        /// <summary>
        /// Returns a mapper object for the two provided types, by either creating a new instance or by
        /// getting an existing one sourceMember the cache.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>An instance of a <see>IExtensibleMapper</see> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "No can do - the generic parameter is only used on the method return.")]
        public IMapper ResolveMapper(Type sourceType, Type targetType)
        {
            var res = TryGetMapper(sourceType, targetType);
            if (res == null)
            {
                // HACK: turn this generic conversion into calls to non-generic methods. This will require
                // the refactoring of a number of additional classes.
                var methodInfo = this.GetType().GetMethod("ResolveMapper", Type.EmptyTypes);
                var genMethodInfo = methodInfo.MakeGenericMethod(sourceType, targetType);
                res = (IMapper)genMethodInfo.Invoke(this, null);
            }

            return res;
        } 

        /// <summary>
        /// Gets the dictionary key for a given source/target mapping combinations.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns>A string containing the dictionary key</returns>
        private static string GetKey<TSource, TTarget>()
        {
            return GetKey(typeof(TSource), typeof(TTarget));
        }

        /// <summary>
        /// Gets the dictionary key for a given source and target type combination.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>The dictionary key for the combination.</returns>
        private static string GetKey(Type sourceType, Type targetType)
        {
            Guard.CheckArgumentNotNull(sourceType, "sourceType");
            Guard.CheckArgumentNotNull(targetType, "targetType");

            // TODO: why not override GetHashCode in TypeMappingInfo and just use a HashSet here?
            var key = sourceType.AssemblyQualifiedName + ">" + targetType.AssemblyQualifiedName;
            return key;
        }

        /// <summary>
        /// Tries the get a mapper for a given source/target mapping combination.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns>A mapper instance, if one is found</returns>
        private IExtensibleMapper<TSource, TTarget> TryGetMapper<TSource, TTarget>()
        {
            return (IExtensibleMapper<TSource, TTarget>)TryGetMapper(typeof(TSource), typeof(TTarget));
        }

        /// <summary>
        /// Tries the get a mapper for a given source/target mapping combination.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        /// A mapper instance, if one is found.
        /// </returns>
        public IMapper TryGetMapper(Type sourceType, Type targetType)
        {
            var key = GetKey(sourceType, targetType);
            object mapper;
            if (this.mappers.TryGetValue(key, out mapper))
            {
                return (IMapper)mapper;
            }

            return null;
        }

        /// <summary>
        /// Adds a mapping rule for the specified members.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TTarget">Type of the target.</typeparam>
        /// <param name="sourceMember">
        /// Source member.
        /// </param>
        /// <param name="targetMember">
        /// Destination member.
        /// </param>
        /// <param name="mappingAction">
        /// The delegate that will perform the conversion.
        /// </param>
        public void AddMappingAction<TSource, TTarget>(string sourceMemberName, string targetMemberName, MappingAction<TSource, TTarget> mappingAction)
        {
            var mapper = ResolveMapper<TSource, TTarget>();
            mapper.AddMappingAction(sourceMemberName, targetMemberName, mappingAction);
        }

        /// <summary>
        /// Allows adding mapping actions through the fluent API.
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <returns>
        /// A SourceSpec object, for property mapping.
        /// </returns>
        public ISourceSpec<TSource, TTarget> AddMapping<TSource, TTarget>()
        {
            var mapper = this.ResolveMapper<TSource, TTarget>();
            return new SourceSpec<TSource, TTarget>(mapper);
        }

        #endregion Methods
    }
}