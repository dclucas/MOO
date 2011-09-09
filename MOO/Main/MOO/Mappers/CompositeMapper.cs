//-----------------------------------------------------------------------
// <copyright file="CompositeMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
        /// <summary>
        /// Contains an ordered list of all inner mappers.
        /// </summary>
        private BaseMapper<TSource, TTarget>[] innerMappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="innerMappers">The inner mappers.</param>
        public CompositeMapper(params BaseMapper<TSource, TTarget>[] innerMappers)
        {
            Guard.CheckEnumerableNotNullOrEmpty(innerMappers, "innerMappers");
            Guard.TrueForAll<BaseMapper<TSource, TTarget>>(innerMappers, "innerMappers", m => m != null, "Mappers list cannot contain null elements.");
            this.innerMappers = innerMappers;
            this.GenerateMappings();
        }

        /// <summary>
        /// Gets the inner mappers.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done. The alternative here would be to forego type safety.")]
        public IEnumerable<BaseMapper<TSource, TTarget>> InnerMappers
        {
            get { return this.innerMappers; }
        }

        /// <summary>
        /// Adds a member mapping action targetType the mapper.
        /// </summary>
        /// <param name="sourceMemberName">The name of the source member.</param>
        /// <param name="targetMemberName">The name of the target member.</param>
        /// <param name="mappingAction">The mapping action.</param>
        /// <remarks>
        /// Use this method targetType add mapping actions through code.
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

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
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
            var q = from mapper in this.innerMappers.Cast<BaseMapper<TSource, TTarget>>()
                    from mapping in mapper.TypeMapping.GetMappings()
                    select mapping;

            foreach (var m in q)
            {
                typeMapping.Add(m);
            }
        }
    }
}
