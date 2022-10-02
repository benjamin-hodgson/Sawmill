using System;
using System.Collections.Generic;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Yields all of the nodes in the tree represented by <paramref name="value"/>, starting at the top.
    /// 
    /// <para>
    /// This is a depth-first pre-order traversal.
    /// </para>
    /// 
    /// <seealso cref="DescendantsAndSelf"/>
    /// </summary>
    /// <example>
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// Expr[] expected = new[]
    ///     {
    ///         expr,
    ///         new Add(new Lit(1), new Lit(2)),
    ///         new Lit(1),
    ///         new Lit(2),
    ///         new Lit(3),
    ///     };
    /// Assert.Equal(expected, rewriter.SelfAndDescendants(expr));
    /// </code>
    /// </example>
    /// <typeparam name="T">The rewritable tree type</typeparam>
    /// <param name="rewriter">The rewriter</param>
    /// <param name="value">The value to traverse</param>
    /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value"/>, starting at the top.</returns>
    public static IEnumerable<T> SelfAndDescendants<T>(this IRewriter<T> rewriter, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        IEnumerable<T> Iterator()
        {
            var stack = new ChunkStack<T>();
            stack.Allocate(1)[0] = value;

            try
            {
                while (!stack.IsEmpty)
                {
                    var x = stack.Pop();
                    yield return x;

                    var count = rewriter.CountChildren(x);
                    var span = stack.Allocate(count);
                    rewriter.GetChildren(span, x);
                    span.Reverse();  // pop them in left to right order
                }
            }
            finally
            {
                stack.Dispose();
            }
        }
        return Iterator();
    }
}
