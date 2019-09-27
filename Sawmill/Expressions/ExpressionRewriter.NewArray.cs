using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(NewArrayExpression n) => n.Expressions.Count;

        private static void GetChildren(Span<Expression> children, NewArrayExpression n)
        {
            Copy(n.Expressions, children);
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, NewArrayExpression n)
            => n.Update(newChildren.ToArray());
    }
}
