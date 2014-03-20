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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;

namespace Moo.Tests.Utils
{
    /// <summary>
    ///     Internal factory class, for target instance construction.
    /// </summary>
    internal static class TestFactory
    {
        public static T CreateTarget<T>()
        {
            ConstructorInfo ctorInfo =
                typeof (T).GetConstructors().OrderByDescending(c => c.GetParameters().Count()).First();
            MethodInfo fakeFactory = typeof (A).GetMethod("Fake", Type.EmptyTypes);

            IEnumerable<object> ctorParams = from p in ctorInfo.GetParameters()
                select fakeFactory.MakeGenericMethod(p.ParameterType).Invoke(null, null);
            object result = ctorInfo.Invoke(ctorParams.ToArray());

            return (T) result;
        }
    }
}