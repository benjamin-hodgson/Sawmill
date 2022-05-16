using System;
using System.Linq;
using System.Xml.Linq;

namespace Sawmill.Xml
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="XElement"/>s.
    /// </summary>
    public class XElementRewriter : IRewriter<XElement>
    {
        /// <summary>
        /// Create a new instance of <see cref="XElementRewriter"/>.
        /// </summary>
        protected XElementRewriter() { }

        /// <summary>
        /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(XElement value) => value is XContainer c ? c.Elements().Count() : 0;

        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
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
}
