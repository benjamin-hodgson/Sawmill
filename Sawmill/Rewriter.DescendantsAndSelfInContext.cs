using System;
using System.Collections.Generic;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields each node in the tree represented by <paramref name="value"/>
        /// paired with a function to replace the node, starting at the bottom.
        /// This is typically useful when you need to replace nodes one at a time,
        /// such as during mutation testing.
        /// 
        /// <para>
        /// The replacement function can be seen as the "context" of the node; calling the
        /// function with a new node "plugs the hole" in the context.
        /// </para>
        /// 
        /// <seealso cref="DescendantsAndSelf"/>
        /// <seealso cref="ChildrenInContext"/>
        /// <seealso cref="SelfAndDescendantsInContext"/>
        /// <seealso cref="SelfAndDescendantsInContextLazy"/>
        /// <seealso cref="DescendantsAndSelfInContextLazy"/>
        /// </summary>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to get the contexts for the descendants</param>
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContext<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            var result = new List<(T item, Func<T, T> replace)>();

            void Go(T t, Func<T, T> replace)
            {
                foreach (var (child, replaceChild) in rewriter.ChildrenInContext(t))
                {
                    Go(child, newDescendant => replace(replaceChild(newDescendant)));
                }

                result.Add((t, replace));                
            }
            Go(value, x => x);
            return result;
        }

        /// <summary>
        /// Lazily yields each node in the tree represented by <paramref name="value"/>
        /// paired with a function to replace the node, starting at the bottom.
        /// 
        /// <para>
        /// <see cref="DescendantsAndSelfInContext"/> will typically be faster than this method for small trees,
        /// but the lazy version can be more efficient when you only need to query part of the tree.
        /// </para>
        /// 
        /// <seealso cref="DescendantsAndSelf"/>
        /// <seealso cref="ChildrenInContext"/>
        /// <seealso cref="SelfAndDescendantsInContext"/>
        /// <seealso cref="SelfAndDescendantsInContextLazy"/>
        /// <seealso cref="DescendantsAndSelfInContext"/>
        /// </summary>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to get the contexts for the descendants</param>
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContextLazy<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            IEnumerable<(T item, Func<T, T> context)> Go(T t)
            {
                foreach (var (child, replaceChild) in ChildrenInContext(rewriter, t))
                {
                    foreach (var (descendant, replaceDescendant) in Go(child))
                    {
                        yield return (descendant, newDescendant => replaceChild(replaceDescendant(newDescendant)));
                    }
                }
                
                yield return (t, newT => newT);                
            }
            return Go(value);
        }
    }
}