using System;
using System.Linq;
using System.Xml.Linq;

namespace Sawmill.Xml;

/// <summary>
/// An implementation of <see cref="IRewriter{T}"/> for <see cref="XElement"/>s.
/// </summary>
public class XElementRewriter : IRewriter<XElement>
{
    /// <summary>
    /// Create a new instance of <see cref="XElementRewriter"/>.
    /// </summary>
    protected XElementRewriter()
    {
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value"/>'s number of immediate children.</returns>
    public int CountChildren(XElement value) => value is XContainer c ? c.Elements().Count() : 0;

    /// <summary>
    /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}"/> to copy <paramref name="value"/>'s immediate children into.
    /// The <see cref="Span{T}"/>'s <see cref="Span{T}.Length"/> will be equal to the number returned by <see cref="CountChildren"/>.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<XElement> childrenReceiver, XElement value)
    {
        if (value is XContainer c)
        {
            var i = 0;
            foreach (var e in c.Elements())
            {
                childrenReceiver[i] = e;
                i++;
            }
        }
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
    public XElement SetChildren(ReadOnlySpan<XElement> newChildren, XElement value)
    {
        var clone = new XElement(value);
        clone.RemoveNodes();
        clone.Add(newChildren.ToArray());
        return clone;
    }

    /// <summary>
    /// Gets the single global instance of <see cref="XElementRewriter"/>.
    /// </summary>
    /// <returns>The single global instance of <see cref="XElementRewriter"/>.</returns>
    public static XElementRewriter Instance { get; } = new XElementRewriter();
}
