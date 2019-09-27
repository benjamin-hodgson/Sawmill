using System;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;

namespace Sawmill.Xml
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="XmlNode"/>s.
    /// </summary>
    public sealed class XmlNodeRewriter : IRewriter<XmlNode>
    {
        private XmlNodeRewriter() { }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(XmlNode value) => value.ChildNodes.Count;

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<XmlNode> children, XmlNode value)
        {
            for (var i = 0; i < value.ChildNodes.Count; i++)
            {
                children[i] = value.ChildNodes[i];
            }
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public XmlNode SetChildren(ReadOnlySpan<XmlNode> newChildren, XmlNode oldValue)
        {
            // XmlNode is such garbage
            var oldAttrs = oldValue.Attributes;
            var clone = oldValue.Clone();
            clone.RemoveAll();
            foreach (XmlAttribute attr in oldAttrs)
            {
                clone.Attributes.Append((XmlAttribute)attr.Clone());
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
}
