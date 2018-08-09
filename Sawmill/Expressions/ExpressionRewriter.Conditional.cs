using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(ConditionalExpression c)
            => Children.Many(ImmutableList<Expression>.Empty.Add(c.Test).Add(c.IfTrue).Add(c.IfFalse));

        private static ConditionalExpression SetChildren(Children<Expression> newChildren, ConditionalExpression c)
            => c.Update(newChildren.Many[0], newChildren.Many[1], newChildren.Many[2]);
    }
}