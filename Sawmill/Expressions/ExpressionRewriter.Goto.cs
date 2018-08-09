using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(GotoExpression g)
            => Children.One(g.Value);

        private static Expression SetChildren(Children<Expression> newChildren, GotoExpression g)
            => g.Update(g.Target, newChildren.First);
    }
}