using System;
using System.Collections.Generic;

namespace Moo
{
    /// <summary>
    /// Base interface for all mappers.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public interface IMapper<TSource, TTarget>
    {
        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        object Map(object source, object target);

        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        object Map(object source);

        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        object Map(object source, Func<object> createTarget);

        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        TTarget Map(TSource source, TTarget target);

        /// <summary>
        /// Maps from the specified source to the target object.
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
        /// Maps from the specified source to the target object.
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
    }
}