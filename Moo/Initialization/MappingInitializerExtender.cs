using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moo.Core;

namespace Moo.Initialization
{
    /// <summary>Adds easy mapping initialization for repositores.</summary>
    public static class MappingInitializerExtender
    {
        /// <summary>An IMappingRepository extension method that initializes the mappings.</summary>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="initializers" /> parameter is null.
        /// </exception>
        /// <param name="mappingRepo"> The mappingRepo to act on.</param>
        /// <param name="initializers">The initializers.</param>
        public static void InitializeMappings(
            this IMappingRepository mappingRepo,
            IEnumerable<IMappingInitializer> initializers)
        {
            Guard.CheckArgumentNotNull(initializers, "initializers");
            foreach (IMappingInitializer i in initializers)
            {
                i.InitializeMappings(mappingRepo);
            }
        }

        /// <summary>An IMappingRepository extension method that initializes the mappings.</summary>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="assemblies" /> parameter is null.
        /// </exception>
        /// <param name="mappingRepo"> The mappingRepo to act on.</param>
        /// <param name="assemblies">The initializers.</param>
        public static void InitializeMappings(
            this IMappingRepository mappingRepo,
            IEnumerable<Assembly> assemblies)
        {
            Guard.CheckArgumentNotNull(assemblies, "assemblies");

            IEnumerable<IMappingInitializer> q = from assm in assemblies
                from t in assm.GetTypes()
                where !t.IsAbstract
                where typeof (IMappingInitializer).IsAssignableFrom(t)
                select (IMappingInitializer) Activator.CreateInstance(t);

            InitializeMappings(mappingRepo, q);
        }
    }
}