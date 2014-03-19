// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moo.Extenders;

namespace Moo
{
    /// <summary>
    ///     Extends all objects, providing async mapping functionalities.
    /// </summary>
    public static class AsyncExtender
    {
        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source)
        {
            return await Task.Run(() => source.MapTo<TTarget>());
        }

        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="mapper">The mapper to be used.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source, IMapper mapper)
        {
            return await Task.Run(() => source.MapTo<TTarget>(mapper));
        }

        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="repo">The repository that will provide the mapper.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source, IMappingRepository repo)
        {
            return await Task.Run(() => source.MapTo<TTarget>(repo));
        }

        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source, TTarget target)
        {
            return await Task.Run(() => source.MapTo(target));
        }

        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <param name="mapper">The mapper to be used.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source, TTarget target, IMapper mapper)
        {
            return await Task.Run(() => source.MapTo(target, mapper));
        }

        /// <summary>
        ///     Asynchronously maps the values on object the to a target type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <param name="repo">The repository that will provide the mapper.</param>
        /// <returns>
        ///     The object representing the asynchronous operation. The task result will contain a
        ///     filled target object.
        /// </returns>
        public static async Task<TTarget> MapToAsync<TTarget>(this object source, TTarget target, IMappingRepository repo)
        {
            return await Task.Run(() => source.MapTo(target, repo));
        }
    }
}
