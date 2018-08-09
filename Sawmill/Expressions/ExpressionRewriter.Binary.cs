using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(BinaryExpression b)
            => Children.Two(b.Left, b.Right);

        private static BinaryExpression SetChildren(Children<Expression> newChildren, BinaryExpression b)
            => b.Update(newChildren.First, b.Conversion, newChildren.Second);
    }
}