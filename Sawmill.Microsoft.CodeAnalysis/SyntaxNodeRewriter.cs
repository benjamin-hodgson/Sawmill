using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value.ChildNodes().Count();
        }

        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<T> childrenReceiver, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var i = 0;
            foreach (var child in (IEnumerable<T>)value.ChildNodes())
            {
                childrenReceiver[i] = child;
                i++;
            }
        }

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public T SetChildren(ReadOnlySpan<T> newChildren, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value.ChildNodes()
                .Zip(newChildren.ToArray(), ValueTuple.Create)
                .Aggregate(value, (x, tup) => x.ReplaceNode(tup.Item1, tup.Item2));
        }

        /// <summary>
        /// Gets the single global instance of <see cref="SyntaxNodeRewriter{T}"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="SyntaxNodeRewriter{T}"/>.</returns>
        [SuppressMessage("design", "CA1000")]  // "Do not declare static members on generic types"
        public static SyntaxNodeRewriter<T> Instance { get; } = new SyntaxNodeRewriter<T>();
    }
}
