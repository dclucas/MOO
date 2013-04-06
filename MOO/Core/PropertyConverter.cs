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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides functionalities for property conversion.
    /// </summary>
    public class PropertyConverter : IPropertyConverter
    {
        #region Fields

        /// <summary>
        /// Backing field for the static default converter instance.
        /// </summary>
        private static readonly PropertyConverter DefaultInstance = new PropertyConverter();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the default <see cref="PropertyConverter"/> instance.
        /// </summary>
        internal static PropertyConverter Default
        {
            get { return DefaultInstance; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether this converter can make a strict
        /// conversion between the two properties.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <returns>
        ///   <c>true</c> if a strict conversion is possible (types are
        /// convertible and naming matches), <c>false</c> otherwise.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The call to Guard does this check.")]
        public bool CanConvert(PropertyInfo sourceProperty, PropertyInfo targetProperty)
        {
            Guard.CheckArgumentNotNull(sourceProperty, "sourceProperty");
            Guard.CheckArgumentNotNull(targetProperty, "targetProperty");
            PropertyInfo innerProp;
            var isConvertible = this.CanConvert(sourceProperty, targetProperty, out innerProp);
            return isConvertible;
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

        /// <summary>Creates convert expression from a property to another.</summary>
        /// <param name="sourceProperty"> The source property.</param>
        /// <param name="targetProperty"> The target property.</param>
        /// <param name="sourceParameter">Source parameter expression.</param>
        /// <param name="targetParameter">Target parameter expression.</param>
        /// <returns>The new convert expression.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard does that.")]
        public virtual Expression CreateConvertExpression(
            PropertyInfo sourceProperty,
            PropertyInfo targetProperty,
            ParameterExpression sourceParameter,
            ParameterExpression targetParameter)
        {
            Guard.CheckArgumentNotNull(sourceProperty, "sourceProperty");
            Guard.CheckArgumentNotNull(targetParameter, "targetParameter");
            Guard.CheckArgumentNotNull(sourceParameter, "sourceParameter");
            Guard.CheckArgumentNotNull(targetParameter, "targetParameter");

            PropertyInfo innerProp;
            var checkProp = false;
            string sourceName = sourceProperty.Name;
            string targetName = targetProperty.Name;
            var targetGet = Expression.Property(targetParameter, targetProperty);
            var sourceGet = Expression.Property(sourceParameter, sourceProperty);
            var originalSourceGet = sourceGet;
            if (!this.CanConvert(sourceProperty, targetProperty, out innerProp))
            {
                return null;
            }

            if (innerProp != null)
            {
                sourceGet = Expression.Property(sourceGet, innerProp);
                sourceProperty = innerProp;
                checkProp = true;
            }

            Expression valueGet = sourceGet;

            if (!sourceProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
            {
                var converter = Expression.Constant(CreateValueConverter());

                // TODO: shouldn't I do a GetType or something here?
                var targetType = Expression.Constant(targetProperty.PropertyType);
                var convertMethod = typeof(ValueConverter).GetMethod("Convert");
                var sourceCast = Expression.Convert(sourceGet, typeof(object));
                var convertCall = Expression.Call(converter, convertMethod, sourceCast, targetType);
            }

            Expression assignment = Expression.Assign(targetGet, valueGet);

            if (checkProp)
            {
                var nullComparison = Expression.NotEqual(originalSourceGet, Expression.Constant(null));
                assignment = Expression.IfThen(nullComparison, assignment);
            }

            var excParam = Expression.Parameter(typeof(Exception));
            var excCtr = typeof(MappingException).GetConstructor(new Type[] 
                {
                    typeof(Type),
                    typeof(Type),
                    typeof(string),   
                    typeof(string), 
                    typeof(Exception)
                });
            
            // TODO: add try catch blocks here, to provide the correct property names upon errors.
            return assignment;
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

            // If the mapping is from a nested property, we need to get the sourceValue
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
        /// Determines whether this class can/should make a (strict)
        /// conversion between the provided properties.
        /// </summary>
        /// <param name="sourceProperty">Origin property.</param>
        /// <param name="targetProperty">Destination property.</param>
        /// <param name="finalProperty"><c>out</c> parameter, will contain the final destination
        /// in case of a nested conversion (conversion targetMember a property within
        /// the "targetMember" property).</param>
        /// <returns>
        ///   <c>true</c> if a conversion between the property types is possible
        /// and if a match was found between the property names.
        /// </returns>
        /// <remarks>
        /// This method allows the <see>PropertyConverter</see> class to
        /// give support to nested properties in the destination. In that case,
        /// a conversion from <c>objA.Customer.Name</c> to <c>objB.CustomerName</c> is possible.
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

        /// <summary>
        /// Factory method targetMember create <see>ValueConverter</see> objects.
        /// </summary>
        /// <returns>
        /// A new instance of a <see>ValueConverter</see> object.
        /// </returns>
        protected virtual ValueConverter CreateValueConverter()
        {
            var typeChecker = new ValueConverter();
            return typeChecker;
        }

        #endregion Methods
    }
}