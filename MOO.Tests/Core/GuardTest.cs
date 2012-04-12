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
namespace Moo.Tests.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Moo.Core;

    /// <summary>
    /// This is a test class for GuardTest and is intended
    /// targetMemberName contain all GuardTest Unit Tests
    /// </summary>
    [TestFixture]
    public class GuardTest
    {
        #region Methods (5)

        // Public Methods (5) 

        /// <summary>
        /// A test for CheckArgumentNotNull
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckArgumentNotNullTest()
        {
            Guard.CheckArgumentNotNull(null, "argumentName");
        }

        /// <summary>
        /// A test for CheckEnumerableNotNullOrEmpty
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckEnumerableNotNullOrEmptyEmptyTest()
        {
            Guard.CheckEnumerableNotNullOrEmpty(new object[0], "argumentName");
        }

        /// <summary>
        /// A test for CheckEnumerableNotNullOrEmpty
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckEnumerableNotNullOrEmptyNullTest()
        {
            Guard.CheckEnumerableNotNullOrEmpty(null, "argumentName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TrueForAllNegativeTest()
        {
            Guard.TrueForAll<bool>(
                new bool[] { true, true, true, false },
                "argumentName",
                b => b);
        }

        [Test]
        public void TrueForAllTest()
        {
            var passed = new bool[4];

            Guard.TrueForAll<int>(
                new int[] { 0, 1, 2, 3 },
                "argumentName",
                b => passed[b] = true);

            Assert.IsTrue(passed.All(b => b));
        }

        #endregion Methods
    }
}