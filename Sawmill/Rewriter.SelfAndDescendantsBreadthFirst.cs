using System;
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
                var q = new Queue<T>();
                q.Enqueue(value);

                while (q.Any())
                {
                    var node = q.Dequeue();
                    
                    yield return node;

                    foreach (var child in rewriter.GetChildren(node))
                    {
                        q.Enqueue(child);
                    }
                }
            }

            return Iterator();
        }
    }
}