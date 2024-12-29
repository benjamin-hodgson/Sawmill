using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(LoopExpression l) => 1;

    private static void GetChildren(Span<Expression> children, LoopExpression l)
    {
        children[0] = l.Body;
    }

    private static LoopExpression SetChildren(ReadOnlySpan<Expression> newChildren, LoopExpression l)
        => l.Update(l.BreakLabel, l.ContinueLabel, newChildren[0]);
}
