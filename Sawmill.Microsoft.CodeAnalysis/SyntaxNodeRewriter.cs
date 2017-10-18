using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sawmill.Microsoft.CodeAnalysis
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for sublcasses of <see cref="SyntaxNode"/>.
    /// </summary>
    public sealed class SyntaxNodeRewriter<T> : IRewriter<T> where T : SyntaxNode
    {
        private SyntaxNodeRewriter() { }

        /// <inheritdoc/>
        public Children<T> GetChildren(T value)
            => Children.Many((IEnumerable<T>)value.ChildNodes());

        /// <inheritdoc/>
        public T SetChildren(Children<T> newChildren, T oldValue)
            => oldValue.ChildNodes()
                .Zip(newChildren.Many, ValueTuple.Create)
                .Aggregate(oldValue, (x, tup) => x.ReplaceNode(tup.Item1, tup.Item2));

        /// <inheritdoc/>
        public T RewriteChildren(Func<T, T> transformer, T oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);
        
        /// <summary>
        /// Gets the single global instance of <see cref="SyntaxNodeRewriter{T}"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="SyntaxNodeRewriter{T}"/>.</returns>
        public static SyntaxNodeRewriter<T> Instance { get; } = new SyntaxNodeRewriter<T>();
    }
}
