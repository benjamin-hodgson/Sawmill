using System;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields each node in the tree represented by <paramref name="value"/>
        /// paired with a function to replace the node, in a breadth-first traversal order.
        /// This is typically useful when you need to replace nodes one at a time,
        /// such as during mutation testing.
        /// 
        /// <para>
        /// The replacement function can be seen as the "context" of the node; calling the
        /// function with a new node "plugs the hole" in the context.
        /// </para>
        /// 
        /// <seealso cref="SelfAndDescendants"/>
        /// <seealso cref="ChildrenInContext"/>
        /// <seealso cref="DescendantsAndSelfInContext"/>
        /// </summary>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to get the contexts for the descendants</param>
        public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContextBreadthFirst<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            IEnumerable<(T item, Func<T, T> replace)> Iterator()
            {
                var q = new Queue<(T item, Func<T, T> replace)>();
                q.Enqueue((value, newValue => newValue));

                while (q.Any())
                {
                    var (item, replace) = q.Dequeue();
                    
                    yield return (item, replace);

                    foreach (var (child, replaceChild) in rewriter.ChildrenInContext(item))
                    {
                        q.Enqueue((child, newChild => replace(replaceChild(newChild))));
                    }
                }
            }

            return Iterator();
        }
    }
}