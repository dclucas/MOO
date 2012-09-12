namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides property searching features.
    /// </summary>
    public class PropertyExplorer : IPropertyExplorer
    {
        /// <summary>Gets all properties for a given source type.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        public IEnumerable<PropertyInfo> GetSourceProps<TSource>()
        {
            return typeof(TSource).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.GetProperty);
        }

        /// <summary>Gets all properties for a given target type.</summary>
        /// <typeparam name="TTarget">Type of the source.</typeparam>
        /// <returns>Iterates all valid properties in the source type.</returns>
        public IEnumerable<PropertyInfo> GetTargetProps<TTarget>()
        {
            return typeof(TTarget).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.SetProperty);
        }

        /// <summary>Enumerates get matches in this collection.</summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TTarget">Type of the target.</typeparam>
        /// <param name="checkAction">
        /// A check function, that receives a source and a target property and determines if they match.
        /// </param>
        /// <returns>A list of all matches between the source and target types.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It's either that or using an array.")]
        public IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetMatches<TSource, TTarget>(Func<PropertyInfo, PropertyInfo, bool> checkAction)
        {
            return from s in GetSourceProps<TSource>()
                   from t in GetTargetProps<TTarget>()
                   where checkAction(s, t)
                   select new KeyValuePair<PropertyInfo, PropertyInfo>(s, t);
        }
    }
}
