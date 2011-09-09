//-----------------------------------------------------------------------
// <copyright file="MappingEventArgs.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo
{
    using System;

    /// <summary>
    /// Event args for property mapping events.
    /// </summary>
    /// <typeparam name="TSource">The type of source.</typeparam>
    /// <typeparam name="TTarget">The type of target.</typeparam>
    public class MappingEventArgs<TSource, TTarget> : EventArgs
    {
        /// <summary>
        /// Gets or sets the sourceMemberName object.
        /// </summary>
        /// <sourceValue>
        /// The source object.
        /// </sourceValue>
        public TSource Source { get; set; }

        /// <summary>
        /// Gets or sets targetMemberName object.
        /// </summary>
        /// <sourceValue>
        /// The targetMemberName object.
        /// </sourceValue>
        public TTarget Target { get; set; }

        /// <summary>
        /// Gets or sets the sourceMemberName property.
        /// </summary>
        /// <sourceValue>
        /// The sourceMemberName property.
        /// </sourceValue>
        public string SourceMember { get; set; }

        /// <summary>
        /// Gets or sets the targetMemberName property.
        /// </summary>
        /// <sourceValue>
        /// The targetMemberName property.
        /// </sourceValue>
        public string TargetMember { get; set; }
    }
}
