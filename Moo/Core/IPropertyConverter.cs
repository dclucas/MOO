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
// along Moo.  If not, see http:// www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace Moo.Core
{
    /// <summary>Interface for property converters.</summary>
    public interface IPropertyConverter
    {
        /// <summary>Determine if a source property can be converted to a target one.</summary>
        /// <param name="sourceProperty">Source property.</param>
        /// <param name="targetProperty">Target property.</param>
        /// <returns><c>true</c> if conversion possible, <c>false</c> if not.</returns>
        bool CanConvert(PropertyInfo sourceProperty, PropertyInfo targetProperty);

        /// <summary>Converts from one source property and object to a target property and object.</summary>
        /// <param name="source">        Source for the conversion.</param>
        /// <param name="sourceProperty">Source property for the conversion.</param>
        /// <param name="target">        Target for the conversion.</param>
        /// <param name="targetProperty">Target property for the conversion.</param>
        void Convert(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty);

        /// <summary>
        ///     Converts from one source property and object to a target property and object.
        /// </summary>
        /// <param name="source">        Source for the conversion.</param>
        /// <param name="sourceProperty">Source property for the conversion.</param>
        /// <param name="target">        Target for the conversion.</param>
        /// <param name="targetProperty">Target property for the conversion.</param>
        /// <param name="strict">
        ///     Determines whether strict conversion should be performed. If <c>false</c>, the converter may
        ///     do property unfolding for the conversion.
        /// </param>
        void Convert(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty, bool strict);
    }
}