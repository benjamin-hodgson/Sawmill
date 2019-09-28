using System;
using System.Buffers;

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

            var stack = new ChunkStack<T>();

            var result = rewriter.RewriteChildrenInternal(transformer, value, ref stack);

            stack.Dispose();

            return result;
        }

        internal static T RewriteChildrenInternal<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value, ref ChunkStack<T> stack)
            => rewriter.WithChildren(
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
                ref stack
            );
    }
}
