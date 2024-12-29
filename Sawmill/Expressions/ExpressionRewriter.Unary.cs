using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(UnaryExpression u) => 1;

    private static void GetChildren(Span<Expression> children, UnaryExpression u)
    {
        children[0] = u.Operand;
    }

    private static UnaryExpression SetChildren(ReadOnlySpan<Expression> newChildren, UnaryExpression u)
        => u.Update(newChildren[0]);
}
