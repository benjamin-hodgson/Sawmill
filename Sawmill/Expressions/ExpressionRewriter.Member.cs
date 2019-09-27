using System;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(MemberExpression m) => 1;
        
        private static void GetChildren(Span<Expression> children, MemberExpression m)
        {
            children[0] = m.Expression;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, MemberExpression m)
            => m.Update(newChildren[0]);
    }
}
