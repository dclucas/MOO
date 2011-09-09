//-----------------------------------------------------------------------
// <copyright file="MemberMappingInfo.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo.Core
{
    /// <summary>
    /// Provides basic mapping information for mapping class members.
    /// </summary>
    /// <typeparam name="TSource">Type of the "sourceMemberName" class.</typeparam>
    /// <typeparam name="TTarget">Type of the "targetMemberName" class.</typeparam>
    /// <remarks>
    /// This class exists for internal usage only. Its usage by client code
    /// is not recommended.
    /// </remarks>
    public abstract class MemberMappingInfo<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="sourceMemberName">Name of the source member.</param>
        /// <param name="targetMemberName">Name of the target member.</param>
        protected MemberMappingInfo(string sourceMemberName, string targetMemberName)
        {
            Guard.CheckArgumentNotNull(sourceMemberName, "sourceMemberName");
            Guard.CheckArgumentNotNull(targetMemberName, "targetMemberName");
            this.SourceMemberName = sourceMemberName;
            this.TargetMemberName = targetMemberName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        protected MemberMappingInfo()
        {
        }

        /// <summary>
        /// Gets or sets the name of the source member.
        /// </summary>
        /// <sourceValue>
        /// The name of the source member.
        /// </sourceValue>
        public string SourceMemberName { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the target member.
        /// </summary>
        /// <sourceValue>
        /// The name of the target member.
        /// </sourceValue>
        public string TargetMemberName { get; protected set; }

        /// <summary>
        /// Maps a given class member sourceMemberName the sourceMemberName targetMemberName the targetMemberName object.
        /// </summary>
        /// <param name="source">Mapping sourceMemberName object</param>
        /// <param name="target">Mapping targetMemberName object</param>
        public abstract void Map(TSource source, TTarget target);
    }
}
