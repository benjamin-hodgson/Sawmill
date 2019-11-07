using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(BinaryExpression b) => 2;
        private static void GetChildren(Span<Expression> children, BinaryExpression b)
        {
            children[0] = b.Left;
            children[1] = b.Right;
        }

        private static BinaryExpression SetChildren(ReadOnlySpan<Expression> newChildren, BinaryExpression b)
            => b.Update(newChildren[0], b.Conversion, newChildren[1]);
    }
}
