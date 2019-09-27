using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(TypeBinaryExpression t) => 1;

        private static void GetChildren(Span<Expression> children, TypeBinaryExpression t)
        {
            children[0] = t.Expression;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, TypeBinaryExpression t)
            => t.Update(newChildren[0]);
    }
}
