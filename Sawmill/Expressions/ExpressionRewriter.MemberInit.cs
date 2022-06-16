using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

public partial class ExpressionRewriter
{
    private static int CountChildren(MemberInitExpression m)
    {
        static int CountBindingExprs(MemberBinding binding)
            => binding switch
            {
                MemberAssignment a => 1,
                MemberListBinding l => l.Initializers.Select(i => i.Arguments.Count).Sum(),
                MemberMemberBinding mm => mm.Bindings.Select(CountBindingExprs).Sum(),
                _ => throw new ArgumentOutOfRangeException(nameof(binding)),
            };

        return m.Bindings.Select(CountBindingExprs).Sum();
    }

    private static void GetChildren(Span<Expression> children, MemberInitExpression m)
    {
        static IEnumerable<Expression> GetBindingExprs(MemberBinding binding)
            => binding switch
            {
                MemberAssignment a => new[] { a.Expression },
                MemberListBinding l => l.Initializers.SelectMany(i => i.Arguments),
                MemberMemberBinding mm => mm.Bindings.SelectMany(GetBindingExprs),
                _ => throw new ArgumentOutOfRangeException(nameof(binding)),
            };

        Copy(m.Bindings.SelectMany(GetBindingExprs), children);
    }

    private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, MemberInitExpression m)
    {
        static MemberBindingUpdateResult UpdateBindings(IEnumerable<MemberBinding> oldBindings, ReadOnlySpan<Expression> newArgs)
        {
            var newBindings = new List<MemberBinding>();
            foreach (var binding in oldBindings)
            {
                MemberBinding newBinding;
                switch (binding)
                {
                    case MemberAssignment a:
                        newBinding = a.Update(newArgs[0]);
                        newArgs = newArgs[1..];
                        break;
                    case MemberListBinding l:
                        var newInits = new List<ElementInit>(l.Initializers.Count);
                        foreach (var oldInit in l.Initializers)
                        {
                            newInits.Add(oldInit.Update(newArgs[..oldInit.Arguments.Count].ToArray()));
                            newArgs = newArgs[oldInit.Arguments.Count..];
                        }
                        newBinding = l.Update(newInits);
                        break;
                    case MemberMemberBinding mm:
                        var updatedBindings = UpdateBindings(mm.Bindings, newArgs);
                        newBinding = mm.Update(updatedBindings.Bindings);
                        newArgs = updatedBindings.Remainder;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unexpected type of binding", nameof(binding));
                }
                newBindings.Add(newBinding);
            }
            return new MemberBindingUpdateResult(newBindings, newArgs);
        }

        return m.Update(m.NewExpression, UpdateBindings(m.Bindings, newChildren).Bindings);
    }

    private readonly ref struct MemberBindingUpdateResult
    {
        public IEnumerable<MemberBinding> Bindings { get; }
        public ReadOnlySpan<Expression> Remainder { get; }
        public MemberBindingUpdateResult(IEnumerable<MemberBinding> bindings, ReadOnlySpan<Expression> remainder)
        {
            Bindings = bindings;
            Remainder = remainder;
        }
    }
}
