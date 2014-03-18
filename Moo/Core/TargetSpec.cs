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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Moo.Core
{
    /// <summary>
    ///     Base class for fluently setting mapping targets
    /// </summary>
    /// <typeparam name="TSource">Type of the mapping source.</typeparam>
    /// <typeparam name="TTarget">Type of the mapping target.</typeparam>
    /// <typeparam name="TInnerSource">Type of the source property/expression.</typeparam>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1005:AvoidExcessiveParametersOnGenericTypes",
        Justification =
            "I wish I could. No big deal, though, as type inference makes the specifications not necessary in client code."
        )]
    public class TargetSpec<TSource, TTarget, TInnerSource> : ITargetSpec<TSource, TTarget>
    {
        /// <summary>The expression handler to use.</summary>
        private IExpressionHandler _expressionHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TargetSpec{TSource,TTarget, TInnerSource}" /> class.
        /// </summary>
        /// <param name="mapper">Mapper to extend.</param>
        /// <param name="sourceArgument">Expression to pull source data.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done")]
        public TargetSpec(IExtensibleMapper<TSource, TTarget> mapper,
            Expression<Func<TSource, TInnerSource>> sourceArgument)
            : this(mapper, sourceArgument, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TargetSpec{TSource,TTarget, TInnerSource}" />
        ///     class.
        /// </summary>
        /// <param name="mapper">Mapper to extend.</param>
        /// <param name="sourceArgument">Expression to pull source data.</param>
        /// <param name="useInnerMapper">
        ///     Determines whether this mapping should be carried by an internal mapper.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done")]
        internal TargetSpec(
            IExtensibleMapper<TSource, TTarget> mapper,
            Expression<Func<TSource, TInnerSource>> sourceArgument,
            bool useInnerMapper)
        {
            Guard.CheckArgumentNotNull(mapper, "mapper");
            Guard.CheckArgumentNotNull(sourceArgument, "sourceArgument");
            Mapper = mapper;
            SourceArgument = sourceArgument;
            UseInnerMapper = useInnerMapper;
        }

        /// <summary>
        ///     Gets the mapper that to be extended.
        /// </summary>
        protected IExtensibleMapper<TSource, TTarget> Mapper { get; private set; }

        /// <summary>
        ///     Gets the expression that pulls source data.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Easier said than done")]
        protected Expression<Func<TSource, TInnerSource>> SourceArgument { get; private set; }

        /// <summary>Gets the expression handler to use.</summary>
        /// <value>The expression handler to use.</value>
        internal IExpressionHandler ExpressionHandler
        {
            get { return _expressionHandler ?? (_expressionHandler = new ExpressionHandler()); }
        }

        /// <summary>Gets or sets a value indicating whether this mapping should be carried by an inner mapper.</summary>
        /// <value>true if use the mapping should be carried by an inner mapper, false otherwise.</value>
        public bool UseInnerMapper { get; set; }

        /// <summary>Instructs Moo to map a source expression to the property expression below.</summary>
        /// <remarks>
        ///     The argument parameter must be a property access expression, such as <c>(t) =&gt; t.Name</c>,
        ///     or else an ArgumentException will be thrown.
        /// </remarks>
        /// <typeparam name="TInnerTarget">Type of the inner target property.</typeparam>
        /// <param name="argument">An expression fetching the property to map to.</param>
        /// <returns>A spec object allowing further fluent setup.</returns>
        public ISourceSpec<TSource, TTarget> To<TInnerTarget>(Expression<Func<TTarget, TInnerTarget>> argument)
        {
            Guard.CheckArgumentNotNull(argument, "argument");
            Guard.CheckArgumentNotNull(argument.Body, "argument.Body");
            ExpressionHandler.ValidatePropertyExpression(argument);

            if (UseInnerMapper)
            {
                ExpressionHandler.ValidatePropertyExpression(SourceArgument);
                Mapper.AddInnerMapper<TInnerSource, TInnerTarget>(
                    ExpressionHandler.GetProperty(SourceArgument),
                    ExpressionHandler.GetProperty(argument));

                return new SourceSpec<TSource, TTarget>(Mapper);
            }
            Mapper.AddMappingAction(
                ExpressionHandler.GetMemberName(SourceArgument.Body),
                ExpressionHandler.GetMemberName(argument.Body),
                ExpressionHandler.GetAction<TSource, TTarget>(SourceArgument, argument));

            return new SourceSpec<TSource, TTarget>(Mapper);
        }
    }
}