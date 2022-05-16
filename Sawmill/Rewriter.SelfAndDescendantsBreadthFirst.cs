using System;
using System.Collections.Generic;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields all of the nodes in the tree represented by <paramref name="value"/> in a breadth-first traversal order.
        /// 
        /// <para>
        /// This is a breadth-first pre-order traversal.
        /// </para>
        /// 
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
                var queue = new PooledQueue<T>();
                queue.AllocateRight(1)[0] = value;

                try
                {
                    while (queue.Count != 0)
                    {
                        var x = queue.PopLeft();

                        yield return x;

                        var count = rewriter.CountChildren(x);
                        var span = queue.AllocateRight(count);
                        rewriter.GetChildren(span, x);
                    }
                }
                finally
                {
                    queue.Dispose();
                }
            }

            return Iterator();
        }
    }
}
