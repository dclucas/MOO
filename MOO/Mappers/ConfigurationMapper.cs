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

namespace Moo.Mappers
{
    using System.Configuration;
    using System.Linq;
    using Moo.Configuration;
    using Moo.Core;
    using System.Collections.Generic;

    /// <summary>
    /// Uses configuration to determine mappings between two classes
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ConfigurationMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        public ConfigurationMapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMapper{TSource,TTarget}"/> class. 
        /// </summary>
        /// <param name="constructionInfo">Mapper construction information.</param>
        public ConfigurationMapper(MapperConstructionInfo constructionInfo)
            : base(constructionInfo)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the type mapping configuration element.
        /// </summary>
        /// <returns>
        /// A <see cref="TypeMappingElement"/> instance in case one has been found in the config file,
        /// <c>null</c> otherwise.
        /// </returns>
        internal static TypeMappingElement GetTypeMapping()
        {
            return GetTypeMapping("mooSettings");
        }

        /// <summary>
        /// Gets the type mapping configuration element.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>
        /// A <see cref="TypeMappingElement"/> instance in case one has been found in the config file,
        /// <c>null</c> otherwise.
        /// </returns>
        internal static TypeMappingElement GetTypeMapping(string sectionName)
        {
            Guard.CheckArgumentNotNull(sectionName, "sectionName");
            var section = (MappingConfigurationSection)ConfigurationManager.GetSection(sectionName);
            if (section != null)
            {
                return
                    section.TypeMappings.Cast<TypeMappingElement>().FirstOrDefault(
                        t => typeof(TTarget).AssemblyQualifiedName.Contains(t.TargetType)
                             && typeof(TSource).AssemblyQualifiedName.Contains(t.SourceType));
            }
            else
            {
                return null;
            }
        }

        protected internal override IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            TypeMappingElement element = GetTypeMapping();
            if (element != null)
            {
                return from propMapping in element.MemberMappings.Cast<MemberMappingElement>()
                       select new ReflectionPropertyMappingInfo<TSource, TTarget>(
                        typeof(TSource).GetProperty(propMapping.SourceMemberName),
                        typeof(TTarget).GetProperty(propMapping.TargetMemberName),
                        true);
            }
            else
            {
                return null;
            }
        }

        #endregion Methods
    }
}