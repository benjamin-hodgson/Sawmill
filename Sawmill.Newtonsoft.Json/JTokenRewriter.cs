using System;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json;

/// <summary>
/// An implementation of <see cref="IRewriter{T}" /> for <see cref="JToken" />s.
/// </summary>
public class JTokenRewriter : IRewriter<JToken>
{
    /// <summary>
    /// Create a new instance of <see cref="JTokenRewriter" />.
    /// </summary>
    protected JTokenRewriter()
    {
    }

    /// <summary>
    /// See <see cref="IRewriter{T}.CountChildren(T)" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value" />'s number of immediate children.</returns>
    public int CountChildren(JToken value)
        => value is JContainer c
            ? c.Children().Count()
            : 0;

    /// <summary>
    /// See <see cref="IRewriter{T}.GetChildren(Span{T}, T)" />.
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}" /> to copy <paramref name="value" />'s immediate children into.
    /// The <see cref="Span{T}" />'s <see cref="Span{T}.Length" /> will be equal to the number returned by <see cref="CountChildren" />.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<JToken> childrenReceiver, JToken value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value is JContainer)
        {
            var i = 0;
            foreach (var child in value.Children())
            {
                childrenReceiver[i] = child;
                i++;
            }
        }
    }

    /// <summary>
    /// See <see cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)" />.
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
    public JToken SetChildren(ReadOnlySpan<JToken> newChildren, JToken value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (newChildren.Length == 0)
        {
            return value;
        }

        var c = (JContainer)value.DeepClone();
        c.ReplaceAll(newChildren.ToArray());
        return c;
    }

    /// <summary>
    /// Gets the single global instance of <see cref="JTokenRewriter" />.
    /// </summary>
    /// <returns>The single global instance of <see cref="JTokenRewriter" />.</returns>
    public static JTokenRewriter Instance { get; } = new JTokenRewriter();
}
