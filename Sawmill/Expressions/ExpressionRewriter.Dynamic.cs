using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(DynamicExpression d)
            => Children.Many(d.Arguments.ToImmutableList());

        private static Expression SetChildren(Children<Expression> newChildren, DynamicExpression d)
            => d.Update(newChildren.Many);
    }
}