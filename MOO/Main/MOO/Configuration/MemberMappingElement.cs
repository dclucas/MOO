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

namespace Moo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a configuration element targetType map one class member into another.
    /// </summary>
    public class MemberMappingElement : ConfigurationElement
    {
        #region Properties (2)

        /// <summary>
        /// Gets or sets the name of the source member.
        /// </summary>
        /// <sourceValue>
        /// The name of the source member.
        /// </sourceValue>
        [ConfigurationProperty("SourceMemberName")]
        public string SourceMemberName
        {
            get { return (string)this["SourceMemberName"]; }
            set { this["SourceMemberName"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the target member.
        /// </summary>
        /// <sourceValue>
        /// The name of the target member.
        /// </sourceValue>
        [ConfigurationProperty("TargetMemberName")]
        public string TargetMemberName
        {
            get { return (string)this["TargetMemberName"]; }
            set { this["TargetMemberName"] = value; }
        }

        #endregion Properties
    }
}