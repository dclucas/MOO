//-----------------------------------------------------------------------
// <copyright file="ManualMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo.Mappers
{
    using Moo.Core;

    /// <summary>
    /// Manual mappers allow the addition of custom mapping rules, through code.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ManualMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
    {
        /// <summary>
        /// Adds the mapping action.
        /// </summary>
        /// <param name="sourceMemberName">Name of the source member.</param>
        /// <param name="targetMemberName">The target member member.</param>
        /// <param name="mappingAction">The mapping action.</param>
        public void AddMappingAction(
            string sourceMemberName, 
            string targetMemberName, 
            MappingAction<TSource, TTarget> mappingAction)
        {
            var info = new DelegateMappingInfo<TSource, TTarget>(sourceMemberName, targetMemberName, mappingAction);
            this.AddMappingInfo(info);
        }

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        /// <remarks>
        /// As this mapper should generate absolutely no mappings (all of its mappings are to
        /// be added manually), this method is intentionally left blank.
        /// </remarks>
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
        }
    }
}
