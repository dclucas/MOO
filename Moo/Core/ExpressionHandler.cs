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

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Moo.Core
{
    /// <summary>Provides LINQ expression handling capabilities.</summary>
    internal class ExpressionHandler : IExpressionHandler
    {
        /// <summary>Checks whether the expression represent a property access.</summary>
        /// <exception cref="ArgumentException">The expression is not for property access.</exception>
        /// <param name="argument">The argument to be validated.</param>
        public void ValidatePropertyExpression(LambdaExpression argument)
        {
            if (argument.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "'To' should be called with a property getter delegate, but instead got a {0} expression type, with a {1} expression body",
                        argument.NodeType,
                        argument.Body.NodeType));
            }
        }

        /// <summary>Gets internal name to be used for an expression.</summary>
        /// <param name="expression">The expression to be named.</param>
        /// <returns>A name for the expression.</returns>
        public string GetMemberName(Expression expression)
        {
            var memberExpr = expression as MemberExpression;
            if (memberExpr != null)
            {
                return memberExpr.Member.Name;
            }

            return expression.ToString();
        }

        /// <summary>Gets an action action from two expressions.</summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="sourceArgument">An expression fetching the source data.</param>
        /// <param name="targetArgument">An expression fetching the target property.</param>
        /// <returns>A delegate that maps from the source to the target object.</returns>
        /// ###
        /// <exception cref="ArgumentException">
        ///     targetExpression is not a property access expression.
        /// </exception>
        public MappingAction<TSource, TTarget> GetAction<TSource, TTarget>(
            LambdaExpression sourceArgument,
            LambdaExpression targetArgument)
        {
            ParameterExpression sourceParam = Expression.Parameter(typeof (TSource));
            ParameterExpression targetParam = Expression.Parameter(typeof (TTarget));
            string targetPropName = ((MemberExpression) targetArgument.Body).Member.Name;
            MemberExpression targetProp = Expression.Property(targetParam, targetPropName);
            UnaryExpression sourceGet = Expression.Convert(Expression.Invoke(sourceArgument, sourceParam),
                targetProp.Type);
            BinaryExpression assignment = Expression.Assign(targetProp, sourceGet);
            Expression<MappingAction<TSource, TTarget>> finalExpr =
                Expression.Lambda<MappingAction<TSource, TTarget>>(assignment, sourceParam, targetParam);
            return finalExpr.Compile();
        }

        /// <summary>Extracts a property from within an expression.</summary>
        /// <param name="expression">The expression to be checked.</param>
        /// <returns>The expression's internal property.</returns>
        /// ###
        /// <exception cref="ArgumentException">
        ///     The expression is not for property access.
        /// </exception>
        public PropertyInfo GetProperty(LambdaExpression expression)
        {
            Guard.CheckArgumentNotNull(expression, "expression");
            Guard.CheckArgumentNotNull(expression.Body, "expression.Body");
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must contain a body.");
            }

            var propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new InvalidOperationException("Mappers can only handle property expressions.");
            }
            return propInfo;
        }
    }
}