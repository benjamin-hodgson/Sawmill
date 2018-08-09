using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private Children<Expression> GetChildren(SwitchExpression s)
        {
            IEnumerable<Expression> GetSwitchCaseChildren(SwitchCase switchCase)
            {
                var list = new List<Expression>(switchCase.TestValues.Count + 1);
                list.AddRange(switchCase.TestValues);
                list.Add(switchCase.Body);
                return list;
            }
            
            return Children.Many(
                ImmutableList<Expression>
                    .Empty
                    .Add(s.SwitchValue)
                    .AddRange(s.Cases.SelectMany(GetSwitchCaseChildren))
            );
        }

        private Expression SetChildren(Children<Expression> newChildren, SwitchExpression s)
        {
            IEnumerable<SwitchCase> UpdateSwitchCases(IEnumerable<SwitchCase> oldCases, IEnumerable<Expression> c)
            {
                var newCases = new List<SwitchCase>(oldCases.Count());
                foreach (var oldCase in oldCases)
                {
                    var newTestValues = c.Take(oldCase.TestValues.Count);
                    c = c.Skip(oldCase.TestValues.Count);
                    var newBody = c.ElementAt(0);
                    c = c.Skip(1);
                    newCases.Add(oldCase.Update(newTestValues, newBody));
                }
                return newCases;
            }

            return s.Update(
                newChildren.Many[0],
                UpdateSwitchCases(s.Cases, newChildren.Many),
                newChildren.Many[newChildren.Many.Count - 1]
            );
        }
    }
}