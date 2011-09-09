//-----------------------------------------------------------------------
// <copyright file="DelegateMappingInfo.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo.Core
{
    /// <summary>
    /// Basic information on how to map from one class member to another.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    internal class DelegateMappingInfo<TSource, TTarget> : MemberMappingInfo<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="mappingAction">The mapping action.</param>
        public DelegateMappingInfo(
            string source, 
            string target, 
            MappingAction<TSource, TTarget> mappingAction)
            : base(source, target)
        {
            Guard.CheckArgumentNotNull(mappingAction, "mappingAction");
            this.MappingAction = mappingAction;
        }

        /// <summary>
        /// Gets the mapping action.
        /// </summary>
        public MappingAction<TSource, TTarget> MappingAction { get; private set; }

        /// <summary>
        /// Maps from the specified source to the specified target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public override void Map(TSource source, TTarget target)
        {
            this.MappingAction(source, target);
        }
    }
}
