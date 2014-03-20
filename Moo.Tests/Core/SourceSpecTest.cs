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
using Moo.Core;
using Moo.Tests.Utils;
using NUnit.Framework;
using Shouldly;

namespace Moo.Tests.Core
{
    [TestFixture(TypeArgs = new[] {typeof (TestClassA), typeof (TestClassB)})]
    public class SourceSpecTest<TSource, TTarget>
    {
        [Test]
        public void From_DefaultCase_ReturnsCorrectly()
        {
            var target = TestFactory.CreateTarget<SourceSpec<TSource, TTarget>>();

            ITargetSpec<TSource, TTarget> from = target.From(s => 1);

            from.ShouldNotBe(null);
        }

        [Test]
        public void From_NullMapper_Throws()
        {
            Should.Throw<ArgumentNullException>(() => new SourceSpec<TSource, TTarget>(null));
        }
    }
}