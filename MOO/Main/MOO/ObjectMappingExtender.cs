/*-----------------------------------------------------------------------------
Copyright 2010 Diogo Lucas

This file is part of Moo.

Foobar is free software: you can redistribute it and/or modify it under the 
terms of the GNU General Public License as published by the Free Software 
Foundation, either version 3 of the License, or (at your option) any later 
version.

Moo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with 
Moo. If not, see http://www.gnu.org/licenses/.
---------------------------------------------------------------------------- */
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