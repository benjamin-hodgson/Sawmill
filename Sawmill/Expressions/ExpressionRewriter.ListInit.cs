using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static int CountChildren(ListInitExpression l) => l.Initializers.Select(i => i.Arguments.Count).Sum();

        private static void GetChildren(Span<Expression> children, ListInitExpression l)
        {
            Copy(l.Initializers.SelectMany(i => i.Arguments), children);
        }

        private Expression SetChildren(ReadOnlySpan<Expression> newChildren, ListInitExpression l)
        {
            IEnumerable<ElementInit> UpdateElementInits(ReadOnlyCollection<ElementInit> oldInits, ReadOnlySpan<Expression> newArguments)
            {
                var newInits = new List<ElementInit>(oldInits.Count());
                foreach (var oldInit in oldInits)
                {
                    var argCount = oldInit.Arguments.Count;
                    newInits.Add(oldInit.Update(newArguments.Slice(0, argCount).ToArray()));
                    newArguments = newArguments.Slice(argCount);
                }
                return newInits;
            }
            
            return l.Update(l.NewExpression, UpdateElementInits(l.Initializers, newChildren));
        }
    }
}
