using System;
using System.Diagnostics.CodeAnalysis;

namespace Sawmill;

/// <summary>
/// An implementation of <see cref="IRewriter{T}"/> for <typeparamref name="T"/>s which implement <see cref="IRewritable{T}"/>.
/// </summary>
/// <typeparam name="T">The rewritable tree type.</typeparam>
public class RewritableRewriter<T> : IRewriter<T>
    where T : IRewritable<T>
{
    /// <summary>
    /// Create an instance of <see cref="RewritableRewriter{T}"/>.
    /// </summary>
    protected RewritableRewriter()
    {
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
    /// <param name="value">The rewritable tree.</param>
    /// <returns>The <paramref name="value" />'s number of immediate children.</returns>
    public int CountChildren(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.CountChildren();
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}"/> to copy the current instance's immediate children into.
    /// The <see cref="Span{T}"/>'s <see cref="Span{T}.Length"/> should be equal to the number returned by <see cref="CountChildren"/>.
    /// </param>
    /// <param name="value">The rewritable tree.</param>
    public void GetChildren(Span<T> childrenReceiver, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        value.GetChildren(childrenReceiver);
    }

    /// <summary>
    /// See <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The rewritable tree.</param>
    /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
    public T SetChildren(ReadOnlySpan<T> newChildren, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.SetChildren(newChildren);
    }

    /// <summary>
    /// Gets the single global instance of <see cref="RewritableRewriter{T}"/>.
    /// </summary>
    [SuppressMessage("design", "CA1000", Justification = "Breaking change")] // "Do not declare static members on generic types"
    public static RewritableRewriter<T> Instance { get; } = new RewritableRewriter<T>();
}
