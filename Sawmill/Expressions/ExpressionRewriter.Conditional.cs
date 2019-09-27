using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(ConditionalExpression c) => 3;

        private static void GetChildren(Span<Expression> children, ConditionalExpression c)
        {
            children[0] = c.Test;
            children[1] = c.IfTrue;
            children[2] = c.IfFalse;
        }

        private static ConditionalExpression SetChildren(ReadOnlySpan<Expression> newChildren, ConditionalExpression c)
            => c.Update(newChildren[0], newChildren[1], newChildren[2]);
    }
}
