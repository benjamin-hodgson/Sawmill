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
    protected XmlNodeRewriter() { }

    /// <summary>
    /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
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
