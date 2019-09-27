using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields all of the nodes in the tree represented by <paramref name="value"/> in a breadth-first traversal order.
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to traverse</param>
        /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value"/> in a breadth-first traversal order.</returns>
        public static IEnumerable<T> SelfAndDescendantsBreadthFirst<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            IEnumerable<T> Iterator()
            {
                T[] buffer = null;
                var queue = new Queue<T>();
                queue.Enqueue(value);

                try
                {
                    while (queue.Any())
                    {
                        var node = queue.Dequeue();
                        
                        yield return node;

                        rewriter.WithChildren_(
                            (children, q) =>
                            {
                                foreach (var child in children)
                                {
                                    q.Enqueue(child);
                                }
                            },
                            queue,
                            node,
                            ref buffer
                        );
                    }
                }
                finally
                {
                    ArrayPool<T>.Shared.Return(buffer);
                }
            }

            return Iterator();
        }
    }
}
