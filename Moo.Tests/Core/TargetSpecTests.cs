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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FakeItEasy;
using Moo.Core;
using NUnit.Framework;
using Shouldly;

namespace Moo.Tests.Core
{
    [TestFixture(TypeArgs = new[] {typeof (TestClassA), typeof (TestClassB)})]
    public class TargetSpecTests<TSource, TTarget>
        where TSource : new()
        where TTarget : new()
    {
        [Test]
        public void Constructor_NullArgument_Throws()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            Expression<Func<TSource, object>> fromExpr = s => 1;

            Should.Throw<ArgumentNullException>(() => new TargetSpec<TSource, TTarget, object>(mapper, null));
            Should.Throw<ArgumentNullException>(() => new TargetSpec<TSource, TTarget, object>(null, fromExpr));
        }

        [Test]
        public void To_DefaultCase_DoesNotThrow()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            const string resultstring = "foo";
            Expression<Func<TSource, string>> fromExpr = s => resultstring;
            var target = new TargetSpec<TSource, TTarget, string>(mapper, fromExpr);
            PropertyInfo prop = typeof (TTarget).GetProperties(
                BindingFlags.GetProperty
                | BindingFlags.SetProperty
                | BindingFlags.Instance
                | BindingFlags.Public)
                .First(p => p.PropertyType == typeof (string));
            ParameterExpression propParam = Expression.Parameter(typeof (TTarget));
            MemberExpression propExpr = Expression.Property(propParam, prop);
            UnaryExpression propCast = Expression.Convert(propExpr, typeof (object));
            Expression<Func<TTarget, object>> propLambda = Expression.Lambda<Func<TTarget, object>>(propExpr, propParam);
            MappingAction<TSource, TTarget> resultingAction = null;
            A.CallTo(
                () => mapper.AddMappingAction(
                    A<string>.Ignored,
                    prop.Name,
                    A<MappingAction<TSource, TTarget>>.Ignored))
                .Invokes(a => { resultingAction = (MappingAction<TSource, TTarget>) a.Arguments[2]; });

            ISourceSpec<TSource, TTarget> res = target.To(propLambda);

            var sourceObj = new TSource();
            var targetObj = new TTarget();
            resultingAction(sourceObj, targetObj);
            prop.GetValue(targetObj, null).ShouldBe(resultstring);
        }

        [Test]
        public void To_NotAProperty_Throws()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            Expression<Func<TSource, int>> fromExpr = s => 1;
            var target = new TargetSpec<TSource, TTarget, int>(mapper, fromExpr);

            Should.Throw<ArgumentException>(() => target.To(t => 1));
        }
    }
}