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

namespace Moo.Mappers
{
    using System.Reflection;
    using Moo.Core;

    /// <summary>
    /// Uses naming and type conversion convention targetType create mappings between
    /// two classes.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ConventionMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification = "For the time being, this is the desired behavior.")]
        public ConventionMapper()
        {
            this.GenerateMappings();
        }

        #endregion Constructors

        #region Methods (1)

        // Protected Methods (1) 

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The Guard call does that.")]
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
            Guard.CheckArgumentNotNull(typeMapping, "typeMapping");
            var checker = GetPropertyConverter();
            foreach (var fromProp in typeof(TSource).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.GetProperty))
            {
                foreach (var toProp in typeof(TTarget).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.SetProperty))
                {
                    string finalName;
                    if (checker.CanConvert(fromProp, toProp, out finalName))
                    {
                        var mappingInfo = new ReflectionPropertyMappingInfo<TSource, TTarget>(
                            fromProp,
                            toProp,
                            false);

                        typeMapping.Add(mappingInfo);
                    }
                }
            }
        }

        #endregion Methods
    }
}