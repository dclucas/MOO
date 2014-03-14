using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Core
{
    /// <summary>
    /// Use this attribute to indicate the precedence of a mapper class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MapperPrecedenceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperPrecedenceAttribute"/> class.
        /// </summary>
        /// <param name="mapperPrecedence">The precedence for this mapper.</param>
        public MapperPrecedenceAttribute(int mapperPrecedence)
        {
            this.MapperPrecedence = mapperPrecedence;
        }

        /// <summary>
        /// Gets the mapper precedence.
        /// </summary>
        public int MapperPrecedence { get; private set; }
    }
}
