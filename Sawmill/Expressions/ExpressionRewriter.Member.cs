using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
        private static int CountChildren(MemberExpression m) => 1;

        private static void GetChildren(Span<Expression> children, MemberExpression m)
        {
            children[0] = m.Expression!;  // it's declared as nullable but the declaration is wrong, I think
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, MemberExpression m)
            => m.Update(newChildren[0]);
    }
}
