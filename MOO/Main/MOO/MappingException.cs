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
namespace Moo
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents a Moo mapping exception.
    /// </summary>
    [global::System.Serializable]
    [DebuggerNonUserCode]
    public class MappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        public MappingException() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
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
        /// Initializes a new instance of the <see cref="MappingException"/> class.
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
                String.Format(
                    CultureInfo.InvariantCulture,
                    "Error mapping source {0}.{1} to {2}.{3}", 
                    sourceType, 
                    sourceMember, 
                    targetType, 
                    targetMember), 
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MappingException(string message) : base(message) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner exception.</param>
        public MappingException(string message, Exception inner) : base(message, inner)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected MappingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        { 
        }
    }
}
