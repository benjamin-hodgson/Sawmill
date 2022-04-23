using System;
#if NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Update the immediate children of the value by applying a transformation function to each one.
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="transformer">A transformation function to apply to each of <paramref name="value"/>'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer"/>.</param>
        /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
        public static T RewriteChildren<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            var chunks = new ChunkStack<T>();

            var result = rewriter.RewriteChildrenInternal(transformer, value, ref chunks);

            chunks.Dispose();

            return result;
        }

        internal static T RewriteChildrenInternal<T>(
            this IRewriter<T> rewriter,
            Func<T, T> transformer,
            T value,
            ref ChunkStack<T> chunks
        ) => rewriter.WithChildren(
            (children, t) =>
            {
                var (r, v, f) = t;
                var changed = false;
                for (var i = 0; i < children.Length; i++)
                {
                    var child = children[i];
                    var newChild = f(child);
                    children[i] = newChild;
                    changed |= !ReferenceEquals(newChild, child);
                }

                if (changed)
                {
                    return r.SetChildren(children, v);
                }
                return v;
            },
            (rewriter, value, transformer),
            value,
            ref chunks
        );

#if NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Update the immediate children of the value by applying an asynchronous transformation function to each one.
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="transformer">An asynchronous transformation function to apply to each of <paramref name="value"/>'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer"/>.</param>
        /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="ValueTask"/>.</remarks>
        public static async ValueTask<T> RewriteChildren<T>(
            this IRewriter<T> rewriter,
            Func<T, ValueTask<T>> transformer,
            T value
        )
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            var chunks = new Box<ChunkStack<T>>(new ChunkStack<T>());

            var result = await rewriter.RewriteChildrenInternal(transformer, value, chunks);

            chunks.Value.Dispose();

            return result;
        }

        internal static ValueTask<T> RewriteChildrenInternal<T>(
            this IRewriter<T> rewriter,
            Func<T, ValueTask<T>> transformer,
            T value,
            Box<ChunkStack<T>> chunks
        ) => rewriter.WithChildren(
            async (children, t) =>
            {
                var (r, v, f) = t;
                var changed = false;
                for (var i = 0; i < children.Length; i++)
                {
                    var child = children.Span[i];
                    var newChild = await f(child);
                    children.Span[i] = newChild;
                    changed |= !ReferenceEquals(newChild, child);
                }

                if (changed)
                {
                    return r.SetChildren(children.Span, v);
                }
                return v;
            },
            (rewriter, value, transformer),
            value,
            chunks
        );
#endif
    }
}
