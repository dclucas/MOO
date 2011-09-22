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

namespace Moo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Contains a collection  of type mappings.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1010:CollectionsShouldImplementGenericInterface",
        Justification = "The base class does not implement it. And there's no need for the generic interface implementation (YAGNI).")]
    public class TypeMappingCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The type mapping element.</param>
        public void Add(TypeMappingElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> targetType return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var mapping = (TypeMappingElement)element;
            return mapping.SourceType + ">" + mapping.TargetType;
        }

        /// <summary>
        /// Creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeMappingElement();
        }
    }
}