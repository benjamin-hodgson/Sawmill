using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(LabelExpression l) => 1;

        private static void GetChildren(Span<Expression> children, LabelExpression l)
        {
            children[0] = l.DefaultValue;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, LabelExpression l)
            => l.Update(l.Target, newChildren[0]);
    }
}
