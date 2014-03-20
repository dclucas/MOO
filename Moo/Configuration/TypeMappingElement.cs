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

using System.Configuration;

namespace Moo.Configuration
{
    /// <summary>
    ///     Contains configuration to map between two classes.
    /// </summary>
    public class TypeMappingElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        ///     Gets the member mappings.
        /// </summary>
        [ConfigurationProperty("MemberMappings")]
        public MemberMappingCollection MemberMappings
        {
            get { return (MemberMappingCollection) this["MemberMappings"]; }
        }

        /// <summary>
        ///     Gets or sets the name of the source type.
        /// </summary>
        /// <sourceValue>
        ///     The name of the source type.
        /// </sourceValue>
        [ConfigurationProperty("SourceType")]
        public string SourceType
        {
            get { return (string) this["SourceType"]; }
            set { this["SourceType"] = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the target type.
        /// </summary>
        /// <sourceValue>
        ///     The name of the target type.
        /// </sourceValue>
        [ConfigurationProperty("TargetType")]
        public string TargetType
        {
            get { return (string) this["TargetType"]; }
            set { this["TargetType"] = value; }
        }

        #endregion Properties
    }
}