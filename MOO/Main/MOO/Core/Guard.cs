//-----------------------------------------------------------------------
// <copyright file="Guard.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
                    String.Format(
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
                    String.Format(CultureInfo.InvariantCulture, messageFormat, argumentName));
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
    }
}
