using System;
using System.Collections.Immutable;
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

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public Children<XElement> GetChildren(XElement value)
            => value is XContainer c
                ? Children.Many(c.Elements().ToImmutableList())
                : Children.None<XElement>();

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public XElement SetChildren(Children<XElement> newChildren, XElement oldValue)
        {
            var clone = new XElement(oldValue);
            clone.RemoveNodes();
            clone.Add(newChildren.ToArray());
            return clone;
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public XElement RewriteChildren(Func<XElement, XElement> transformer, XElement oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="XElementRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="XElementRewriter"/>.</returns>
        public static XElementRewriter Instance { get; } = new XElementRewriter();
    }
}
