using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    [SuppressMessage("Style", "IDE0060", Justification = "Used by overload resolution")]
    private static int CountChildren(TryExpression t)
        => 1
            + t.Handlers.Sum(h => h.Filter == null ? 1 : 2)
            + (t.Finally == null ? 0 : 1)
            + (t.Fault == null ? 0 : 1);

    private static void GetChildren(Span<Expression> children, TryExpression t)
    {
        var i = 0;
        children[i] = t.Body;
        i++;

        foreach (var h in t.Handlers)
        {
            if (h.Filter != null)
            {
                children[i] = h.Filter;
                i++;
            }

            children[i] = h.Body;
            i++;
        }

        if (t.Finally != null)
        {
            children[i] = t.Finally;
            i++;
        }

        if (t.Fault != null)
        {
            children[i] = t.Fault;
        }
    }

    private static TryExpression SetChildren(ReadOnlySpan<Expression> newChildren, TryExpression t)
    {
        static CatchBlockUpdateResult UpdateCatchBlocks(IEnumerable<CatchBlock> oldCatchBlocks, ReadOnlySpan<Expression> c)
        {
            var newCatchBlocks = new List<CatchBlock>(oldCatchBlocks.Count());
            foreach (var oldCatchBlock in oldCatchBlocks)
            {
                var (filter, body, i) = oldCatchBlock.Filter == null ? (null, c[0], 1) : (c[0], c[1], 2);
                newCatchBlocks.Add(oldCatchBlock.Update(oldCatchBlock.Variable, filter, body));
                c = c[i..];
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
