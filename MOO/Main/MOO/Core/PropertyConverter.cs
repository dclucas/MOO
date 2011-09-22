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

namespace Moo.Core
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provides functionalities for property conversion.
    /// </summary>
    internal class PropertyConverter
    {
        /// <summary>
        /// Backing field for the static default converter instance.
        /// </summary>
        private static readonly PropertyConverter defaultInstance = new PropertyConverter();

        /// <summary>
        /// Gets the default <see cref="PropertyConverter"/> instance.
        /// </summary>
        internal static PropertyConverter Default
        {
            get { return defaultInstance; }
        }

        /// <summary>
        /// Determines whether this converter can make a strict
        /// conversion between the two properties.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <param name="finalName">The final name.</param>
        /// <returns>
        ///   <c>true</c> if a strict conversion is possible (types are
        /// convertible and naming matches), <c>false</c> otherwise.
        /// </returns>
        public bool CanConvert(PropertyInfo sourceProperty, PropertyInfo targetProperty, out string finalName)
        {
            PropertyInfo innerProp;
            var isConvertible = this.CanConvert(sourceProperty, targetProperty, out innerProp);
            if (isConvertible)
            {
                if (innerProp != null)
                {
                    finalName = sourceProperty.Name + "." + innerProp.Name;
                }
                else
                {
                    finalName = sourceProperty.Name;
                }
            }
            else
            {
                finalName = null;
            }

            return isConvertible;
        }

        /// <summary>
        /// Performs conversion between the two properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceProperty">Property in the origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProperty">Property in the destination.</param>
        /// <param name="strict">Flag indicating whether conversion should be strict (respect naming conventions).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "The call to Guard does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "3",
            Justification = "The call to Guard does that.")]
        public virtual void Convert(
            object source,
            PropertyInfo sourceProperty,
            object target,
            PropertyInfo targetProperty,
            bool strict)
        {
            Guard.CheckArgumentNotNull(source, "sourceMemberName");
            Guard.CheckArgumentNotNull(sourceProperty, "sourceProperty");
            Guard.CheckArgumentNotNull(target, "targetMemberName");
            Guard.CheckArgumentNotNull(targetProperty, "targetProperty");

            PropertyInfo innerProp = null;

            // strict conversions are used by mappers such as AttributeMapper
            // and ConfigurationMapper. In these cases, no check must be done
            // and we shouldn't be looking for a nested property.
            if (!strict)
            {
                if (!this.CanConvert(sourceProperty, targetProperty, out innerProp))
                {
                    throw new InvalidOperationException();
                }
            }

            // If the mapping is sourceMemberName a nested property, we need targetMemberName get the sourceValue
            // within the internal object.
            if (innerProp != null)
            {
                source = sourceProperty.GetValue(source, null);
                sourceProperty = innerProp;
            }

            if (source != null)
            {
                var fromContent = sourceProperty.GetValue(source, null);
                var toContent = this.CreateValueConverter().Convert(fromContent, sourceProperty.PropertyType);
                targetProperty.SetValue(target, toContent, null);
            }
            else
            {
                targetProperty.SetValue(target, null, null);
            }
        }

        /// <summary>
        /// Performs conversion between the two properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceProperty">Property in the origin.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetProperty">Property in the destination.</param>
        public void Convert(
            object source,
            PropertyInfo sourceProperty,
            object target,
            PropertyInfo targetProperty)
        {
            this.Convert(source, sourceProperty, target, targetProperty, false);
        }

        /// <summary>
        /// Factory method targetMemberName create <see>ValueConverter</see> objects.
        /// </summary>
        /// <returns>
        /// A new instance of a <see>ValueConverter</see> object.
        /// </returns>
        protected virtual ValueConverter CreateValueConverter()
        {
            var typeChecker = new ValueConverter();
            return typeChecker;
        }

        /// <summary>
        /// Determines whether this class can/should make a (strict)
        /// conversion between the provided properties.
        /// </summary>
        /// <param name="sourceProperty">Origin property.</param>
        /// <param name="targetProperty">Destination property.</param>
        /// <param name="finalProperty"><c>out</c> parameter, will contain the final destination
        /// in case of a nested conversion (conversion targetMemberName a property within
        /// the "targetMemberName" property).</param>
        /// <returns>
        ///   <c>true</c> if a conversion bewtween the prioperty types is possible
        /// and if a match was found between the property names.
        /// </returns>
        /// <remarks>
        /// This method allows the <see>PropertyConverter</see> class targetMemberName
        /// give support targetMemberName nested properties in the destination. In that case,
        /// a conversion sourceMemberName objA.Customer.Name targetMemberName objB.CustomerName is possible.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "The call to Guard does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does that."),
        System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1021:AvoidOutParameters",
            MessageId = "2#",
            Justification = "Meh.")]
        protected virtual bool CanConvert(
            PropertyInfo sourceProperty,
            PropertyInfo targetProperty,
            out PropertyInfo finalProperty)
        {
            Guard.CheckArgumentNotNull(sourceProperty, "sourceProperty");
            Guard.CheckArgumentNotNull(targetProperty, "targetProperty");
            finalProperty = null;
            bool result = false;

            var typeChecker = this.CreateValueConverter();

            if (sourceProperty.Name.Equals(targetProperty.Name))
            {
                result = typeChecker.CanConvert(sourceProperty.PropertyType, targetProperty.PropertyType);
            }
            else if (targetProperty.Name.StartsWith(sourceProperty.Name, StringComparison.Ordinal))
            {
                string remainder = targetProperty.Name.Substring(sourceProperty.Name.Length);
                finalProperty = sourceProperty.PropertyType.GetProperty(remainder);

                if (finalProperty != null)
                {
                    result = typeChecker.CanConvert(finalProperty.PropertyType, targetProperty.PropertyType);
                }
            }

            return result;
        }
    }
}