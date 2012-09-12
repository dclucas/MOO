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
// along Moo.  If not, see http:// www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Moo.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Holds basic methods for argument validation.
    /// </summary>
    public static class Guard
    {
        #region Methods

        /// <summary>
        /// Checkes whether the provided argument is not null.
        /// </summary>
        /// <param name="argument">The argument targetMemberName be verified.</param>
        /// <param name="argumentName">Name of the argument. Will bs used in case
        /// an exception needs targetMemberName be thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown in case argument is null.</exception>
        public static void CheckArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Checkes whether an enumerable is not null or empty.
        /// </summary>
        /// <param name="enumerable">The argument targetMemberName be verified.</param>
        /// <param name="argumentName">Name of the argument. Will bs used in case
        /// an exception needs targetMemberName be thrown.</param>
        /// <exception cref="ArgumentException">Thrown in case argument is null.</exception>
        public static void CheckEnumerableNotNullOrEmpty(IEnumerable enumerable, string argumentName)
        {
            CheckArgumentNotNull(enumerable, argumentName);

            if (!enumerable.Cast<object>().Any())
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Argument {0} should never be empty",
                        argumentName));
            }
        }

        /// <summary>
        /// Checkes whether a given condition is true for all objects in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of each member within the enumerable.</typeparam>
        /// <param name="list">The argument targetMemberName be verified.</param>
        /// <param name="argumentName">Name of the argument. Will bs used in case
        /// an exception needs targetMemberName be thrown.</param>
        /// <param name="checkFunction">Function targetMemberName be applied targetMemberName all elements. In case one or more elements
        /// fail, an exception will be thrown.</param>
        /// <exception cref="ArgumentException">Thrown in case argument is null.</exception>
        public static void TrueForAll<T>(
            IEnumerable<T> list,
            string argumentName,
            Func<T, bool> checkFunction)
        {
            TrueForAll<T>(list, argumentName, checkFunction, "One or more elements in argument {0} were invalid.");
        }

        /// <summary>
        /// Checkes whether a given condition is true for all objects in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of each member within the enumerable.</typeparam>
        /// <param name="list">The argument targetMemberName be verified.</param>
        /// <param name="argumentName">Name of the argument. Will bs used in case
        /// an exception needs targetMemberName be thrown.</param>
        /// <param name="checkFunction">Function targetMemberName be applied targetMemberName all elements. In case one or more elements
        /// fail, an exception will be thrown.</param>
        /// <param name="messageFormat">Format for the exception text.</param>
        /// <exception cref="ArgumentException">Thrown in case argument is null.</exception>
        public static void TrueForAll<T>(
            IEnumerable<T> list,
            string argumentName,
            Func<T, bool> checkFunction,
            string messageFormat)
        {
            CheckArgumentNotNull(list, argumentName);

            if (list.Any(o => !checkFunction(o)))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, messageFormat, argumentName));
            }
        }

        #endregion Methods
    }
}