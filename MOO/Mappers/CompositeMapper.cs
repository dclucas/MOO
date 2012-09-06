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

    /// <summary>
    /// Allows the combination of multiple mapper classes into one.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class CompositeMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
    {
        #region Fields (1)

        /// <summary>
        /// Contains an ordered list of all inner mappers.
        /// </summary>
        private IMapper<TSource, TTarget>[] _innerMappers;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="innerMappers">The inner mappers.</param>
        public CompositeMapper(params IMapper<TSource, TTarget>[] innerMappers)
        {
            this.Initialize(innerMappers);
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
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the inner mappers.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done. The alternative here would be to forego type safety.")]
        public IEnumerable<IMapper<TSource, TTarget>> InnerMappers
        {
            get { return this._innerMappers; }
        }

        #endregion Properties

        #region Methods (2)

        private void Initialize(IMapper<TSource, TTarget>[] innerMappers)
        {
            Guard.CheckEnumerableNotNullOrEmpty(innerMappers, "innerMappers");
            Guard.TrueForAll(innerMappers, "innerMappers", m => m != null, "Mappers list cannot contain null elements.");
            this._innerMappers = innerMappers;
            this.GenerateMappings();
        }

        // Public Methods (1) 

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
            var info = new DelegateMappingInfo<TSource, TTarget>(
                sourceMemberName,
                targetMemberName,
                mappingAction);

            AddMappingInfo(info);
        }

        // Protected Methods (1) 

        /// <summary>
        /// Generates the member mappings and adds them to the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The Guard call does just that.")]
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
            Guard.CheckArgumentNotNull(typeMapping, "typeMapping");
            var q = from mapper in InnerMappers.OfType<BaseMapper<TSource, TTarget>>()
                    from mapping in mapper.TypeMapping.GetMappings()
                    select mapping;

            foreach (var m in q)
            {
                typeMapping.Add(m);
            }
        }

        #endregion Methods
    }
}