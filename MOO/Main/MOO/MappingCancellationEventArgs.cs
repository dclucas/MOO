//-----------------------------------------------------------------------
// <copyright file="MappingCancellationEventArgs.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo
{
    /// <summary>
    /// Event args for cancellation-enabled events.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class MappingCancellationEventArgs<TSource, TTarget> : MappingEventArgs<TSource, TTarget>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MappingCancellationEventArgs&lt;TSource, TTarget&gt;"/> should be canceled.
        /// </summary>
        /// <sourceValue>
        ///   <c>true</c> if should cancel; otherwise, <c>false</c>.
        /// </sourceValue>
        public bool Cancel { get; set; }
    }
}
