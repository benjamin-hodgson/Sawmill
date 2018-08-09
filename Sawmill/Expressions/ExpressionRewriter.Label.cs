using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(LabelExpression l)
            => Children.One(l.DefaultValue);

        private static Expression SetChildren(Children<Expression> newChildren, LabelExpression l)
            => l.Update(l.Target, newChildren.First);
    }
}