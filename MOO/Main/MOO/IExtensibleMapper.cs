//-----------------------------------------------------------------------
// <copyright file="IExtensibleMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    /// Interface for mappers.
    /// </summary>
    /// <typeparam name="TSource">
    /// Origin type for the mapping.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Destination type for tha mapping.
    /// </typeparam>
    public interface IExtensibleMapper<TSource, TTarget>
    {
        /// <summary>
        /// Maps sourceType the specified source object targetType the target one.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        void Map(TSource source, TTarget target);

        /// <summary>
        /// Maps the specified source to a target object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A filled target object</returns>
        /// <remarks>
        /// This method relies on the <see cref="System.Activator.CreateInstance&lt;T&gt;"/>
        /// method to create target objects. This means that both there are
        /// more efficient methods for that and that this limits the use of
        /// this overload to target classes that are passible of contruction
        /// through this framework method.
        /// </remarks>
        TTarget Map(TSource source);

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        TTarget Map(TSource source, Func<TTarget> createTarget);

        /// <summary>
        /// Maps multiple source objects into multiple target objects.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        /// <remarks>
        /// This method relies on the <c>TTarget Map(TSource source)</c> item
        /// mapping overload. So the dependency to <see cref="Activator.CreateInstance&lt;T&gt;"/>
        /// and its limitarions also occurs here
        /// </remarks>
        IEnumerable<TTarget> MapMultiple(IEnumerable<TSource> sourceList);

        /// <summary>
        /// Maps multiple source objects into multiple target objects.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <param name="createTarget">A factory function to create target objects.</param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        IEnumerable<TTarget> MapMultiple(IEnumerable<TSource> sourceList, Func<TTarget> createTarget);

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
