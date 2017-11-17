using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <typeparamref name="T"/>s which implement <see cref="IRewritable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type</typeparam>
    public sealed class RewritableRewriter<T> : IRewriter<T> where T : IRewritable<T>
    {
        private RewritableRewriter() { }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public Children<T> GetChildren(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value.GetChildren();
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public T SetChildren(Children<T> newChildren, T oldValue)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            return oldValue.SetChildren(newChildren);
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public T RewriteChildren(Func<T, T> transformer, T oldValue)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            return oldValue.RewriteChildren(transformer);
        }

        /// <summary>
        /// Gets the single global instance of <see cref="RewritableRewriter{T}"/>
        /// </summary>
        public static RewritableRewriter<T> Instance { get; } = new RewritableRewriter<T>();
    }
}