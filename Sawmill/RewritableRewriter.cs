using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <typeparamref name="T"/>s which implement <see cref="IRewritable{T}"/>.
    /// </summary>
    public sealed class RewritableRewriter<T> : IRewriter<T> where T : IRewritable<T>
    {
        private RewritableRewriter() { }

        /// <inheritdoc/>
        public Children<T> GetChildren(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value.GetChildren();
        }

        /// <inheritdoc/>
        public T SetChildren(Children<T> newChildren, T oldValue)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            return oldValue.SetChildren(newChildren);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public static RewritableRewriter<T> Instance { get; } = new RewritableRewriter<T>();
    }
}