using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(NewExpression n)
            => Children.Many(n.Arguments.ToImmutableList());

        private static Expression SetChildren(Children<Expression> newChildren, NewExpression n)
            => n.Update(newChildren.Many);
    }
}