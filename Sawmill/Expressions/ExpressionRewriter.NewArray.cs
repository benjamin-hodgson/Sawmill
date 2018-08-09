using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(NewArrayExpression n)
            => Children.Many(n.Expressions.ToImmutableList());

        private static Expression SetChildren(Children<Expression> newChildren, NewArrayExpression n)
            => n.Update(newChildren.Many);
    }
}