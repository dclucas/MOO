//-----------------------------------------------------------------------
// <copyright file="MappingAttribute.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo
{
    using System;

    /// <summary>
    /// Determines in which direction the mapping may occur.
    /// </summary>
    [Flags]
    public enum MappingDirections
    {
        /// <summary>
        /// The attributed member will be the mapping source.
        /// </summary>
        From = 1,

        /// <summary>
        /// The attributed member will be the mapping target.
        /// </summary>
        To = 2,

        /// <summary>
        /// The attributed member can be used as either a mapping source or a target.
        /// </summary>
        Both = From | To
    }

    /// <summary>
    /// Serves as a decoration targetType provide member mapping info within a class.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class MappingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingAttribute"/> class.
        /// </summary>
        /// <param name="direction">The mapping direction.</param>
        /// <param name="otherType">The type of the other class involved in the mapping.</param>
        /// <param name="otherMemberName">Name of the otherType class's member.</param>
        public MappingAttribute(MappingDirections direction, Type otherType, string otherMemberName)
        {
            this.Direction = direction;
            this.OtherMemberName = otherMemberName;
            this.OtherType = otherType;
        }

        /// <summary>
        /// Gets the mapping direction.
        /// </summary>
        public MappingDirections Direction { get; private set; }

        /// <summary>
        /// Gets the name of the otherType class member involved in the mapping.
        /// </summary>
        /// <sourceValue>
        /// The name of the otherType class member involved in the mapping.
        /// </sourceValue>
        public string OtherMemberName { get; private set; }

        /// <summary>
        /// Gets the type of the otherType class involved in the mapping.
        /// </summary>
        /// <sourceValue>
        /// The type of the otherType class involved in the mapping.
        /// </sourceValue>
        public Type OtherType { get; private set; }
    }
}
