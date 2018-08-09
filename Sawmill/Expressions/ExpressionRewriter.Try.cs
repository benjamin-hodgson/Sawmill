using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(TryExpression t)
        {
            var tryExpressionChildren = ImmutableList.CreateBuilder<Expression>();
            tryExpressionChildren.Add(t.Body);
            foreach (var h in t.Handlers)
            {
                tryExpressionChildren.Add(h.Filter);
                tryExpressionChildren.Add(h.Body);
            }
            tryExpressionChildren.Add(t.Finally);
            tryExpressionChildren.Add(t.Fault);
            return Children.Many(tryExpressionChildren.ToImmutable());
        }

        private Expression SetChildren(Children<Expression> newChildren, TryExpression t)
        {
            (IEnumerable<CatchBlock> catchBlocks, IEnumerable<Expression> remainingNewChildren) UpdateCatchBlocks(IEnumerable<CatchBlock> oldCatchBlocks, IEnumerable<Expression> c)
            {
                var newCatchBlocks = new List<CatchBlock>(oldCatchBlocks.Count());
                foreach (var oldCatchBlock in oldCatchBlocks)
                {
                    newCatchBlocks.Add(oldCatchBlock.Update(oldCatchBlock.Variable, c.ElementAt(0), c.ElementAt(1)));
                    c = c.Skip(2);
                }
                return (newCatchBlocks, c);
            }

            var newTryBlock = newChildren.Many[0];
            var updateResult = UpdateCatchBlocks(t.Handlers, newChildren.Many.Skip(1));
            var remainingNewChildren = updateResult.Item2;
            return t.Update(
                newTryBlock,
                updateResult.Item1,
                remainingNewChildren.ElementAt(0),
                remainingNewChildren.ElementAt(1)
            );
        }
    }
}