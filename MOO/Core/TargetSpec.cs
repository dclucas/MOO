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
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Base class for fluently setting mapping targets
    /// </summary>
    /// <typeparam name="TSource">Type of the mapping source.</typeparam>
    /// <typeparam name="TTarget">Type of the mapping target.</typeparam>
    public class TargetSpec<TSource, TTarget> : ITargetSpec<TSource, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetSpec{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="mapper">Mapper to extend.</param>
        /// <param name="sourceArgument">Expression to pull source data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Easier said than done")]
        public TargetSpec(IExtensibleMapper<TSource, TTarget> mapper, Expression<Func<TSource, object>> sourceArgument)
        {
            // TODO: Complete member initialization
            this.Mapper = mapper;
            this.SourceArgument = sourceArgument;
        }

        /// <summary>
        /// Gets the mapper that to be extended.
        /// </summary>
        protected IExtensibleMapper<TSource, TTarget> Mapper { get; private set; }

        /// <summary>
        /// Gets the expression that pulls source data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Easier said than done")]
        protected Expression<Func<TSource, object>> SourceArgument { get; private set; }

        /// <summary>
        /// Instructs Moo to map a source expression to the property expression below.
        /// </summary>
        /// <param name="argument">An expression fetching the property to map to.</param>
        /// <remarks>
        /// The argument parameter must be a property access expression, such as <c>(t) => t.Name</c>,
        /// or else an ArgumentException will be thrown.
        /// </remarks>
        public void To(Expression<Func<TTarget, object>> argument)
        {
            Guard.CheckArgumentNotNull(argument, "argument");
            Guard.CheckArgumentNotNull(argument.Body, "argument.Body");
            if (argument.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("'To' should be called with a property getter delegate");
            }

            Mapper.AddMappingAction(
                GetMemberName(SourceArgument.Body),
                GetMemberName(argument.Body),
                GetAction(SourceArgument, argument));
        }

        /// <summary>
        /// Combines the two mapping expressions (one to get the target, the other
        /// to produce data from the source) into a mapping action.
        /// </summary>
        /// <param name="sourceExpr">Expression to pull source data.</param>
        /// <param name="targetExpr">Expression to determine target property.</param>
        /// <returns>A mapping action, mapping from the source to the target expression.</returns>
        private static MappingAction<TSource, TTarget> GetAction(
            Expression<Func<TSource, object>> sourceExpr,
            Expression<Func<TTarget, object>> targetExpr)
        {
            // Decomposition of (a, b) => b.FooBar = func(a) :
            // 1. (a, 
            // 2. b) => 
            // 3. b.FooBar 
            // 4. =
            // 5. func(a) -- we still need the method call

            // 1:
            var sourceParam = Expression.Parameter(typeof(TSource));
            
            // 2:
            var targetParam = Expression.Parameter(typeof(TTarget));
            
            // 3:
            var targetPropName = ((MemberExpression)targetExpr.Body).Member.Name;
            var targetProp = Expression.Property(targetParam, targetPropName);
            
            // 5:
            var sourceGet = Expression.Convert(Expression.Invoke(sourceExpr, sourceParam), targetProp.Type);
            
            // 4:
            var assignment = Expression.Assign(targetProp, sourceGet);

            var finalExpr = Expression.Lambda<MappingAction<TSource, TTarget>>(assignment, sourceParam, targetParam);

            return finalExpr.Compile();
        }

        /// <summary>
        /// Gets the mapping key to a given expression.
        /// </summary>
        /// <param name="argument">Expression to extract the key.</param>
        /// <returns>A string containing the key to use in the mapping table.</returns>
        /// <remarks>
        /// In case of property accessors this will return the property name (important,
        /// as we want this to allow mapping overwrites for the same target property), in
        /// other cases, merely the expression.ToSting.
        /// </remarks>
        private static string GetMemberName(Expression argument)
        {
            var memberExpr = argument as System.Linq.Expressions.MemberExpression;
            if (memberExpr != null)
            {
                return memberExpr.Member.Name;
            }

            return argument.ToString();
        }
    }
}
