using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    public sealed partial class ExpressionRewriter
    {
        private Children<Expression> GetChildren(MemberInitExpression m)
        {
            IEnumerable<Expression> GetBindingExpr(MemberBinding binding)
            {
                switch (binding)
                {
                    case MemberAssignment a:
                        return new[] { a.Expression };
                    case MemberListBinding l:
                        return l.Initializers.SelectMany(i => i.Arguments);
                    case MemberMemberBinding mm:
                        return mm.Bindings.SelectMany(GetBindingExpr);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(binding));
                }
            }
            
            return Children.Many(m.Bindings.SelectMany(GetBindingExpr).ToImmutableList());
        }

        private Expression SetChildren(Children<Expression> newChildren, MemberInitExpression m)
        {
            (IEnumerable<MemberBinding>, IEnumerable<Expression>) UpdateBindings(IEnumerable<MemberBinding> oldBindings, IEnumerable<Expression> newArgs)
            {
                var newBindings = new List<MemberBinding>();
                foreach (var binding in oldBindings)
                {
                    MemberBinding newBinding;
                    switch (binding)
                    {
                        case MemberAssignment a:
                            newBinding = a.Update(newArgs.First());
                            newArgs = newArgs.Skip(1);
                            break;
                        case MemberListBinding l:
                            var newInits = new List<ElementInit>(l.Initializers.Count);
                            foreach (var oldInit in l.Initializers)
                            {
                                newInits.Add(oldInit.Update(newArgs.Take(oldInit.Arguments.Count)));
                                newArgs = newArgs.Skip(oldInit.Arguments.Count);
                            }
                            newBinding = l.Update(newInits);
                            break;
                        case MemberMemberBinding mm:
                            var updatedBindings = UpdateBindings(mm.Bindings, newArgs);
                            newBinding = mm.Update(updatedBindings.Item1);
                            newArgs = updatedBindings.Item2;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(binding));
                    }
                    newBindings.Add(newBinding);
                }
                return (newBindings, newArgs);
            }
        
            return m.Update(m.NewExpression, UpdateBindings(m.Bindings, newChildren.Many).Item1);
        }
    }
}