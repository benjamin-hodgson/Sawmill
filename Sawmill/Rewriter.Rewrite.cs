using System;
using System.Buffers;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Rebuild a tree by applying a transformation function to every node from bottom to top.
        /// </summary>
        /// <example>
        /// Given a representation of the expression <c>(1+2)+3</c>,
        /// <code>
        /// Expr expr = new Add(
        ///     new Add(
        ///         new Lit(1),
        ///         new Lit(2)
        ///     ),
        ///     new Lit(3)
        /// );
        /// </code>
        /// <see cref="Rewrite"/> replaces the leaves of the tree with the result of calling <paramref name="transformer"/>,
        /// then replaces their parents with the result of calling <paramref name="transformer"/>, and so on.
        /// By the end, <see cref="Rewrite"/> has traversed the whole tree.
        /// <code>
        /// Expr expected = transformer(new Add(
        ///     transformer(new Add(
        ///         transformer(new Lit(1)),
        ///         transformer(new Lit(2))
        ///     )),
        ///     transformer(new Lit(3))
        /// ));
        /// Assert.Equal(expected, rewriter.Rewrite(transformer, expr));
        /// </code>
        /// </example>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="transformer">The transformation function to apply to every node in the tree</param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        /// The result of applying <paramref name="transformer"/> to every node in the tree represented by <paramref name="value"/>.
        /// </returns>
        public static T Rewrite<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            // todo: can we use a shared stack for the intermediate children
            // so that RewriteChildren doesn't have to repeatedly rent arrays from the pool?
            Func<T, T> goDelegate = null;
            goDelegate = Go;
            T Go(T x) => transformer(rewriter.RewriteChildren(goDelegate, x));

            return Go(value);
        }
    }
}
