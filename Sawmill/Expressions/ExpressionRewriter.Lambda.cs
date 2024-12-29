using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(LambdaExpression l) => 1;

    private static void GetChildren(Span<Expression> children, LambdaExpression l)
    {
        children[0] = l.Body;
    }

    private static LambdaExpression SetChildren(ReadOnlySpan<Expression> newChildren, LambdaExpression l)
        => Expression.Lambda(newChildren[0], l.Name, l.TailCall, l.Parameters);
}
