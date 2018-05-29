using System;
using System.Collections.Immutable;
using HtmlAgilityPack;

namespace Sawmill.HtmlAgilityPack
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="HtmlNode"/>s.
    /// </summary>
    public class HtmlNodeRewriter : IRewriter<HtmlNode>
    {
        private HtmlNodeRewriter() {}

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public Children<HtmlNode> GetChildren(HtmlNode value)
            => value.ChildNodes.ToImmutableList();

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public HtmlNode SetChildren(Children<HtmlNode> newChildren, HtmlNode value)
        {
            var clone = value.Clone();
            clone.ChildNodes.Clear();
            foreach (var child in newChildren)
            {
                clone.ChildNodes.Add(child);
            }
            return clone;
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public HtmlNode RewriteChildren(Func<HtmlNode, HtmlNode> transformer, HtmlNode value)
            => this.DefaultRewriteChildren(transformer, value);

        /// <summary>
        /// Gets the single global instance of <see cref="HtmlNodeRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="HtmlNodeRewriter"/>.</returns>
        public static IRewriter<HtmlNode> Instance { get; } = new HtmlNodeRewriter();
    }
}
