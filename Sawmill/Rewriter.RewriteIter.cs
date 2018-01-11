using System;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Rebuild a tree by repeatedly applying a transformation function to every node in the tree,
        /// until a fixed point is reached. <paramref name="transformer"/> should always eventually return
        /// its argument unchanged, or this method will loop.
        /// That is, <c>x.RewriteIter(transformer).SelfAndDescendants().All(x => transformer(x) == x)</c>.
        /// <para>
        /// This is typically useful when you want to put your tree into a normal form
        /// by applying a collection of rewrite rules until none of them can fire any more.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="transformer">
        /// A transformation function to apply to every node in <paramref name="value"/> repeatedly.
        /// </param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        /// The result of applying <paramref name="transformer"/> to every node in the tree
        /// represented by <paramref name="value"/> repeatedly until a fixed point is reached.
        /// </returns>
        public static T RewriteIter<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value) where T : class
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            Func<T, T> transformerDelegate = null;
            transformerDelegate = Transformer;
            T Transformer(T x)
            {
                var newX = transformer(x);
                if (!ReferenceEquals(x, newX))
                {
                    return Go(newX);
                }
                return x;
            }
            T Go(T x) => rewriter.Rewrite(transformerDelegate, x);
            return Go(value);
        }
    }
}