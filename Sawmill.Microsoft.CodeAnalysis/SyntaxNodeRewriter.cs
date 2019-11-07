using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sawmill.Microsoft.CodeAnalysis
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for sublcasses of <see cref="SyntaxNode"/>.
    /// </summary>
    public class SyntaxNodeRewriter<T> : IRewriter<T> where T : SyntaxNode
    {
        /// <summary>
        /// create a new instance of <see cref="SyntaxNodeRewriter{T}"/>
        /// </summary>
        protected SyntaxNodeRewriter() { }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(T value) => value.ChildNodes().Count();

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<T> children, T value)
        {
            var i = 0;
            foreach (var child in (IEnumerable<T>)value.ChildNodes())
            {
                children[i] = child;
                i++;
            }
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public T SetChildren(ReadOnlySpan<T> newChildren, T oldValue)
            => oldValue.ChildNodes()
                .Zip(newChildren.ToArray(), ValueTuple.Create)
                .Aggregate(oldValue, (x, tup) => x.ReplaceNode(tup.Item1, tup.Item2));

        /// <summary>
        /// Gets the single global instance of <see cref="SyntaxNodeRewriter{T}"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="SyntaxNodeRewriter{T}"/>.</returns>
        public static SyntaxNodeRewriter<T> Instance { get; } = new SyntaxNodeRewriter<T>();
    }
}
