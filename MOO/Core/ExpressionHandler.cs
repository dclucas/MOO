namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    internal class ExpressionHandler : IExpressionHandler
    {
        public void ValidatePropertyExpression(LambdaExpression argument)
        {
            if (argument.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException(
                    String.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "'To' should be called with a property getter delegate, but instead got a {0} expression type, with a {1} expression body",
                        argument.NodeType,
                        argument.Body.NodeType));
            }
        }

        public string GetMemberName(Expression expression)
        {
            var memberExpr = expression as System.Linq.Expressions.MemberExpression;
            if (memberExpr != null)
            {
                return memberExpr.Member.Name;
            }

            return expression.ToString();
        }

        public MappingAction<TSource, TTarget> GetAction<TSource, TTarget>
            (LambdaExpression sourceArgument, LambdaExpression targetArgument)
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
            var targetPropName = ((MemberExpression)targetArgument.Body).Member.Name;
            var targetProp = Expression.Property(targetParam, targetPropName);

            // 5:
            var sourceGet = Expression.Convert(Expression.Invoke(sourceArgument, sourceParam), targetProp.Type);

            // 4:
            var assignment = Expression.Assign(targetProp, sourceGet);

            var finalExpr = Expression.Lambda<MappingAction<TSource, TTarget>>(assignment, sourceParam, targetParam);

            return finalExpr.Compile();
        }

        public PropertyInfo GetProperty(LambdaExpression expression)
        {
            Guard.CheckArgumentNotNull(expression, "expression");
            var memberExpression = (expression.Body) as MemberExpression;
            // TODO: either check if it's a property and throw accordingly
            // or also deal with fields.
            return (PropertyInfo)memberExpression.Member;
        }
    }
}
