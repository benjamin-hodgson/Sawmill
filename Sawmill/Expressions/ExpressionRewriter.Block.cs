using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(BlockExpression b)
            => Children.Many(b.Expressions.ToImmutableList());

        private static BlockExpression SetChildren(Children<Expression> newChildren, BlockExpression b)
            => b.Update(b.Variables, newChildren.Many);
    }
}