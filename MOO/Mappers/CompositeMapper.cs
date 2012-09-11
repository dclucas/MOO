// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Moo.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Moo.Core;
    using System.Reflection;

    /// <summary>
    /// Allows the combination of multiple mapper classes into one.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class CompositeMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="innerMappers">The inner mappers.</param>
        public CompositeMapper(params IMapper<TSource, TTarget>[] innerMappers)
        {
            this.Initialize(innerMappers);
            TypeMapping = new TypeMappingInfo<TSource, TTarget>(MappingOverwriteBehavior.SkipOverwrite);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMapper{TSource,TTarget}"/> class. 
        /// </summary>
        /// <param name="constructionInfo">
        /// Mapper construction information.
        /// </param>
        /// <param name="innerMappers">
        /// The inner mappers.
        /// </param>
        public CompositeMapper(MapperConstructionInfo constructionInfo, params IMapper<TSource, TTarget>[] innerMappers)
            : base(constructionInfo)
        {
            this.Initialize(innerMappers);
            TypeMapping = new TypeMappingInfo<TSource, TTarget>(MappingOverwriteBehavior.SkipOverwrite);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the inner mappers.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done. The alternative here would be to forego type safety.")]
        public IEnumerable<IMapper<TSource, TTarget>> InnerMappers { get; private set; }

        public IExtensibleMapper<TSource, TTarget> ExtensibleMapper { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs basic validation and fires off mapping generation.
        /// </summary>
        /// <param name="innerMappers">
        /// List of internal mappers to use.
        /// </param>
        private void Initialize(IMapper<TSource, TTarget>[] innerMappers)
        {
            Guard.CheckEnumerableNotNullOrEmpty(innerMappers, "innerMappers");
            Guard.TrueForAll(innerMappers, "innerMappers", m => m != null, "Mappers list cannot contain null elements.");
            this.InnerMappers = innerMappers.Reverse();
            this.ExtensibleMapper = innerMappers.OfType<IExtensibleMapper<TSource, TTarget>>().FirstOrDefault();
        }

        /// <summary>
        /// Adds a member mapping action to the mapper.
        /// </summary>
        /// <param name="sourceMemberName">The name of the source member.</param>
        /// <param name="targetMemberName">The name of the target member.</param>
        /// <param name="mappingAction">The mapping action.</param>
        /// <remarks>
        /// Use this method to add mapping actions through code.
        /// </remarks>
        public void AddMappingAction(
            string sourceMemberName,
            string targetMemberName,
            MappingAction<TSource, TTarget> mappingAction)
        {
            /*
            var info = new DelegateMappingInfo<TSource, TTarget>(
                sourceMemberName,
                targetMemberName,
                mappingAction);

            this.AddMappingInfo(info);
             */
            ExtensibleMapper.AddMappingAction(sourceMemberName, targetMemberName, mappingAction);
        }

        /// <summary>
        /// Adds new mapping actions to the mapper, with <c>From</c> and <c>To</c> statements.
        /// </summary>
        /// <returns>A ISourceSpec object, for fluent mapping.</returns>
        public ISourceSpec<TSource, TTarget> AddMapping()
        {
            return new SourceSpec<TSource, TTarget>(this);
        }

        protected internal override IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            foreach (var mapper in this.InnerMappers.OfType<BaseMapper<TSource, TTarget>>())
            {
                var mappings = mapper.GetMappings();
                if (mappings != null)
                {
                    foreach (var mapping in mappings)
                    {
                        yield return mapping;
                    }
                }
            }
        }

        public override void AddInnerMapper<TInnerSource, TInnerTarget>(PropertyInfo sourceMemberName, PropertyInfo targetMemberName)
        {
            ExtensibleMapper.AddInnerMapper<TInnerSource, TInnerTarget>(sourceMemberName, targetMemberName);
        }

        #endregion Methods
    }
}