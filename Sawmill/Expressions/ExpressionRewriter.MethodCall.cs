using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(MethodCallExpression m)
        {
            if (m.Object == null)
            {
                return m.Arguments.ToImmutableList();
            }
            return ImmutableList<Expression>.Empty.Add(m.Object).AddRange(m.Arguments);
        }
            
        private static Expression SetChildren(Children<Expression> newChildren, MethodCallExpression m)
        {
            if (m.Object == null)
            {
                return m.Update(m.Object, newChildren.Many.Skip(1));
            }
            return m.Update(newChildren.Many[0], newChildren.Many.Skip(1));
        }
    }
}