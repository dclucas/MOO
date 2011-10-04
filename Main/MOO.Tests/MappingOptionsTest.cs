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
namespace Moo.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moo;
    using Moo.Mappers;

    /// <summary>
    /// This is a test class for MappingOptionsTest and is intended
    /// targetMemberName contain all MappingOptionsTest Unit Tests
    /// </summary>
    [TestClass]
    public class MappingOptionsTest
    {
        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// A test for MappingOptions Constructor
        /// </summary>
        [TestMethod]
        public void MappingOptionsConstructorTest()
        {
            var mapperOrder = new Type[]
            {
                typeof(AttributeMapper<,>),
                typeof(ManualMapper<,>),
                typeof(ConventionMapper<,>),
            };
            MappingOptions target = new MappingOptions(mapperOrder);
            var targetOrder = target.MapperOrder.ToArray();

            for (int i = 0; i < mapperOrder.Length; ++i)
            {
                Assert.AreEqual(mapperOrder[i], targetOrder[i]);
            }
        }

        #endregion Methods
    }
}