using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(TypeBinaryExpression t)
            => Children.One(t.Expression);

        private static Expression SetChildren(Children<Expression> newChildren, TypeBinaryExpression t)
            => t.Update(newChildren.First);
    }
}