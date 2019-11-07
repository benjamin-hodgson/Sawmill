using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public partial class ExpressionRewriter
    {
        private static int CountChildren(MemberInitExpression m)
        {
            int CountBindingExprs(MemberBinding binding)
            {
                switch (binding)
                {
                    case MemberAssignment a:
                        return 1;
                    case MemberListBinding l:
                        return l.Initializers.Select(i => i.Arguments.Count).Sum();
                    case MemberMemberBinding mm:
                        return mm.Bindings.Select(CountBindingExprs).Sum();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(binding));
                }
            }
            return m.Bindings.Select(CountBindingExprs).Sum();
        }

        private static void GetChildren(Span<Expression> children, MemberInitExpression m)
        {
            IEnumerable<Expression> GetBindingExprs(MemberBinding binding)
            {
                switch (binding)
                {
                    case MemberAssignment a:
                        return new[] { a.Expression };
                    case MemberListBinding l:
                        return l.Initializers.SelectMany(i => i.Arguments);
                    case MemberMemberBinding mm:
                        return mm.Bindings.SelectMany(GetBindingExprs);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(binding));
                }
            }

            Copy(m.Bindings.SelectMany(GetBindingExprs), children);
        }

        private static Expression SetChildren(ReadOnlySpan<Expression> newChildren, MemberInitExpression m)
        {
            MemberBindingUpdateResult UpdateBindings(IEnumerable<MemberBinding> oldBindings, ReadOnlySpan<Expression> newArgs)
            {
                var newBindings = new List<MemberBinding>();
                foreach (var binding in oldBindings)
                {
                    MemberBinding newBinding;
                    switch (binding)
                    {
                        case MemberAssignment a:
                            newBinding = a.Update(newArgs[0]);
                            newArgs = newArgs.Slice(1);
                            break;
                        case MemberListBinding l:
                            var newInits = new List<ElementInit>(l.Initializers.Count);
                            foreach (var oldInit in l.Initializers)
                            {
                                newInits.Add(oldInit.Update(newArgs.Slice(0, oldInit.Arguments.Count).ToArray()));
                                newArgs = newArgs.Slice(oldInit.Arguments.Count);
                            }
                            newBinding = l.Update(newInits);
                            break;
                        case MemberMemberBinding mm:
                            var updatedBindings = UpdateBindings(mm.Bindings, newArgs);
                            newBinding = mm.Update(updatedBindings.Bindings);
                            newArgs = updatedBindings.Remainder;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(binding));
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
}
