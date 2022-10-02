using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
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
