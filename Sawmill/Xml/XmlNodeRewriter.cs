using System;
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

        /// <inheritdoc/>
        public Children<XmlNode> GetChildren(XmlNode value)
            => Children.Many<XmlNode>(value.ChildNodes.Cast<XmlNode>());

        /// <inheritdoc/>
        public XmlNode SetChildren(Children<XmlNode> newChildren, XmlNode oldValue)
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

        /// <inheritdoc/>
        public XmlNode RewriteChildren(Func<XmlNode, XmlNode> transformer, XmlNode oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="XElementRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="XElementRewriter"/>.</returns>
        public static XmlNodeRewriter Instance { get; } = new XmlNodeRewriter();
    }
}