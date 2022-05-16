using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Sawmill.Expressions
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="Expression"/>s.
    /// </summary>
    public partial class ExpressionRewriter : IRewriter<Expression>
    {
        /// <summary>
        /// Create a new instance of <see cref="ExpressionRewriter"/>
        /// </summary>
        protected ExpressionRewriter() { }

        /// <summary>
        /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(Expression value)
        {
            switch (value)
            {
                case BinaryExpression b:
                    return CountChildren(b);
                case BlockExpression b:
                    return CountChildren(b);
                case ConditionalExpression c:
                    return CountChildren(c);
                case DynamicExpression d:
                    return CountChildren(d);
                case GotoExpression g:
                    return CountChildren(g);
                case IndexExpression i:
                    return CountChildren(i);
                case InvocationExpression i:
                    return CountChildren(i);
                case LabelExpression l:
                    return CountChildren(l);
                case LambdaExpression l:
                    return CountChildren(l);
                case ListInitExpression l:
                    return CountChildren(l);
                case LoopExpression l:
                    return CountChildren(l);
                case MemberExpression m:
                    return CountChildren(m);
                case MemberInitExpression m:
                    return CountChildren(m);
                case MethodCallExpression m:
                    return CountChildren(m);
                case NewArrayExpression n:
                    return CountChildren(n);
                case NewExpression n:
                    return CountChildren(n);
                case SwitchExpression s:
                    return CountChildren(s);
                case TryExpression t:
                    return CountChildren(t);
                case TypeBinaryExpression t:
                    return CountChildren(t);
                case UnaryExpression u:
                    return CountChildren(u);
                case ConstantExpression _:
                case DebugInfoExpression _:
                case DefaultExpression _:
                case ParameterExpression _:
                case RuntimeVariablesExpression _:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<Expression> childrenReceiver, Expression value)
        {
            switch (value)
            {
                case BinaryExpression b:
                    GetChildren(childrenReceiver, b);
                    return;
                case BlockExpression b:
                    GetChildren(childrenReceiver, b);
                    return;
                case ConditionalExpression c:
                    GetChildren(childrenReceiver, c);
                    return;
                case DynamicExpression d:
                    GetChildren(childrenReceiver, d);
                    return;
                case GotoExpression g:
                    GetChildren(childrenReceiver, g);
                    return;
                case IndexExpression i:
                    GetChildren(childrenReceiver, i);
                    return;
                case InvocationExpression i:
                    GetChildren(childrenReceiver, i);
                    return;
                case LabelExpression l:
                    GetChildren(childrenReceiver, l);
                    return;
                case LambdaExpression l:
                    GetChildren(childrenReceiver, l);
                    return;
                case ListInitExpression l:
                    GetChildren(childrenReceiver, l);
                    return;
                case LoopExpression l:
                    GetChildren(childrenReceiver, l);
                    return;
                case MemberExpression m:
                    GetChildren(childrenReceiver, m);
                    return;
                case MemberInitExpression m:
                    GetChildren(childrenReceiver, m);
                    return;
                case MethodCallExpression m:
                    GetChildren(childrenReceiver, m);
                    return;
                case NewArrayExpression n:
                    GetChildren(childrenReceiver, n);
                    return;
                case NewExpression n:
                    GetChildren(childrenReceiver, n);
                    return;
                case SwitchExpression s:
                    GetChildren(childrenReceiver, s);
                    return;
                case TryExpression t:
                    GetChildren(childrenReceiver, t);
                    return;
                case TypeBinaryExpression t:
                    GetChildren(childrenReceiver, t);
                    return;
                case UnaryExpression u:
                    GetChildren(childrenReceiver, u);
                    return;
                case ConstantExpression _:
                case DebugInfoExpression _:
                case DefaultExpression _:
                case ParameterExpression _:
                case RuntimeVariablesExpression _:
                    return;
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public Expression SetChildren(ReadOnlySpan<Expression> newChildren, Expression value)
        {
            switch (value)
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
                    return value;
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Gets the single global instance of <see cref="ExpressionRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="ExpressionRewriter"/>.</returns>
        public static ExpressionRewriter Instance { get; } = new ExpressionRewriter();

        private static void Copy<T>(ReadOnlyCollection<T> collection, Span<T> span)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                span[i] = collection[i];
            }
        }

        private static void Copy<T>(IEnumerable<T> collection, Span<T> span)
        {
            var i = 0;
            foreach (var x in collection)
            {
                span[i] = x;
                i++;
            }
        }
    }
}
