using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Yields each node in the tree represented by <paramref name="value" />
    /// paired with a function to replace the node, starting at the top.
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
    /// This is a depth-first pre-order traversal.
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
    public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContext<T>(this IRewriter<T> rewriter, T value)
    {
        ArgumentNullException.ThrowIfNull(rewriter);

        IEnumerable<(T item, Func<T, T> context)> Go(T t)
        {
            yield return (t, newT => newT);

            foreach (var (child, replaceChild) in ChildrenInContext(rewriter, t))
            {
                foreach (var (descendant, replaceDescendant) in Go(child))
                {
                    yield return (descendant, newDescendant => replaceChild(replaceDescendant(newDescendant)));
                }
            }
        }

        return Go(value);
    }
}
