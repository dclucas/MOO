//-----------------------------------------------------------------------
// <copyright file="ConventionMapper.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
    }
}
