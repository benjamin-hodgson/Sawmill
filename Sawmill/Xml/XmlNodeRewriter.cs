using System;
using System.Xml;

namespace Sawmill.Xml;

/// <summary>
/// An implementation of <see cref="IRewriter{T}"/> for <see cref="XmlNode"/>s.
/// </summary>
public class XmlNodeRewriter : IRewriter<XmlNode>
{
    /// <summary>
    /// Create a new instance of <see cref="XmlNodeRewriter"/>.
    /// </summary>
    protected XmlNodeRewriter()
    {
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value"/>'s number of immediate children.</returns>
    public int CountChildren(XmlNode value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.ChildNodes.Count;
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
    /// </summary>
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}"/> to copy <paramref name="value"/>'s immediate children into.
    /// The <see cref="Span{T}"/>'s <see cref="Span{T}.Length"/> will be equal to the number returned by <see cref="CountChildren"/>.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<XmlNode> childrenReceiver, XmlNode value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        for (var i = 0; i < value.ChildNodes.Count; i++)
        {
            childrenReceiver[i] = value.ChildNodes[i]!;
        }
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
    /// </summary>
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
    public XmlNode SetChildren(ReadOnlySpan<XmlNode> newChildren, XmlNode value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        // XmlNode is such garbage
        var oldAttrs = value.Attributes;
        var clone = value.Clone();
        clone.RemoveAll();
        if (oldAttrs != null)
        {
            foreach (XmlAttribute? attr in oldAttrs)
            {
                // can't be null
                clone.Attributes!.Append((XmlAttribute)attr!.Clone());
            }
        }

        foreach (var newChild in newChildren)
        {
            clone.AppendChild(newChild.Clone());
        }

        return clone;
    }

    /// <summary>
    /// Gets the single global instance of <see cref="XElementRewriter"/>.
    /// </summary>
    /// <returns>The single global instance of <see cref="XElementRewriter"/>.</returns>
    public static XmlNodeRewriter Instance { get; } = new XmlNodeRewriter();
}
