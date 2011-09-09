//-----------------------------------------------------------------------
// <copyright file="TypeMappingCollection.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
