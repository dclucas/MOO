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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FakeItEasy;
    using Moo.Core;
    using Moo.Tests.Integration.MappedClasses.DomainModels;
    using Moo.Tests.Integration.MappedClasses.ViewModels;
    using Moo.Tests.Utils;
    using NUnit.Framework;
    using Shouldly;
    using System.Linq.Expressions;
    using System.Reflection;

    [TestFixture(TypeArgs = new Type[] { typeof(Person), typeof(PersonIndexModel) })]
    public class TargetSpecTest<TSource, TTarget> 
        where TSource : new()
        where TTarget : new()
    {
        [Test]
        public void Constructor_NullArgument_Throws()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            Expression<Func<TSource, object>> fromExpr = (s) => 1;

            Should.Throw<ArgumentNullException>(() => new TargetSpec<TSource, TTarget>(mapper, null));
            Should.Throw<ArgumentNullException>(() => new TargetSpec<TSource, TTarget>(null, fromExpr));
        }

        [Test]
        public void To_NotAProperty_Throws()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            Expression<Func<TSource, object>> fromExpr = (s) => 1;
            var target = new TargetSpec<TSource, TTarget>(mapper, fromExpr);

            Should.Throw<ArgumentException>(() => target.To(t => 1));
        }

        [Test]
        public void To_DefaultCase_DoesNotThrow()
        {
            var mapper = A.Fake<IExtensibleMapper<TSource, TTarget>>();
            var resultString = "foo";
            Expression<Func<TSource, object>> fromExpr = (s) => resultString;
            var target = new TargetSpec<TSource, TTarget>(mapper, fromExpr);
            var prop = typeof(TTarget).GetProperties(
                BindingFlags.GetProperty 
                | BindingFlags.SetProperty 
                | BindingFlags.Instance 
                | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(String))
                .First();
            var propParam = Expression.Parameter(typeof(TTarget));
            var propExpr = Expression.Property(propParam, prop);
            var propCast = Expression.Convert(propExpr, typeof(object));
            var propLambda = Expression.Lambda<Func<TTarget, object>>(propExpr, propParam);
            MappingAction<TSource, TTarget> resultingAction = null;
            A.CallTo(
                () => mapper.AddMappingAction(
                    A<string>.Ignored,
                    prop.Name,
                    A<MappingAction<TSource, TTarget>>.Ignored))
                .Invokes((a) =>
                    {
                        resultingAction = (MappingAction<TSource, TTarget>)a.Arguments[2];
                    });

            var res = target.To(propLambda);
            
            var sourceObj = new TSource();
            var targetObj = new TTarget();
            resultingAction(sourceObj, targetObj);
            prop.GetValue(targetObj, null).ShouldBe(resultString);
        }
    }
}
