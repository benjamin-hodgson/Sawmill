using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(IndexExpression i) => i.Arguments.Count + 1;

        private static void GetChildren(Span<Expression> children, IndexExpression index)
        {
            children[0] = index.Object;
            Copy(index.Arguments, children.Slice(1));
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, IndexExpression i)
            => i.Update(newChildren[0], newChildren.Slice(1).ToArray());
    }
}
