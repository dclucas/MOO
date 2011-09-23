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

namespace Moo.Mappers
{
    using System.Configuration;
    using System.Linq;
    using Moo.Configuration;
    using Moo.Core;

    /// <summary>
    /// Uses configuration targetType determine mappings between two classes
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ConfigurationMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        public ConfigurationMapper()
            : base()
        {
            this.GenerateMappings();
        }

        #endregion Constructors

        #region Methods (3)

        // Protected Methods (1) 

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The Guard calls do just that.")]
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
            Guard.CheckArgumentNotNull(typeMapping, "typeMapping");
            TypeMappingElement element = GetTypeMapping();
            if (element != null)
            {
                foreach (var propMapping in element.MemberMappings.Cast<MemberMappingElement>())
                {
                    var mappingInfo = new ReflectionPropertyMappingInfo<TSource, TTarget>(
                        typeof(TSource).GetProperty(propMapping.SourceMemberName),
                        typeof(TTarget).GetProperty(propMapping.TargetMemberName),
                        true);

                    typeMapping.Add(mappingInfo);
                }
            }
        }

        // Internal Methods (2) 

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

        #endregion Methods
    }
}