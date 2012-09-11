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
    using System.Reflection;

    /// <summary>
    /// Represents a reflection-based mapping info for a given property pair.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ReflectionPropertyMappingInfo<TSource, TTarget> : MemberMappingInfo<TSource, TTarget>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionPropertyMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="strict">if set to <c>true</c>, performs a strict mapping.</param>
        /// <param name="converter">The property converter to be used.</param>
        internal ReflectionPropertyMappingInfo(
            PropertyInfo sourceProperty,
            PropertyInfo targetProperty,
            bool strict,
            PropertyConverter converter)
        {
            Guard.CheckArgumentNotNull(sourceProperty, "sourceMemberName");
            Guard.CheckArgumentNotNull(targetProperty, "targetMemberName");
            Guard.CheckArgumentNotNull(converter, "converter");
            this.SourceMemberName = sourceProperty.Name;
            this.TargetMemberName = targetProperty.Name;
            this.FromPropertyInfo = sourceProperty;
            this.ToPropertyInfo = targetProperty;
            this.Converter = converter;
            this.Strict = strict;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionPropertyMappingInfo&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="strict">if set to <c>true</c>, performs a strict mapping.</param>
        internal ReflectionPropertyMappingInfo(
            PropertyInfo sourceProperty,
            PropertyInfo targetProperty,
            bool strict = false)
            : this(sourceProperty, targetProperty, strict, PropertyConverter.Default)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the converter.
        /// </summary>
        public PropertyConverter Converter { get; private set; }

        /// <summary>
        /// Gets from property info.
        /// </summary>
        public PropertyInfo FromPropertyInfo { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ReflectionPropertyMappingInfo&lt;TSource, TTarget&gt;"/> is strict.
        /// </summary>
        /// <value>
        ///   <c>true</c> if strict; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property determines whether conversion needs to be strict (the exact same members in the
        /// sourceMemberName and targetMemberName arguments) fields must be used or lose
        /// (which allows property folding/unfolding).
        /// </remarks>
        public bool Strict { get; private set; }

        /// <summary>
        /// Gets to property info.
        /// </summary>
        public PropertyInfo ToPropertyInfo { get; private set; }

        #endregion Properties

        #region Methods

         

        /// <summary>
        /// Maps from the specified source to the specified target.
        /// </summary>
        /// <param name="source">The mapping source.</param>
        /// <param name="target">The mapping target.</param>
        public override void Map(TSource source, TTarget target)
        {
            this.Converter.Convert(source, this.FromPropertyInfo, target, this.ToPropertyInfo, this.Strict);
        }

        #endregion Methods
    }
}