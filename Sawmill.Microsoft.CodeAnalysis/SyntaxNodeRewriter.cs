using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Sawmill.Microsoft.CodeAnalysis;

/// <summary>
/// An implementation of <see cref="IRewriter{T}"/> for subclasses of <see cref="SyntaxNode"/>.
/// </summary>
/// <typeparam name="T">The type of syntax node.</typeparam>
public class SyntaxNodeRewriter<T> : IRewriter<T>
    where T : SyntaxNode
{
    /// <summary>
    /// create a new instance of <see cref="SyntaxNodeRewriter{T}"/>.
    /// </summary>
    protected SyntaxNodeRewriter()
    {
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value"/>'s number of immediate children.</returns>
    public int CountChildren(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.ChildNodes().Count();
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}"/> to copy <paramref name="value"/>'s immediate children into.
    /// The <see cref="Span{T}"/>'s <see cref="Span{T}.Length"/> will be equal to the number returned by <see cref="CountChildren"/>.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<T> childrenReceiver, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var i = 0;
        foreach (var child in (IEnumerable<T>)value.ChildNodes())
        {
            childrenReceiver[i] = child;
            i++;
        }
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
    public T SetChildren(ReadOnlySpan<T> newChildren, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.ChildNodes()
            .Zip(newChildren.ToArray(), ValueTuple.Create)
            .Aggregate(value, (x, tup) => x.ReplaceNode(tup.Item1, tup.Item2));
    }

    /// <summary>
    /// Gets the single global instance of <see cref="SyntaxNodeRewriter{T}"/>.
    /// </summary>
    /// <returns>The single global instance of <see cref="SyntaxNodeRewriter{T}"/>.</returns>
    [SuppressMessage("design", "CA1000", Justification = "Purposeful")] // "Do not declare static members on generic types"
    public static SyntaxNodeRewriter<T> Instance { get; } = new SyntaxNodeRewriter<T>();
}
