// -----------------------------------------------------------------------
// <copyright file="AssociationMapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Moo.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Moo.Core;

    /// <summary>
    /// The association mapper uses a list of provided mappers to map complex internal properties.
    /// </summary>
    /// <typeparam name="TSource">Type of the source object.</typeparam>
    /// <typeparam name="TTarget">Type of the target object.</typeparam>
    public class AssociationMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssociationMapper{TSource,TTarget}"/> class. 
        /// </summary>
        /// <param name="constructionInfo">Mapper construction information.</param>
        public AssociationMapper(MapperConstructionInfo constructionInfo)
            : base(constructionInfo)
        {
            this.GenerateMappings();
        }

        /// <summary>
        /// Generates the member mappings and adds them to the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", 
            "CA1062:Validate arguments of public methods", 
            MessageId = "0", 
            Justification = "The call to Guard does this check.")]
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
            Guard.CheckArgumentNotNull(typeMapping, "typeMapping");

            typeMapping.AddRange(GetMappings());
        }

        protected override IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            return from sourceProperty in typeof(TSource).GetProperties()
                   from targetProperty in typeof(TTarget).GetProperties()
                   from m in MapperInclusions
                   where m.SourceType == sourceProperty.PropertyType
                   where m.TargetType == targetProperty.PropertyType
                   let mapper = ParentRepository.ResolveMapper(m.SourceType, m.TargetType)
                   select new MapperMappingInfo<TSource, TTarget>(mapper, sourceProperty, targetProperty);

        }
    }
}
