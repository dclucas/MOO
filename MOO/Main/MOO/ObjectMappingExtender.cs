using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moo.Core;

namespace Moo
{
    /// <summary>
    /// Extends the <c>Object</c> class, providing mapping capabilities for all objects.
    /// </summary>
    public static class ObjectMappingExtender
    {
        /// <summary>
        /// Maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="mapper">The mapper to be used.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the provided mapper.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source, IMapper mapper)
        {
            Guard.CheckArgumentNotNull(source, "source");
            Guard.CheckArgumentNotNull(mapper, "mapper");
            return (TTarget)mapper.Map(source);
        }

        /// <summary>
        /// Maps the values on the object to a target object.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <param name="mapper">The mapper to be used.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the provided mapper.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source, TTarget target, IMapper mapper)
        {
            Guard.CheckArgumentNotNull(source, "source");
            Guard.CheckArgumentNotNull(target, "target");
            Guard.CheckArgumentNotNull(mapper, "mapper");
            return (TTarget)mapper.Map(source, target);
        }

        /// <summary>
        /// Maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="repo">The repository that will provide the mapper.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the mapper provided provided by the repository.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source, IMappingRepository repo)
        {
            Guard.CheckArgumentNotNull(source, "source");
            Guard.CheckArgumentNotNull(repo, "repo");
            var mapper = repo.ResolveMapper(source.GetType(), typeof(TTarget));
            return (TTarget)mapper.Map(source);
        }

        /// <summary>
        /// Maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <param name="repo">The repository that will provide the mapper.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the mapper provided provided by the repository.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source, TTarget target, IMappingRepository repo)
        {
            Guard.CheckArgumentNotNull(source, "source");
            Guard.CheckArgumentNotNull(target, "target");
            Guard.CheckArgumentNotNull(repo, "repo");
            var mapper = repo.ResolveMapper(source.GetType(), typeof(TTarget));
            return (TTarget)mapper.Map(source, target);
        }

        /// <summary>
        /// Maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the mapper provided provided by the default repository.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source)
        {
            return MapTo<TTarget>(source, MappingRepository.Default);
        }

        /// <summary>
        /// Maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// The target object, filled according to the mapping instructions in the mapper provided provided by the default repository.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The call to Guard already does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that.")]
        public static TTarget MapTo<TTarget>(this Object source, TTarget target)
        {
            return MapTo<TTarget>(source, target, MappingRepository.Default);
        }
    }
}