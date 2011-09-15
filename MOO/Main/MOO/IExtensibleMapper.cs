//-----------------------------------------------------------------------
// <copyright file="IExtensibleMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo
{
    /// <summary>
    /// Represents a property mapping method.
    /// </summary>
    /// <typeparam name="TSource">
    /// Source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Destination type.
    /// </typeparam>
    /// <param name="source">
    /// Source object.
    /// </param>
    /// <param name="target">
    /// Destination object.
    /// </param>
    public delegate void MappingAction<TSource, TTarget>(TSource source, TTarget target);

    /// <summary>
    /// Interface for extensible mappers.
    /// </summary>
    /// <typeparam name="TSource">
    /// Origin type for the mapping.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Destination type for tha mapping.
    /// </typeparam>
    public interface IExtensibleMapper<TSource, TTarget> : IMapper<TSource, TTarget>
    {
        /// <summary>
        /// Adds a mapping rule for the specified members.
        /// </summary>
        /// <param name="sourceMemberName">
        /// Source member.
        /// </param>
        /// <param name="targetMemberName">
        /// Destination member.
        /// </param>
        /// <param name="mappingAction">
        /// The delegate that will perform the conversion.
        /// </param>
        void AddMappingAction(string sourceMemberName, string targetMemberName, MappingAction<TSource, TTarget> mappingAction);
    }
}