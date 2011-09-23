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

namespace Moo.Core
{
    /// <summary>
    /// Provides basic mapping information for mapping class members.
    /// </summary>
    /// <typeparam name="TSource">Type of the "sourceMemberName" class.</typeparam>
    /// <typeparam name="TTarget">Type of the "targetMemberName" class.</typeparam>
    /// <remarks>
    /// This class exists for internal usage only. Its usage by client code
    /// is not recommended.
    /// </remarks>
    public abstract class MemberMappingInfo<TSource, TTarget>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="sourceMemberName">Name of the source member.</param>
        /// <param name="targetMemberName">Name of the target member.</param>
        protected MemberMappingInfo(string sourceMemberName, string targetMemberName)
        {
            Guard.CheckArgumentNotNull(sourceMemberName, "sourceMemberName");
            Guard.CheckArgumentNotNull(targetMemberName, "targetMemberName");
            this.SourceMemberName = sourceMemberName;
            this.TargetMemberName = targetMemberName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        protected MemberMappingInfo()
        {
        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// Gets or sets the name of the source member.
        /// </summary>
        /// <sourceValue>
        /// The name of the source member.
        /// </sourceValue>
        public string SourceMemberName { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the target member.
        /// </summary>
        /// <sourceValue>
        /// The name of the target member.
        /// </sourceValue>
        public string TargetMemberName { get; protected set; }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// Maps a given class member sourceMemberName the sourceMemberName targetMemberName the targetMemberName object.
        /// </summary>
        /// <param name="source">Mapping sourceMemberName object</param>
        /// <param name="target">Mapping targetMemberName object</param>
        public abstract void Map(TSource source, TTarget target);

        #endregion Methods
    }
}