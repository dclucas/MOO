//-----------------------------------------------------------------------
// <copyright file="ConfigurationMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo.Mappers
{
    using System.Configuration;
    using System.Linq;
    using Moo.Configuration;
    using Moo.Core;

    /// <summary>
    /// Uses configuration targetType determine mappings between two classes
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ConfigurationMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        public ConfigurationMapper()
            : base()
        {
            this.GenerateMappings();
        }

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", 
            "CA1062:Validate arguments of public methods", 
            MessageId = "0",
            Justification = "The Guard calls do just that.")]
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
            Guard.CheckArgumentNotNull(typeMapping, "typeMapping");
            TypeMappingElement element = GetTypeMapping();
            if (element != null)
            {
                foreach (var propMapping in element.MemberMappings.Cast<MemberMappingElement>())
                {
                    var mappingInfo = new ReflectionPropertyMappingInfo<TSource, TTarget>(
                        typeof(TSource).GetProperty(propMapping.SourceMemberName),
                        typeof(TTarget).GetProperty(propMapping.TargetMemberName),
                        true);

                    typeMapping.Add(mappingInfo);
                }
            }
        }

        /// <summary>
        /// Gets the type mapping configuration element.
        /// </summary>
        /// <returns>
        /// A <see cref="TypeMappingElement"/> instance in case one has been found in the config file,
        /// <c>null</c> otherwise.
        /// </returns>
        internal static TypeMappingElement GetTypeMapping()
        {
            return GetTypeMapping("mooSettings");
        }

        internal static TypeMappingElement GetTypeMapping(string sectionName)
        {
            Guard.CheckArgumentNotNull(sectionName, "sectionName");
            var section = (MappingConfigurationSection)ConfigurationManager.GetSection(sectionName);
            if (section != null)
            {
                return
                    section.TypeMappings.Cast<TypeMappingElement>().FirstOrDefault(
                        t => typeof(TTarget).AssemblyQualifiedName.Contains(t.TargetType)
                             && typeof(TSource).AssemblyQualifiedName.Contains(t.SourceType));
            }
            else
            {
                return null;
            }
        }
    }
}
