using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="Expression"/>s.
    /// </summary>
    public sealed partial class ExpressionRewriter : IRewriter<Expression>
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
                    return GetChildren(b);
                case BlockExpression b:
                    return GetChildren(b);
                case ConditionalExpression c:
                    return GetChildren(c);
                case DynamicExpression d:
                    return GetChildren(d);
                case GotoExpression g:
                    return GetChildren(g);
                case IndexExpression i:
                    return GetChildren(i);
                case InvocationExpression i:
                    return GetChildren(i);
                case LabelExpression l:
                    return GetChildren(l);
                case LambdaExpression l:
                    return GetChildren(l);
                case ListInitExpression l:
                    return GetChildren(l);
                case LoopExpression l:
                    return GetChildren(l);
                case MemberExpression m:
                    return GetChildren(m);
                case MemberInitExpression m:
                    return GetChildren(m);
                case MethodCallExpression m:
                    return GetChildren(m);
                case NewArrayExpression n:
                    return GetChildren(n);
                case NewExpression n:
                    return GetChildren(n);
                case SwitchExpression s:
                    return GetChildren(s);
                case TryExpression t:
                    return GetChildren(t);
                case TypeBinaryExpression t:
                    return GetChildren(t);
                case UnaryExpression u:
                    return GetChildren(u);
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
                    return SetChildren(newChildren, b);
                case BlockExpression b:
                    return SetChildren(newChildren, b);
                case ConditionalExpression c:
                    return SetChildren(newChildren, c);
                case DynamicExpression d:
                    return SetChildren(newChildren, d);
                case GotoExpression g:
                    return SetChildren(newChildren, g);
                case IndexExpression i:
                    return SetChildren(newChildren, i);
                case InvocationExpression i:
                    return SetChildren(newChildren, i);
                case LabelExpression l:
                    return SetChildren(newChildren, l);
                case LambdaExpression l:
                    return SetChildren(newChildren, l);
                case ListInitExpression l:
                    return SetChildren(newChildren, l);
                case LoopExpression l:
                    return SetChildren(newChildren, l);
                case MemberExpression m:
                    return SetChildren(newChildren, m);
                case MemberInitExpression m:
                    return SetChildren(newChildren, m);
                case MethodCallExpression m:
                    return SetChildren(newChildren, m);
                case NewArrayExpression n:
                    return SetChildren(newChildren, n);
                case NewExpression n:
                    return SetChildren(newChildren, n);
                case SwitchExpression s:
                    return SetChildren(newChildren, s);
                case TryExpression t:
                    return SetChildren(newChildren, t);
                case TypeBinaryExpression t:
                    return SetChildren(newChildren, t);
                case UnaryExpression u:
                    return SetChildren(newChildren, u);
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
    }
}
