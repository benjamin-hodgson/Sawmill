using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(IndexExpression i) => i.Arguments.Count + 1;

    private static void GetChildren(Span<Expression> children, IndexExpression index)
    {
        children[0] = index.Object!;  // it's declared as nullable but the declaration is wrong, I think
        Copy(index.Arguments, children[1..]);
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, IndexExpression i)
        => i.Update(newChildren[0], newChildren[1..].ToArray());
}
