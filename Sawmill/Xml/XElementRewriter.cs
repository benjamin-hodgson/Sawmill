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
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(XElement value) => value is XContainer c ? c.Elements().Count() : 0;

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<XElement> children, XElement value)
        {
            if (value is XContainer c)
            {
                var i = 0;
                foreach (var e in c.Elements())
                {
                    children[i] = e;
                    i++;
                }
            }
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public XElement SetChildren(ReadOnlySpan<XElement> newChildren, XElement oldValue)
        {
            var clone = new XElement(oldValue);
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
}
