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

namespace Moo
{
    /// <summary>
    ///     Base interface for mapping repositories.
    /// </summary>
    public interface IMappingRepository
    {
        /// <summary>
        ///     Adds the specified mapper to the repository.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper to be added.</param>
        void AddMapper<TSource, TTarget>(IExtensibleMapper<TSource, TTarget> mapper);

        /// <summary>
        ///     Clears this instance, removing all mappers within it.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Returns a mapper object for the two provided types, by
        ///     either creating a new instance or by getting an existing
        ///     one sourceMember the cache.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The originating type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        ///     The destination type.
        /// </typeparam>
        /// <returns>
        ///     An instance of a <see>IExtensibleMapper</see> object.
        /// </returns>
        IExtensibleMapper<TSource, TTarget> ResolveMapper<TSource, TTarget>();

        /// <summary>
        ///     Returns a mapper object for the two provided types, by
        ///     either creating a new instance or by getting an existing
        ///     one sourceMember the cache.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        ///     An instance of a <see>IExtensibleMapper</see> object.
        /// </returns>
        IMapper ResolveMapper(Type sourceType, Type targetType);

        /// <summary>
        ///     Checks whether the repo already contains a mapper for the specified
        ///     classes and returns one if it does.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        ///     An instance of a <see>IMapper</see> object if any exists in the
        ///     repository, <c>null</c> otherwise.
        /// </returns>
        IMapper TryGetMapper(Type sourceType, Type targetType);

        /// <summary>
        ///     Adds a mapping rule for the specified members.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TTarget">Type of the target.</typeparam>
        /// <param name="sourceMemberName">
        ///     Source member.
        /// </param>
        /// <param name="targetMemberName">
        ///     Destination member.
        /// </param>
        /// <param name="mappingAction">
        ///     The delegate that will perform the conversion.
        /// </param>
        void AddMappingAction<TSource, TTarget>(string sourceMemberName, string targetMemberName,
            MappingAction<TSource, TTarget> mappingAction);

        /// <summary>
        ///     Allows adding mapping actions through the fluent API.
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <returns>
        ///     A SourceSpec object, for property mapping.
        /// </returns>
        ISourceSpec<TSource, TTarget> AddMapping<TSource, TTarget>();
    }
}