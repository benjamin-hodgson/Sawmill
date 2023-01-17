using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(SwitchExpression s) => s.Cases.Select(c => c.TestValues.Count + 1).Sum() + 1;

    private static void GetChildren(Span<Expression> children, SwitchExpression s)
    {
        static IEnumerable<Expression> GetSwitchCaseChildren(SwitchCase switchCase)
        {
            var list = new List<Expression>(switchCase.TestValues.Count + 1);
            list.AddRange(switchCase.TestValues);
            list.Add(switchCase.Body);
            return list;
        }

        children[0] = s.SwitchValue;
        Copy(s.Cases.SelectMany(GetSwitchCaseChildren), children[1..]);
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, SwitchExpression s)
    {
        static IEnumerable<SwitchCase> UpdateSwitchCases(IEnumerable<SwitchCase> oldCases, ReadOnlySpan<Expression> c)
        {
            var newCases = new List<SwitchCase>(oldCases.Count());
            foreach (var oldCase in oldCases)
            {
                var newTestValues = c[..oldCase.TestValues.Count];
                c = c[oldCase.TestValues.Count..];
                var newBody = c[0];
                c = c[1..];
                newCases.Add(oldCase.Update(newTestValues.ToArray(), newBody));
            }

            return newCases;
        }

        return s.Update(
            newChildren[0],
            UpdateSwitchCases(s.Cases, newChildren[1..]),
            newChildren[^1]
        );
    }
}
