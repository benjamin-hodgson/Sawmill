using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(UnaryExpression u)
            => Children.One(u.Operand);

        private static Expression SetChildren(Children<Expression> newChildren, UnaryExpression u)
            => u.Update(newChildren.First);
    }
}