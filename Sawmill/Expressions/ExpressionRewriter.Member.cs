using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(MemberExpression m)
            => Children.One(m.Expression);

        private static Expression SetChildren(Children<Expression> newChildren, MemberExpression m)
            => m.Update(newChildren.First);
    }
}