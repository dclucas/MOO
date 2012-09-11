namespace Moo.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal interface IExpressionHandler
    {
        MappingAction<TSource, TTarget> GetAction<TSource, TTarget>(LambdaExpression sourceArgument, LambdaExpression targetArgument);
        
        string GetMemberName(Expression expression);
        
        PropertyInfo GetProperty(LambdaExpression expression);
        
        void ValidatePropertyExpression(LambdaExpression argument);
    }
}
