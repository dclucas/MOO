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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Moo.Core
{
    /// <summary>
    ///     Specs class for fluent mapping of source objects.
    /// </summary>
    /// <typeparam name="TSource">Type of the source object.</typeparam>
    /// <typeparam name="TTarget">Type of the target object.</typeparam>
    public class SourceSpec<TSource, TTarget> : ISourceSpec<TSource, TTarget>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SourceSpec{TSource,TTarget}" /> class.
        /// </summary>
        /// <param name="mapper">Mapper to extend</param>
        public SourceSpec(IExtensibleMapper<TSource, TTarget> mapper)
        {
            Guard.CheckArgumentNotNull(mapper, "mapper");
            Mapper = mapper;
        }

        /// <summary>
        ///     Gets the mapper that to be extended.
        /// </summary>
        protected IExtensibleMapper<TSource, TTarget> Mapper { get; private set; }

        /// <summary>Adds a mapping source.</summary>
        /// <typeparam name="TInnerSource">Type of the inner source property.</typeparam>
        /// <param name="argument">Expression to fetch data from the source object.</param>
        /// <returns>A ITargetSpec, allowing to define the mapping target.</returns>
        public ITargetSpec<TSource, TTarget> From<TInnerSource>(
            Expression<Func<TSource, TInnerSource>> argument)
        {
            return new TargetSpec<TSource, TTarget, TInnerSource>(Mapper, argument);
        }

        /// <summary>
        ///     Instructs Moo to use an internal mapper for properties of <typeparamref name="TInnerSource" />
        ///     type.
        /// </summary>
        /// <typeparam name="TInnerSource">Type of the source property to map.</typeparam>
        /// <param name="argument">Expression to fetch data from the source object.</param>
        /// <returns>A ISourceSpec, allowing to define further mappings.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "Easier said than done")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done")]
        public ITargetSpec<TSource, TTarget> UseMapperFrom<TInnerSource>(
            Expression<Func<TSource, TInnerSource>> argument)
        {
            return new TargetSpec<TSource, TTarget, TInnerSource>(Mapper, argument, true);
        }
    }
}