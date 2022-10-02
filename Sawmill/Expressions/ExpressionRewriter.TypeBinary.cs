using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(TypeBinaryExpression t) => 1;

    private static void GetChildren(Span<Expression> children, TypeBinaryExpression t)
    {
        children[0] = t.Expression;
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, TypeBinaryExpression t)
        => t.Update(newChildren[0]);
}
