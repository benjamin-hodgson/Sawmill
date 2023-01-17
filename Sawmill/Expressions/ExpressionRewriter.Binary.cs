using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(BinaryExpression b) => 2;

    private static void GetChildren(Span<Expression> children, BinaryExpression b)
    {
        children[0] = b.Left;
        children[1] = b.Right;
    }

    private static BinaryExpression SetChildren(ReadOnlySpan<Expression> newChildren, BinaryExpression b)
        => b.Update(newChildren[0], b.Conversion, newChildren[1]);
}
