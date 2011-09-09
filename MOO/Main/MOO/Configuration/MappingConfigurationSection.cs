//-----------------------------------------------------------------------
// <copyright file="MappingConfigurationSection.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a Moo mapping configuration section.
    /// </summary>
    public class MappingConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingConfigurationSection"/> class.
        /// </summary>
        public MappingConfigurationSection()
        {
            this["TypeMappings"] = new TypeMappingCollection();
        }

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
    }
}
