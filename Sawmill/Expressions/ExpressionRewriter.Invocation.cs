using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(InvocationExpression i) => i.Arguments.Count + 1;

        private static void GetChildren(Span<Expression> children, InvocationExpression invocation)
        {
            children[0] = invocation.Expression;
            Copy(invocation.Arguments, children[1..]);
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, InvocationExpression i)
            => i.Update(newChildren[0], newChildren[1..].ToArray());
    }
}
