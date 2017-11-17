using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="Expression"/>s.
    /// </summary>
    public sealed class ExpressionRewriter : IRewriter<Expression>
    {
        private ExpressionRewriter() {}

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public Children<Expression> GetChildren(Expression value)
        {
            switch (value)
            {
                case BinaryExpression b:
                    return Children.Two(b.Left, b.Right);
                case BlockExpression b:
                    return Children.Many(b.Expressions);
                case ConditionalExpression c:
                    return Children.Many(new[]{ c.Test, c.IfTrue, c.IfFalse });
                case DynamicExpression d:
                    return Children.Many(d.Arguments);
                case GotoExpression g:
                    return Children.One(g.Value);
                case IndexExpression i:
                    var indexExpressionChildren = new List<Expression>(i.Arguments.Count + 1) { i.Object };
                    indexExpressionChildren.AddRange(i.Arguments);
                    return Children.Many(indexExpressionChildren);
                case InvocationExpression i:
                    var invocationExpressionChildren = new List<Expression>(i.Arguments.Count + 1) { i.Expression };
                    invocationExpressionChildren.AddRange(i.Arguments);
                    return Children.Many(invocationExpressionChildren);
                case LabelExpression l:
                    return Children.One(l.DefaultValue);
                case LambdaExpression l:
                    return Children.One(l.Body);
                case ListInitExpression l:
                    return Children.Many(l.Initializers.SelectMany(i => i.Arguments));
                case LoopExpression l:
                    return Children.One(l.Body);
                case MemberExpression m:
                    return Children.One(m.Expression);
                case MemberInitExpression m:
                    return Children.Many(m.Bindings.SelectMany(GetBindingExpr));
                case MethodCallExpression m:
                    var methodCallExpressionChildren = new List<Expression>
                    {
                        m.Object
                    };
                    methodCallExpressionChildren.AddRange(m.Arguments);
                    return Children.Many(methodCallExpressionChildren);
                case NewArrayExpression n:
                    return Children.Many(n.Expressions);
                case NewExpression n:
                    return Children.Many(n.Arguments);
                case SwitchExpression s:
                    var switchExpressionChildren = new List<Expression>
                    {
                        s.SwitchValue
                    };
                    switchExpressionChildren.AddRange(s.Cases.SelectMany(GetSwitchCaseChildren));
                    return Children.Many(switchExpressionChildren);
                case TryExpression t:
                    var tryExpressionChildren = new List<Expression>(t.Handlers.Count * 2 + 3);
                    tryExpressionChildren.Add(t.Body);
                    foreach (var h in t.Handlers)
                    {
                        tryExpressionChildren.Add(h.Filter);
                        tryExpressionChildren.Add(h.Body);
                    }
                    tryExpressionChildren.Add(t.Finally);
                    tryExpressionChildren.Add(t.Fault);
                    return Children.Many(tryExpressionChildren);
                case TypeBinaryExpression t:
                    return Children.One(t.Expression);
                case UnaryExpression u:
                    return Children.One(u.Operand);
                case ConstantExpression _:
                case DebugInfoExpression _:
                case DefaultExpression _:
                case ParameterExpression _:
                case RuntimeVariablesExpression _:
                    return Children.None<Expression>();
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public Expression SetChildren(Children<Expression> newChildren, Expression oldValue)
        {
            switch (oldValue)
            {
                case BinaryExpression b:
                    return b.Update(newChildren.First, b.Conversion, newChildren.Second);
                case BlockExpression b:
                    return b.Update(b.Variables, newChildren.Many);
                case ConditionalExpression c:
                    return c.Update(newChildren.Many.ElementAt(0), newChildren.Many.ElementAt(1), newChildren.Many.ElementAt(2));
                case DynamicExpression d:
                    return d.Update(d.Arguments);
                case GotoExpression g:
                    return g.Update(g.Target, newChildren.First);
                case IndexExpression i:
                    return i.Update(newChildren.Many.ElementAt(0), newChildren.Many.Skip(1));
                case InvocationExpression i:
                    return i.Update(newChildren.Many.ElementAt(0), newChildren.Many.Skip(1));
                case LabelExpression l:
                    return l.Update(l.Target, newChildren.First);
                case LambdaExpression l:
                    return Expression.Lambda(newChildren.First, l.Name, l.TailCall, l.Parameters);
                case ListInitExpression l:
                    return l.Update(l.NewExpression, UpdateElementInits(l.Initializers, newChildren.Many));
                case LoopExpression l:
                    return l.Update(l.BreakLabel, l.ContinueLabel, newChildren.First);
                case MemberExpression m:
                    return m.Update(newChildren.First);
                case MemberInitExpression m:
                    return m.Update(m.NewExpression, UpdateBindings(m.Bindings, newChildren.Many).Item1);
                case MethodCallExpression m:
                    return m.Update(newChildren.Many.ElementAt(0), newChildren.Many.Skip(1));
                case NewArrayExpression n:
                    return n.Update(newChildren.Many);
                case NewExpression n:
                    return n.Update(newChildren.Many);
                case SwitchExpression s:
                    return s.Update(
                        newChildren.Many.ElementAt(0),
                        UpdateSwitchCases(s.Cases, newChildren.Many),
                        newChildren.Many.Last()
                    );
                case TryExpression t:
                    var newTryBlock = newChildren.Many.ElementAt(0);
                    var updateResult = UpdateCatchBlocks(t.Handlers, newChildren.Many.Skip(1));
                    var remainingNewChildren = updateResult.Item2;
                    return t.Update(
                        newTryBlock,
                        updateResult.Item1,
                        remainingNewChildren.ElementAt(0),
                        remainingNewChildren.ElementAt(1)
                    );
                case TypeBinaryExpression t:
                    return t.Update(newChildren.First);
                case UnaryExpression u:
                    return u.Update(newChildren.First);
                case ConstantExpression _:
                case DebugInfoExpression _:
                case DefaultExpression _:
                case ParameterExpression _:
                case RuntimeVariablesExpression _:
                    return oldValue;
            }
            throw new ArgumentOutOfRangeException(nameof(oldValue));
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public Expression RewriteChildren(Func<Expression, Expression> transformer, Expression value)
            => this.DefaultRewriteChildren(transformer, value);

        /// <summary>
        /// Gets the single global instance of <see cref="ExpressionRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="ExpressionRewriter"/>.</returns>
        public static ExpressionRewriter Instance { get; } = new ExpressionRewriter();

        private IEnumerable<Expression> GetBindingExpr(MemberBinding binding)
        {
            switch (binding)
            {
                case MemberAssignment a:
                    return new[] { a.Expression };
                case MemberListBinding l:
                    return l.Initializers.SelectMany(i => i.Arguments);
                case MemberMemberBinding m:
                    return m.Bindings.SelectMany(GetBindingExpr);
                default:
                    throw new ArgumentOutOfRangeException(nameof(binding));
            }
        }
        private IEnumerable<Expression> GetSwitchCaseChildren(SwitchCase switchCase)
        {
            var list = new List<Expression>(switchCase.TestValues.Count + 1);
            list.AddRange(switchCase.TestValues);
            list.Add(switchCase.Body);
            return list;
        }
        private IEnumerable<ElementInit> UpdateElementInits(IEnumerable<ElementInit> oldInits, IEnumerable<Expression> newArguments)
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
        private (IEnumerable<MemberBinding>, IEnumerable<Expression>) UpdateBindings(IEnumerable<MemberBinding> oldBindings, IEnumerable<Expression> newArgs)
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
                    case MemberMemberBinding m:
                        var updatedBindings = UpdateBindings(m.Bindings, newArgs);
                        newBinding = m.Update(updatedBindings.Item1);
                        newArgs = updatedBindings.Item2;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(binding));
                }
                newBindings.Add(newBinding);
            }
            return (newBindings, newArgs);
        }
        private IEnumerable<SwitchCase> UpdateSwitchCases(IEnumerable<SwitchCase> oldCases, IEnumerable<Expression> newChildren)
        {
            var newCases = new List<SwitchCase>(oldCases.Count());
            foreach (var oldCase in oldCases)
            {
                var newTestValues = newChildren.Take(oldCase.TestValues.Count);
                newChildren = newChildren.Skip(oldCase.TestValues.Count);
                var newBody = newChildren.ElementAt(0);
                newChildren = newChildren.Skip(1);
                newCases.Add(oldCase.Update(newTestValues, newBody));
            }
            return newCases;
        }
        private (IEnumerable<CatchBlock>, IEnumerable<Expression>) UpdateCatchBlocks(IEnumerable<CatchBlock> oldCatchBlocks, IEnumerable<Expression> newChildren)
        {
            var newCatchBlocks = new List<CatchBlock>(oldCatchBlocks.Count());
            foreach (var oldCatchBlock in oldCatchBlocks)
            {
                newCatchBlocks.Add(oldCatchBlock.Update(oldCatchBlock.Variable, newChildren.ElementAt(0), newChildren.ElementAt(1)));
                newChildren = newChildren.Skip(2);
            }
            return (newCatchBlocks, newChildren);
        }
    }
}