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

namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Determines behavior to use for mapping overwrites
    /// </summary>
    public enum MappingOverwriteBehavior
    {
        /// <summary>
        /// Allows mapping overwrite
        /// </summary>
        AllowOverwrite,

        /// <summary>
        /// Silently skips write operation if a mapping for a give source already exists
        /// </summary>
        SkipOverwrite,

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> if an overwrite attempt happens
        /// </summary>
        ThrowOnOverwrite
    }

    /// <summary>
    /// Contains information targetProperty map between two classes.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class TypeMappingInfo<TSource, TTarget>
    {
        #region Fields

        /// <summary>
        /// Backing field for the internal member mapping collection.
        /// </summary>
        private Dictionary<string, MemberMappingInfo<TSource, TTarget>> memberMappings =
            new Dictionary<string, MemberMappingInfo<TSource, TTarget>>();

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the TypeMappingInfo class.</summary>
        public TypeMappingInfo()
        {
        }

        /// <summary>Initializes a new instance of the TypeMappingInfo class.</summary>
        /// <param name="overwriteBehavior">The overwrite behavior.</param>
        public TypeMappingInfo(MappingOverwriteBehavior overwriteBehavior)
        {
            this.OverwriteBehavior = overwriteBehavior;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        /// <sourceValue>
        /// The type of the source.
        /// </sourceValue>
        public Type SourceType { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <sourceValue>
        /// The type of the target.
        /// </sourceValue>
        public Type TargetType { get; set; }

        /// <summary>Gets the overwrite behavior.</summary>
        /// <value>The overwrite behavior.</value>
        public MappingOverwriteBehavior OverwriteBehavior { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the specified member mapping info.
        /// </summary>
        /// <param name="mappingInfo">The member mapping info.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does that.")]
        public void Add(MemberMappingInfo<TSource, TTarget> mappingInfo)
        {
            Guard.CheckArgumentNotNull(mappingInfo, "mappingInfo");
            switch (this.OverwriteBehavior)
            {
                case MappingOverwriteBehavior.AllowOverwrite:
                    this.memberMappings[mappingInfo.TargetMemberName] = mappingInfo;
                    break;

                case MappingOverwriteBehavior.SkipOverwrite:
                    if (!this.memberMappings.ContainsKey(mappingInfo.TargetMemberName))
                    {
                        this.memberMappings[mappingInfo.TargetMemberName] = mappingInfo;
                    }

                    break;

                case MappingOverwriteBehavior.ThrowOnOverwrite:
                    if (!this.memberMappings.ContainsKey(mappingInfo.TargetMemberName))
                    {
                        throw new MappingException(
                            string.Format(
                                System.Globalization.CultureInfo.InvariantCulture,
                                "Target {0}.{1} was defined multiple times.",
                                typeof(TTarget).FullName,
                                mappingInfo.TargetMemberName),
                            typeof(TSource),
                            typeof(TTarget),
                            mappingInfo.SourceMemberName,
                            mappingInfo.TargetMemberName,
                            null);
                    }

                    break;
            }    
        }

        /// <summary>
        /// Gets the member mappings.
        /// </summary>
        /// <returns>
        /// An <c>IEnumerable</c> containing all member mappings between
        /// <typeparamref name="TSource"/> and <typeparamref name="TTarget"/>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Optional rule, solemnly ignored here -- this get might be costly/have side effects in the future."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done. Not willing to sacrifice type safety here.")]
        public IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings()
        {
            return this.memberMappings.Values;
        }

        /// <summary>
        /// Adds a list of mappings
        /// </summary>
        /// <param name="mappingInfoList">
        /// List to be added.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", 
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It's either that or using an array.")]
        public void AddRange(IEnumerable<MemberMappingInfo<TSource, TTarget>> mappingInfoList)
        {
            Guard.CheckArgumentNotNull(mappingInfoList, "mappingInfoList");
            foreach (var m in mappingInfoList)
            {
                Add(m);
            }
        }

        /// <summary>
        /// Compiles multiple internal mapping entries into a single lambda, for faster execution.
        /// </summary>
        public void Compile()
        {
            var sourceParam = Expression.Parameter(typeof(TSource), "source");
            var targetParam = Expression.Parameter(typeof(TTarget), "target");

            var q = from m in memberMappings
                    let rm = m.Value as ReflectionPropertyMappingInfo<TSource, TTarget>
                    where rm != null
                    select new
                    {
                        CurrentMapping = m,
                        Expression = GetExpression(m.Key, rm, sourceParam, targetParam)
                    };

            var innerExpressions = q.Where(i => i.Expression != null).ToArray();
            if (innerExpressions.Length > 0)
            {
                var block = Expression.Block(innerExpressions.Select(i => i.Expression));
                var lambda = Expression.Lambda<MappingAction<TSource, TTarget>>(block, sourceParam, targetParam);
                var targetNames = innerExpressions.Select(i => i.CurrentMapping.Key);
                var newMapping = new DelegateMappingInfo<TSource, TTarget>(
                    "Multiple sources -- see target.",
                    String.Join(",", targetNames),
                    lambda.Compile());

                foreach (var e in innerExpressions)
                {
                    this.memberMappings.Remove(e.CurrentMapping.Key);
                }

                this.Add(newMapping);
            }
        }

        private PropertyConverter propConverter = new PropertyConverter();

        private Expression GetExpression(
            string targetMemberName, 
            ReflectionPropertyMappingInfo<TSource, TTarget> reflectionInfo,
            ParameterExpression sourceParam,
            ParameterExpression targetParam)
        {
            var converter = new PropertyConverter();
            return converter.CreateConvertExpression(
                reflectionInfo.SourcePropertyInfo,
                reflectionInfo.TargetPropertyInfo,
                sourceParam,
                targetParam);
            /*
            var targetGet = Expression.Property(targetParam, reflectionInfo.TargetPropertyInfo);
            var sourceGet = Expression.Property(sourceParam, reflectionInfo.SourcePropertyInfo);
            if (! reflectionInfo.TargetPropertyInfo.PropertyType.IsAssignableFrom(
                reflectionInfo.SourcePropertyInfo.PropertyType))
            {
                return null;
            }
            var assignment = Expression.Assign(targetGet, sourceGet);
            return assignment;
             */
        }

        #endregion Methods
    }
}