using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(UnaryExpression u) => 1;

        private static void GetChildren(Span<Expression> children, UnaryExpression u)
        {
            children[0] = u.Operand;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, UnaryExpression u)
            => u.Update(newChildren[0]);
    }
}
