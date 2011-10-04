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
    /// Basic information on how to map from one class member to another.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    internal class DelegateMappingInfo<TSource, TTarget> : MemberMappingInfo<TSource, TTarget>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="mappingAction">The mapping action.</param>
        public DelegateMappingInfo(
            string source,
            string target,
            MappingAction<TSource, TTarget> mappingAction)
            : base(source, target)
        {
            Guard.CheckArgumentNotNull(mappingAction, "mappingAction");
            this.MappingAction = mappingAction;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the mapping action.
        /// </summary>
        public MappingAction<TSource, TTarget> MappingAction { get; private set; }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// Maps from the specified source to the specified target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public override void Map(TSource source, TTarget target)
        {
            this.MappingAction(source, target);
        }

        #endregion Methods
    }
}