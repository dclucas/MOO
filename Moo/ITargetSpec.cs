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

namespace Moo
{
    /// <summary>
    ///     Base interface for fluently setting mapping targets
    /// </summary>
    /// <typeparam name="TSource">Type of the mapping source.</typeparam>
    /// <typeparam name="TTarget">Type of the mapping target.</typeparam>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1005:AvoidExcessiveParametersOnGenericTypes",
        Justification =
            "I wish I could. No big deal, though, as type inference makes the specifications not necessary in client code."
        )]
    public interface ITargetSpec<TSource, TTarget>
    {
        /// <summary>Instructs Moo to map a source expression to the property expression below.</summary>
        /// <remarks>
        ///     The argument parameter must be a property access expression, such as <c>(t) =&gt; t.Name</c>,
        ///     or else an ArgumentException will be thrown.
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     The provided lambda is not of a property access.
        /// </exception>
        /// <typeparam name="TInnerTarget">Type of the target's inner property.</typeparam>
        /// <param name="argument">An expression fetching the property to map to.</param>
        /// <returns>A spec object, allowing fluent setup to continue.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "To",
            Justification = "It's highly improbable that this specific interface will have to be implemented elsewhere."
            )]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done")]
        ISourceSpec<TSource, TTarget> To<TInnerTarget>(Expression<Func<TTarget, TInnerTarget>> argument);
    }
}