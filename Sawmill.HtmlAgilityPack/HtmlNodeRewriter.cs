using System;

using HtmlAgilityPack;

namespace Sawmill.HtmlAgilityPack
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="HtmlNode"/>s.
    /// </summary>
    public class HtmlNodeRewriter : IRewriter<HtmlNode>
    {
        /// <summary>
        /// Create a new instance of <see cref="HtmlNodeRewriter"/>
        /// </summary>
        protected HtmlNodeRewriter() { }

        /// <summary>
        /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(HtmlNode value)
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
        public void GetChildren(Span<HtmlNode> childrenReceiver, HtmlNode value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            for (var i = 0; i < value.ChildNodes.Count; i++)
            {
                childrenReceiver[i] = value.ChildNodes[i];
            }
        }

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public HtmlNode SetChildren(ReadOnlySpan<HtmlNode> newChildren, HtmlNode value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var clone = value.Clone();
            clone.ChildNodes.Clear();
            foreach (var child in newChildren)
            {
                clone.ChildNodes.Add(child);
            }
            return clone;
        }

        /// <summary>
        /// Gets the single global instance of <see cref="HtmlNodeRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="HtmlNodeRewriter"/>.</returns>
        public static IRewriter<HtmlNode> Instance { get; } = new HtmlNodeRewriter();
    }
}
