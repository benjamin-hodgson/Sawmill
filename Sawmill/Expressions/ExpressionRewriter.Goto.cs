using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(GotoExpression g) => 1;

        private static void GetChildren(Span<Expression> children, GotoExpression g)
        {
            children[0] = g.Value;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, GotoExpression g)
            => g.Update(g.Target, newChildren[0]);
    }
}
