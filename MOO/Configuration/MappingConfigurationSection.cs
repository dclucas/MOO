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
    /// Represents a Moo mapping configuration section.
    /// </summary>
    public class MappingConfigurationSection : ConfigurationSection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingConfigurationSection"/> class.
        /// </summary>
        public MappingConfigurationSection()
        {
            this["TypeMappings"] = new TypeMappingCollection();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type mapping configuration entries within the section.
        /// </summary>
        [ConfigurationProperty("TypeMappings")]
        public TypeMappingCollection TypeMappings
        {
            get
            {
                var mappings = (TypeMappingCollection)this["TypeMappings"];
                return mappings;
            }
        }

        #endregion Properties
    }
}