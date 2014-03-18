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
using System.Diagnostics;
using System.Globalization;

namespace Moo
{
    /// <summary>
    ///     Represents a Moo mapping exception.
    /// </summary>
    [DebuggerNonUserCode]
    public class MappingException : Exception
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <param name="targetMember">The target member.</param>
        /// <param name="innerException">The inner exception.</param>
        public MappingException(
            Type sourceType,
            Type targetType,
            string sourceMember,
            string targetMember,
            Exception innerException)
            : this(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Error mapping from {0}.{1} to {2}.{3}",
                    sourceType,
                    sourceMember,
                    targetType,
                    targetMember),
                sourceType,
                targetType,
                sourceMember,
                targetMember,
                innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <param name="targetMember">The target member.</param>
        /// <param name="innerException">The inner exception.</param>
        public MappingException(
            string message,
            Type sourceType,
            Type targetType,
            string sourceMember,
            string targetMember,
            Exception innerException)
            : this(message, innerException)
        {
            SourceType = sourceType;
            TargetType = targetType;
            SourceMember = sourceMember;
            TargetMember = targetMember;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <param name="targetMember">The target member.</param>
        public MappingException(
            Type sourceType,
            Type targetType,
            string sourceMember,
            string targetMember)
            : this(
                sourceType,
                targetType,
                sourceMember,
                targetMember,
                null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner exception.</param>
        public MappingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MappingException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        public MappingException()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the type of the source object.</summary>
        /// <value>The type of the source.</value>
        public Type SourceType { get; private set; }

        /// <summary>Gets the type of the target.</summary>
        /// <value>The type of the target.</value>
        public Type TargetType { get; private set; }

        /// <summary>Gets source member.</summary>
        /// <value>The source member.</value>
        public string SourceMember { get; private set; }

        /// <summary>Gets target member.</summary>
        /// <value>The target member.</value>
        public string TargetMember { get; private set; }

        #endregion Properties
    }
}