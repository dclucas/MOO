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
namespace Moo.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>Interface for expression handler.</summary>
    internal interface IExpressionHandler
    {
        /// <summary>Gets an action action from two expressions.</summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="sourceArgument">An expression fetching the source data.</param>
        /// <param name="targetArgument">An expression fetching the target property.</param>
        /// <returns>A delegate that maps from the source to the target object</returns>
        /// <exception cref="ArgumentException">targetExpression is not a property access expression.</exception>
        MappingAction<TSource, TTarget> GetAction<TSource, TTarget>(LambdaExpression sourceArgument, LambdaExpression targetArgument);
        
        /// <summary>Gets internal name to be used for an expression.</summary>
        /// <param name="expression">The expression to be named.</param>
        /// <returns>A name for the expression.</returns>
        /// <remarks>
        /// This method will return the property name for a property access expression and the 
        /// expression's ToString result other wise.
        /// </remarks>
        string GetMemberName(Expression expression);
        
        /// <summary>Extracts a property from within an expression.</summary>
        /// <param name="expression">The expression to be checked.</param>
        /// <returns>The expression's internal property.</returns>
        /// <exception cref="ArgumentException">The expression is not for property access.</exception>
        PropertyInfo GetProperty(LambdaExpression expression);
        
        /// <summary>Checks whether the expression represent a property access.</summary>
        /// <param name="argument">The argument to be validated.</param>
        /// <exception cref="ArgumentException">The expression is not for property access.</exception>
        void ValidatePropertyExpression(LambdaExpression argument);
    }
}
