using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(DynamicExpression d) => d.Arguments.Count;

    private static void GetChildren(Span<Expression> children, DynamicExpression d)
    {
        Copy(d.Arguments, children);
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, DynamicExpression d)
        => d.Update(newChildren.ToArray());
}
