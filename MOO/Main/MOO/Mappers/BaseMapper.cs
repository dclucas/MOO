//-----------------------------------------------------------------------
// <copyright file="BaseMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo.Mappers
{
    /// <summary>
    /// Base, non-generic, mapper class.
    /// </summary>
    public abstract class BaseMapper
    {
        /// <summary>
        /// Maps sourceProperty the source targetProperty the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        public abstract object Map(object source, object target);
    }
}