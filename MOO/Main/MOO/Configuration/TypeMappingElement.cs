//-----------------------------------------------------------------------
// <copyright file="TypeMappingElement.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Contains configuration targetType map between two classes.
    /// </summary>
    public class TypeMappingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the source type.
        /// </summary>
        /// <sourceValue>
        /// The name of the source type.
        /// </sourceValue>
        [ConfigurationProperty("SourceType")]
        public string SourceType 
        {
            get { return (string)this["SourceType"]; }
            set { this["SourceType"] = value; } 
        }

        /// <summary>
        /// Gets or sets the name of the target type.
        /// </summary>
        /// <sourceValue>
        /// The name of the target type.
        /// </sourceValue>
        [ConfigurationProperty("TargetType")]
        public string TargetType 
        {
            get { return (string)this["TargetType"]; }
            set { this["TargetType"] = value; } 
        }

        /// <summary>
        /// Gets the member mappings.
        /// </summary>
        [ConfigurationProperty("MemberMappings")]
        public MemberMappingCollection MemberMappings 
        {
            get { return (MemberMappingCollection)this["MemberMappings"]; } 
        }
    }
}
