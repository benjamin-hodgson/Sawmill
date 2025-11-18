using System;

using HtmlAgilityPack;

namespace Sawmill.HtmlAgilityPack;

/// <summary>
/// An implementation of <see cref="IRewriter{T}" /> for <see cref="HtmlNode" />s.
/// </summary>
public class HtmlNodeRewriter : IRewriter<HtmlNode>
{
    /// <summary>
    /// Create a new instance of <see cref="HtmlNodeRewriter" />.
    /// </summary>
    protected HtmlNodeRewriter()
    {
    }

    /// <summary>
    /// See <see cref="IRewriter{T}.CountChildren(T)" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value" />'s number of immediate children.</returns>
    public int CountChildren(HtmlNode value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.ChildNodes.Count;
    }

    /// <summary>
    /// See <see cref="IRewriter{T}.GetChildren(Span{T}, T)" />.
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}" /> to copy <paramref name="value" />'s immediate children into.
    /// The <see cref="Span{T}" />'s <see cref="Span{T}.Length" /> will be equal to the number returned by <see cref="CountChildren" />.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<HtmlNode> childrenReceiver, HtmlNode value)
    {
        ArgumentNullException.ThrowIfNull(value);

        for (var i = 0; i < value.ChildNodes.Count; i++)
        {
            childrenReceiver[i] = value.ChildNodes[i];
        }
    }

    /// <summary>
    /// See <see cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)" />.
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
    public HtmlNode SetChildren(ReadOnlySpan<HtmlNode> newChildren, HtmlNode value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var clone = value.Clone();
        clone.ChildNodes.Clear();
        foreach (var child in newChildren)
        {
            clone.ChildNodes.Add(child);
        }

        return clone;
    }

    /// <summary>
    /// Gets the single global instance of <see cref="HtmlNodeRewriter" />.
    /// </summary>
    /// <returns>The single global instance of <see cref="HtmlNodeRewriter" />.</returns>
    public static IRewriter<HtmlNode> Instance { get; } = new HtmlNodeRewriter();
}
