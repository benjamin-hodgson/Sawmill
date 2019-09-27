using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(LoopExpression l) => 1;

        private static void GetChildren(Span<Expression> children, LoopExpression l)
        {
            children[0] = l.Body;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, LoopExpression l)
            => l.Update(l.BreakLabel, l.ContinueLabel, newChildren[0]);
    }
}