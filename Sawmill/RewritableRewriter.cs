using System;

namespace Sawmill
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <typeparamref name="T"/>s which implement <see cref="IRewritable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type</typeparam>
    public class RewritableRewriter<T> : IRewriter<T> where T : IRewritable<T>
    {
        /// <summary>
        /// Create an instance of <see cref="RewritableRewriter{T}"/>.
        /// </summary>
        protected RewritableRewriter() { }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value.CountChildren();
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<T> childrenReceiver, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            value.GetChildren(childrenReceiver);
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public T SetChildren(ReadOnlySpan<T> newChildren, T oldValue)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            return oldValue.SetChildren(newChildren);
        }

        /// <summary>
        /// Gets the single global instance of <see cref="RewritableRewriter{T}"/>
        /// </summary>
        public static RewritableRewriter<T> Instance { get; } = new RewritableRewriter<T>();
    }
}
