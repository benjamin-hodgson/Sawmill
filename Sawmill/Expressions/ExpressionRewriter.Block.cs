using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(BlockExpression b)
        => b.Expressions.Count;
    private static void GetChildren(Span<Expression> children, BlockExpression b)
    {
        Copy(b.Expressions, children);
    }

    private static BlockExpression SetChildren(ReadOnlySpan<Expression> newChildren, BlockExpression b)
        => b.Update(b.Variables, newChildren.ToArray());
}
