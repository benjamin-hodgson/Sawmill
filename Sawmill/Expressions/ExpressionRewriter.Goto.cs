using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(GotoExpression g) => g.Value == null ? 0 : 1;

    private static void GetChildren(Span<Expression> children, GotoExpression g)
    {
        if (g.Value != null)
        {
            children[0] = g.Value;
        }
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, GotoExpression g)
        => g.Update(g.Target, g.Value == null ? null : newChildren[0]);
}
