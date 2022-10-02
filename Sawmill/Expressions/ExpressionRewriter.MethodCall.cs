using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(MethodCallExpression m) => (m.Object == null ? 0 : 1) + m.Arguments.Count;

    private static void GetChildren(Span<Expression> children, MethodCallExpression m)
    {
        Span<Expression> rest;
        if (m.Object != null)
        {
            children[0] = m.Object;
            rest = children[1..];
        }
        else
        {
            rest = children;
        }
        Copy(m.Arguments, rest);
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, MethodCallExpression m)
    {
        if (m.Object == null)
        {
            return m.Update(m.Object, newChildren.ToArray());
        }
        return m.Update(newChildren[0], newChildren[1..].ToArray());
    }
}
