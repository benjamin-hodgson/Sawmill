using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Yields each node in the tree represented by <paramref name="value" />
    /// paired with a function to replace the node, in a breadth-first traversal order.
    /// This is typically useful when you need to replace nodes one at a time,
    /// such as during mutation testing.
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// The replacement function can be seen as the "context" of the node; calling the
    /// function with a new node "plugs the hole" in the context.
    /// </para>
    ///
    /// <para>
    /// This is a breadth-first pre-order traversal.
    /// </para>
    /// </remarks>
    ///
    /// <seealso cref="SelfAndDescendants" />
    /// <seealso cref="ChildrenInContext" />
    /// <seealso cref="DescendantsAndSelfInContext" />
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="value">The value to get the contexts for the descendants.</param>
    /// <returns>An enumerable of contexts.</returns>
    [SuppressMessage("naming", "SA1316", Justification = "Breaking change")] // "Tuple element names should use correct casing"
    public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContextBreadthFirst<T>(this IRewriter<T> rewriter, T value)
    {
        ArgumentNullException.ThrowIfNull(rewriter);

        IEnumerable<(T item, Func<T, T> replace)> Iterator()
        {
            var q = new Queue<(T item, Func<T, T> replace)>();
            q.Enqueue((value, newValue => newValue));

            while (q.Count > 0)
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
