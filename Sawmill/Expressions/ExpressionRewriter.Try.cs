using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
        private static int CountChildren(TryExpression t) => t.Handlers.Count * 2 + 3;

        private static void GetChildren(Span<Expression> children, TryExpression t)
        {
            var i = 0;
            children[i] = t.Body;
            i++;

            foreach (var h in t.Handlers)
            {
                children[i] = h.Filter;
                i++;
                children[i] = h.Body;
                i++;
            }
            children[i] = t.Finally;
            i++;
            children[i] = t.Fault;
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, TryExpression t)
        {
            static CatchBlockUpdateResult UpdateCatchBlocks(IEnumerable<CatchBlock> oldCatchBlocks, ReadOnlySpan<Expression> c)
            {
                var newCatchBlocks = new List<CatchBlock>(oldCatchBlocks.Count());
                foreach (var oldCatchBlock in oldCatchBlocks)
                {
                    newCatchBlocks.Add(oldCatchBlock.Update(oldCatchBlock.Variable, c[0], c[1]));
                    c = c[2..];
                }
                return new CatchBlockUpdateResult(newCatchBlocks, c);
            }

            var newTryBlock = newChildren[0];
            var updateResult = UpdateCatchBlocks(t.Handlers, newChildren[1..]);
            var remainingNewChildren = updateResult.RemainingNewChildren;
            return t.Update(
                newTryBlock,
                updateResult.CatchBlocks,
                remainingNewChildren[0],
                remainingNewChildren[1]
            );
        }

        private readonly ref struct CatchBlockUpdateResult
        {
            public IEnumerable<CatchBlock> CatchBlocks { get; }
            public ReadOnlySpan<Expression> RemainingNewChildren { get; }

            public CatchBlockUpdateResult(IEnumerable<CatchBlock> catchBlocks, ReadOnlySpan<Expression> remainingNewChildren)
            {
                CatchBlocks = catchBlocks;
                RemainingNewChildren = remainingNewChildren;
            }
        }
    }
}
