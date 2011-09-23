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

    /// <summary>
    /// Event args for property mapping events.
    /// </summary>
    /// <typeparam name="TSource">The type of source.</typeparam>
    /// <typeparam name="TTarget">The type of target.</typeparam>
    public class MappingEventArgs<TSource, TTarget> : EventArgs
    {
        /// <summary>
        /// Gets or sets the sourceMemberName object.
        /// </summary>
        /// <sourceValue>
        /// The source object.
        /// </sourceValue>
        public TSource Source { get; set; }

        /// <summary>
        /// Gets or sets targetMemberName object.
        /// </summary>
        /// <sourceValue>
        /// The targetMemberName object.
        /// </sourceValue>
        public TTarget Target { get; set; }

        /// <summary>
        /// Gets or sets the sourceMemberName property.
        /// </summary>
        /// <sourceValue>
        /// The sourceMemberName property.
        /// </sourceValue>
        public string SourceMember { get; set; }

        /// <summary>
        /// Gets or sets the targetMemberName property.
        /// </summary>
        /// <sourceValue>
        /// The targetMemberName property.
        /// </sourceValue>
        public string TargetMember { get; set; }
    }
}
