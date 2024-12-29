using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(NewArrayExpression n) => n.Expressions.Count;

    private static void GetChildren(Span<Expression> children, NewArrayExpression n)
    {
        Copy(n.Expressions, children);
    }

    private static NewArrayExpression SetChildren(ReadOnlySpan<Expression> newChildren, NewArrayExpression n)
        => n.Update(newChildren.ToArray());
}
