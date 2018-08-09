using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(LambdaExpression l)
            => Children.One(l.Body);

        private static Expression SetChildren(Children<Expression> newChildren, LambdaExpression l)
            => Expression.Lambda(newChildren.First, l.Name, l.TailCall, l.Parameters);
    }
}