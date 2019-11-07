using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(NewExpression n) => n.Arguments.Count;

        private static void GetChildren(Span<Expression> children, NewExpression n)
        {
            Copy(n.Arguments, children);
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, NewExpression n)
            => n.Update(newChildren.ToArray());
    }
}
