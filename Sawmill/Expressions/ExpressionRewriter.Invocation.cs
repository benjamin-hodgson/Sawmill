using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(InvocationExpression i)
            => Children.Many(ImmutableList<Expression>.Empty.Add(i.Expression).AddRange(i.Arguments));

        private static Expression SetChildren(Children<Expression> newChildren, InvocationExpression i)
            => i.Update(newChildren.Many[0], newChildren.Many.Skip(1));
    }
}