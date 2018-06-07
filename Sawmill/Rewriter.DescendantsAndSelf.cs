using System;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Yields all of the nodes in the tree represented by <paramref name="value"/>, starting at the bottom.
        /// <seealso cref="SelfAndDescendants"/>
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
        ///         new Lit(1),
        ///         new Lit(2),
        ///         new Add(new Lit(1), new Lit(2)),
        ///         new Lit(3),
        ///         expr    
        ///     };
        /// Assert.Equal(expected, rewriter.DescendantsAndSelf(expr));
        /// </code>
        /// </example>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to traverse</param>
        /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value"/>, starting at the bottom.</returns>
        public static IEnumerable<T> DescendantsAndSelf<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            IEnumerable<T> Iterator(T x)
            {
                foreach (var child in rewriter.GetChildren(x))
                {
                    foreach (var descendant in Iterator(child))
                    {
                        yield return descendant;
                    }
                }
                yield return x;
            }

            return Iterator(value);
        }
    }
}