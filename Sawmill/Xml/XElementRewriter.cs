using System;
using System.Linq;
using System.Xml.Linq;

namespace Sawmill.Xml
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="XElement"/>s.
    /// </summary>
    public sealed class XElementRewriter : IRewriter<XElement>
    {
        private XElementRewriter() { }

        /// <inheritdoc/>
        public Children<XElement> GetChildren(XElement value)
            => value is XContainer c
                ? Children.Many(c.Elements())
                : Children.None<XElement>();

        /// <inheritdoc/>
        public XElement SetChildren(Children<XElement> newChildren, XElement oldValue)
        {
            var clone = new XElement(oldValue);
            clone.RemoveNodes();
            clone.Add(newChildren.ToArray());
            return clone;
        }

        /// <inheritdoc/>
        public XElement RewriteChildren(Func<XElement, XElement> transformer, XElement oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="XElementRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="XElementRewriter"/>.</returns>
        public static XElementRewriter Instance { get; } = new XElementRewriter();
    }
}