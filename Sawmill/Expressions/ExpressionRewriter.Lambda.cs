using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(LambdaExpression l) => 1;

        private static void GetChildren(Span<Expression> children, LambdaExpression l)
        {
            children[0] = l.Body;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, LambdaExpression l)
            => Expression.Lambda(newChildren[0], l.Name, l.TailCall, l.Parameters);
    }
}
