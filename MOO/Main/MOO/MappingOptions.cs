//-----------------------------------------------------------------------
// <copyright file="MappingOptions.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Moo
{
    using System;
    using System.Collections.Generic;
    using Moo.Core;
    using Moo.Mappers;

    /// <summary>
    /// Contains mapping options
    /// </summary>
    public class MappingOptions
    {
        /// <summary>
        /// Backing field containing the internal mappers, in order.
        /// </summary>
        private IEnumerable<Type> mapperOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingOptions"/> class.
        /// </summary>
        public MappingOptions()
        {
            this.MapperOrder = new Type[]
            {
                typeof(ConventionMapper<,>),
                typeof(AttributeMapper<,>),
                typeof(ConfigurationMapper<,>),
                typeof(ManualMapper<,>)
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingOptions"/> class.
        /// </summary>
        /// <param name="mapperOrder">The internal mappers, in order.</param>
        public MappingOptions(IEnumerable<Type> mapperOrder)
        {
            this.MapperOrder = mapperOrder;
        }

        /// <summary>
        /// Gets the list of internal mappers, in order.
        /// </summary>
        public IEnumerable<Type> MapperOrder
        {
            get 
            {
                return this.mapperOrder;
            }

            private set
            {
               Guard.TrueForAll<Type>(
                    value, 
                    "sourceValue", 
                    t => typeof(BaseMapper).IsAssignableFrom(t), 
                    "All types must implement the IMapper interface.");
                this.mapperOrder = value; 
            }
        }
    }
}
