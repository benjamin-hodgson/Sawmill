using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Sawmill.Expressions;

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
        return value switch
        {
            BinaryExpression b => CountChildren(b),
            BlockExpression b => CountChildren(b),
            ConditionalExpression c => CountChildren(c),
            DynamicExpression d => CountChildren(d),
            GotoExpression g => CountChildren(g),
            IndexExpression i => CountChildren(i),
            InvocationExpression i => CountChildren(i),
            LabelExpression l => CountChildren(l),
            LambdaExpression l => CountChildren(l),
            ListInitExpression l => CountChildren(l),
            LoopExpression l => CountChildren(l),
            MemberExpression m => CountChildren(m),
            MemberInitExpression m => CountChildren(m),
            MethodCallExpression m => CountChildren(m),
            NewArrayExpression n => CountChildren(n),
            NewExpression n => CountChildren(n),
            SwitchExpression s => CountChildren(s),
            TryExpression t => CountChildren(t),
            TypeBinaryExpression t => CountChildren(t),
            UnaryExpression u => CountChildren(u),
            ConstantExpression _ or DebugInfoExpression _ or DefaultExpression _ or ParameterExpression _ or RuntimeVariablesExpression _ => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
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
        return value switch
        {
            BinaryExpression b => SetChildren(newChildren, b),
            BlockExpression b => SetChildren(newChildren, b),
            ConditionalExpression c => SetChildren(newChildren, c),
            DynamicExpression d => SetChildren(newChildren, d),
            GotoExpression g => SetChildren(newChildren, g),
            IndexExpression i => SetChildren(newChildren, i),
            InvocationExpression i => SetChildren(newChildren, i),
            LabelExpression l => SetChildren(newChildren, l),
            LambdaExpression l => SetChildren(newChildren, l),
            ListInitExpression l => SetChildren(newChildren, l),
            LoopExpression l => SetChildren(newChildren, l),
            MemberExpression m => SetChildren(newChildren, m),
            MemberInitExpression m => SetChildren(newChildren, m),
            MethodCallExpression m => SetChildren(newChildren, m),
            NewArrayExpression n => SetChildren(newChildren, n),
            NewExpression n => SetChildren(newChildren, n),
            SwitchExpression s => SetChildren(newChildren, s),
            TryExpression t => SetChildren(newChildren, t),
            TypeBinaryExpression t => SetChildren(newChildren, t),
            UnaryExpression u => SetChildren(newChildren, u),
            ConstantExpression _ or DebugInfoExpression _ or DefaultExpression _ or ParameterExpression _ or RuntimeVariablesExpression _ => value,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
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
