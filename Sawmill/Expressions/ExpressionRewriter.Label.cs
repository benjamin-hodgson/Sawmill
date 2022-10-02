using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(LabelExpression l) => l.DefaultValue == null ? 0 : 1;

    private static void GetChildren(Span<Expression> children, LabelExpression l)
    {
        if (l.DefaultValue != null)
        {
            children[0] = l.DefaultValue;
        }
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, LabelExpression l)
        => l.DefaultValue == null ? l : l.Update(l.Target, newChildren[0]);
}
