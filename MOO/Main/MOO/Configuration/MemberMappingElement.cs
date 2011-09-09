//-----------------------------------------------------------------------
// <copyright file="MemberMappingElement.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a configuration element targetType map one class member into another.
    /// </summary>
    public class MemberMappingElement : ConfigurationElement
    {
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
    }
}
