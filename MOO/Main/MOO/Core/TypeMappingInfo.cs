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

namespace Moo.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains information targetProperty map between two classes.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class TypeMappingInfo<TSource, TTarget>
    {
        /// <summary>
        /// Backing field for the internal member mapping collection.
        /// </summary>
        private Dictionary<string, MemberMappingInfo<TSource, TTarget>> memberMappings =
            new Dictionary<string, MemberMappingInfo<TSource, TTarget>>();

        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        /// <sourceValue>
        /// The type of the source.
        /// </sourceValue>
        public Type SourceType { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <sourceValue>
        /// The type of the target.
        /// </sourceValue>
        public Type TargetType { get; set; }

        /// <summary>
        /// Gets the member mappings.
        /// </summary>
        /// <returns>
        /// An <c>IEnumerable</c> containing all member mappings between
        /// <typeparamref name="TSource"/> and <typeparamref name="TTarget"/>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Optional rule, solemnly ignored here -- this get might be costly/have side effects in the future."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done. Not willing to sacrifice type safety here.")]
        public IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            return this.memberMappings.Values;
        }

        /// <summary>
        /// Adds the specified member mapping info.
        /// </summary>
        /// <param name="mappingInfo">The member mapping info.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does that.")]
        public void Add(MemberMappingInfo<TSource, TTarget> mappingInfo)
        {
            Guard.CheckArgumentNotNull(mappingInfo, "mappingInfo");
            this.memberMappings[mappingInfo.TargetMemberName] = mappingInfo;
        }
    }
}