namespace Moo
{
    using Moo.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Extends the IExtensibleMapper interface with fluent methods.
    /// </summary>
    public static class IExtensibleMapperExtender
    {
        /// <summary>
        /// Adds a fluent AddMapping method to IExtensibleMapperExtender
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="mapper">Mapper to extend.</param>
        /// <returns>A ISourceSpec object, for fluent mapping.</returns>
        public static SourceSpec<TSource, TTarget> AddMapping<TSource, TTarget>(this IExtensibleMapper<TSource, TTarget> mapper)
        {
            return new SourceSpec<TSource, TTarget>(mapper);
        }
    }

    public class SourceSpec<TSource, TTarget>
    {
        protected IExtensibleMapper<TSource, TTarget> Mapper { get; private set; }

        public SourceSpec(IExtensibleMapper<TSource, TTarget> mapper)
        {
            this.Mapper = mapper;
        }

        public TargetSpec<TSource, TTarget> From(Expression<Func<TSource, object>> argument)
        {
            return new TargetSpec<TSource, TTarget>(Mapper, argument);
        }
    }

    public class TargetSpec<TSource, TTarget>
    {
        protected IExtensibleMapper<TSource, TTarget> Mapper { get; private set; }

        protected Expression<Func<TSource, object>> SourceArgument { get; private set; }

        public TargetSpec(IExtensibleMapper<TSource, TTarget> mapper, Expression<Func<TSource, object>> sourceArgument)
        {
            // TODO: Complete member initialization
            this.Mapper = mapper;
            this.SourceArgument = sourceArgument;
        }

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

        private MappingAction<TSource, TTarget> GetAction(
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
            //var sourceGet = Expression.Invoke(sourceExpr, sourceParam);

            // 4:
            var assignment = Expression.Assign(targetProp, sourceGet);

            var finalExpr = Expression.Lambda<MappingAction<TSource, TTarget>>(assignment, sourceParam, targetParam);

            return finalExpr.Compile();
        }
 

        private string GetMemberName(Expression argument)
        {
            var memberExpr = (argument) as System.Linq.Expressions.MemberExpression;
            if (memberExpr != null)
            {
                return memberExpr.Member.Name;
            }

            return argument.ToString();
        }
    }

}
