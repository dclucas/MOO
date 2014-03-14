using Moo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Moo.Initialization
{
    /// <summary>Adds easy mapping initialization for repositores.</summary>
    public static class AdditionalMappingInitializerExtender
    {
        /// <summary>An IMappingRepository extension method that initializes the mappings.</summary>
        public static void InitializeMappings(this IMappingRepository mappingRepo)
        {
            mappingRepo.InitializeMappings(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
