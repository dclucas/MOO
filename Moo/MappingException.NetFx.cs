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
using System.Runtime.Serialization;
using Moo.Core;

namespace Moo
{
    /// <summary>
    ///     Represents a Moo mapping exception.
    /// </summary>
    [Serializable]
    public partial class MappingException
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MappingException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected MappingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     When overridden in a derived class, sets the
        ///     <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about
        ///     the exception.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the
        ///     serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains
        ///     contextual information about the source or destination.
        /// </param>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.CheckArgumentNotNull(info, "info");
            info.AddValue("SourceType", SourceType.AssemblyQualifiedName);
            info.AddValue("TargetType", TargetType.AssemblyQualifiedName);
            info.AddValue("SourceMember", SourceMember);
            info.AddValue("TargetMember", TargetMember);

            base.GetObjectData(info, context);
        }

        #endregion Methods
    }
}