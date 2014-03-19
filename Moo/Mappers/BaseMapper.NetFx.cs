using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.Mappers
{
    public abstract partial class BaseMapper<TSource, TTarget> : BaseMapper, IMapper<TSource, TTarget>
    {
        /// <summary>Maps the specified source to a target object asynchronously.</summary>
        /// <remarks>
        ///     This method relies on the <see cref="System.Activator.CreateInstance&lt;T&gt;" />
        ///     method to create target objects. This means that both there are more efficient methods for
        ///     that and that this limits the use of this overload to target classes that this framework
        ///     method is able to construct.
        /// </remarks>
        /// <param name="source">The source object.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public async Task<TTarget> MapAsync(TSource source)
        {
            return await Task.Run(() => Map(source));
        }

        /// <summary>Maps the specified source to a target object asynchronously.</summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public async Task<TTarget> MapAsync(TSource source, TTarget target)
        {
            return await Task.Run((() => Map(source, target)));
        }

        /// <summary>Maps the specified source to a target object asynchronously.</summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public async Task<TTarget> MapAsync(TSource source, Func<TTarget> createTarget)
        {
            return await Task.Run((() => Map(source, createTarget)));
        }         
    }
}
