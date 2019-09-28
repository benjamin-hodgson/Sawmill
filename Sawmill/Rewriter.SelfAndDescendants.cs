using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields all of the nodes in the tree represented by <paramref name="value"/>, starting at the top.
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
                var chunkStack = new ChunkStack<T>();
                var stack = new Stack<T>();
                stack.Push(value);

                try
                {
                    while (stack.Count != 0)
                    {
                        var x = stack.Pop();
                        yield return x;

                        rewriter.WithChildren_(
                            (children, s) =>
                            {
                                for (var i = children.Length - 1; i >= 0; i--)
                                {
                                    s.Push(children[i]);
                                }
                            },
                            stack,
                            x,
                            ref chunkStack
                        );
                    }
                }
                finally
                {
                    chunkStack.Dispose();
                }
            }
            return Iterator();
        }
    }
}
