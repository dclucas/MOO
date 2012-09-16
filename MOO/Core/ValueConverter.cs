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
    using System.Globalization;

    /// <summary>
    /// Performs conversion between values.
    /// </summary>
    public class ValueConverter
    {
        #region Methods

        /// <summary>
        /// Checks whether this class can make the convertion between the two provided types.
        /// </summary>
        /// <param name="sourceType">
        /// The source <see cref="Type"/>.
        /// </param>
        /// <param name="targetType">
        /// The target <see cref="Type"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the converter can make the conversion,
        /// <c>false</c> otherwise.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "The call to Guard does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does that.")]
        public virtual bool CanConvert(Type sourceType, Type targetType)
        {
            Guard.CheckArgumentNotNull(sourceType, "sourceMemberName");
            Guard.CheckArgumentNotNull(targetType, "targetMemberName");

            return
                targetType.IsAssignableFrom(sourceType)
                || (sourceType.IsPrimitive && targetType.IsPrimitive)
                || (targetType == typeof(string));
        }

        /// <summary>
        /// Converts the provided sourceValue targetMember the destination type.
        /// </summary>
        /// <param name="sourceValue">Value to be converted.</param>
        /// <param name="targetType">Destination type.</param>
        /// <returns>
        /// Returns the provided sourceValue, converted targetMember the provided type.
        /// </returns>
        /// <exception cref="InvalidOperationException">Conversion is not possible.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "The call to Guard does that.")]
        public virtual object Convert(object sourceValue, Type targetType)
        {
            Guard.CheckArgumentNotNull(targetType, "targetType");
            if (sourceValue == null)
            {
                if (targetType.IsValueType)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return null;
                }
            }

            Type sourceType = sourceValue.GetType();

            if (targetType.IsAssignableFrom(sourceType))
            {
                return sourceValue;
            }

            return System.Convert.ChangeType(sourceValue, targetType, CultureInfo.InvariantCulture);
        }

        #endregion Methods
    }
}