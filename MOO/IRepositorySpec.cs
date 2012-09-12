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
    using System.Linq;
    using System.Text;

    /// <summary>Interface for repository behavior specification.</summary>
    public interface IRepositorySpec
    {
        /// <summary>Gets the mapper order.</summary>
        /// <value>The mapper order.</value>
        IMapperStartSpec MapperOrder { get; }
        
        /// <summary>Gets the resulting mapping options.</summary>
        /// <returns>The resulting mapping options.</returns>
        MappingOptions GetOptions();
    }

    /// <summary>Interface for mapper sequence start specification.</summary>
    public interface IMapperStartSpec
    {
        /// <summary>Specifies a mapper to use in the sequence.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>An object allowing to specify the target (To) of the mapping.</returns>
        IMapperSequenceSpec Use<TMapper>() where TMapper : IMapper;

        /// <summary>Specifies that a single mapper should be used in a sequence.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>The parent repo spec. Useful for chaining other "fluent" setup.</returns>
        IRepositorySpec UseJust<TMapper>() where TMapper : IMapper;

        /// <summary>Sets a sequence.</summary>
        /// <param name="mapperTypes">List of types of the mappers.</param>
        /// <returns>Sets the final ordered list of mappers.</returns>
        IRepositorySpec SetSequence(params Type[] mapperTypes);
    }

    /// <summary>
    /// Interface for mapper sequence specification.
    /// </summary>
    public interface IMapperSequenceSpec
    {
        /// <summary>Gets the next mapper to use.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>An object allowing the fluent specification to continue</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Naming", 
            "CA1716:IdentifiersShouldNotMatchKeywords", 
            MessageId = "Then",
            Justification = "Hardly any good reason to override/implement this interface outside this assembly.")]
        IMapperSequenceSpec Then<TMapper>() where TMapper : IMapper;

        /// <summary>Gets the last mapper to use.</summary>
        /// <typeparam name="TMapper">Type of the mapper.</typeparam>
        /// <returns>
        /// An object allowing the return to the beginning of the fluent specification (the parent
        /// repo spec).
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Naming",
            "CA1716:IdentifiersShouldNotMatchKeywords",
            MessageId = "Finally",
            Justification = "Hardly any good reason to override/implement this interface outside this assembly.")]
        IRepositorySpec Finally<TMapper>() where TMapper : IMapper;
    }
}
