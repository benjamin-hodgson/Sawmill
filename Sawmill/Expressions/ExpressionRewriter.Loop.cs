using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(LoopExpression l)
            => Children.One(l.Body);

        private static Expression SetChildren(Children<Expression> newChildren, LoopExpression l)
            => l.Update(l.BreakLabel, l.ContinueLabel, newChildren.First);
    }
}