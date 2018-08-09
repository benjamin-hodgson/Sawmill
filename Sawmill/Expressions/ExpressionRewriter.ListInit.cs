using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private static Children<Expression> GetChildren(ListInitExpression l)
            => Children.Many(l.Initializers.SelectMany(i => i.Arguments).ToImmutableList());

        private Expression SetChildren(Children<Expression> newChildren, ListInitExpression l)
        {
            IEnumerable<ElementInit> UpdateElementInits(IEnumerable<ElementInit> oldInits, IEnumerable<Expression> newArguments)
            {
                var newInits = new List<ElementInit>(oldInits.Count());
                foreach (var oldInit in oldInits)
                {
                    var argCount = oldInit.Arguments.Count;
                    newInits.Add(oldInit.Update(newArguments.Take(argCount)));
                    newArguments = newArguments.Skip(argCount);
                }
                return newInits;
            }
            
            return l.Update(l.NewExpression, UpdateElementInits(l.Initializers, newChildren.Many));
        }
    }
}