using System;
using System.Threading.Tasks;

namespace Sawmill;

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
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="transformer">
    /// A transformation function to apply to every node in <paramref name="value"/> repeatedly.
    /// </param>
    /// <param name="value">The value to rewrite.</param>
    /// <returns>
    /// The result of applying <paramref name="transformer"/> to every node in the tree
    /// represented by <paramref name="value"/> repeatedly until a fixed point is reached.
    /// </returns>
    public static T RewriteIter<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value)
        where T : class
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        using var traversal = new RewriteIterTraversal<T>(rewriter, transformer);
        return traversal.Go(value);
    }

    private class RewriteIterTraversal<T> : RewriteTraversal<T>
    {
        public RewriteIterTraversal(IRewriter<T> rewriter, Func<T, T> transformer)
            : base(rewriter, transformer)
        {
        }

        protected override T Transform(T value)
        {
            var newValue = Transformer(value);
            if (!ReferenceEquals(value, newValue))
            {
                return Go(newValue);
            }

            return value;
        }
    }

    /// <summary>
    /// Rebuild a tree by repeatedly applying an asynchronous transformation function to every node in the tree,
    /// until a fixed point is reached. <paramref name="transformer"/> should always eventually return
    /// its argument unchanged, or this method will loop.
    /// That is, <c>x.RewriteIter(transformer).SelfAndDescendants().All(x => await transformer(x) == x)</c>.
    /// <para>
    /// This is typically useful when you want to put your tree into a normal form
    /// by applying a collection of rewrite rules until none of them can fire any more.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="transformer">
    /// An asynchronous transformation function to apply to every node in <paramref name="value"/> repeatedly.
    /// </param>
    /// <param name="value">The value to rewrite.</param>
    /// <returns>
    /// The result of applying <paramref name="transformer"/> to every node in the tree
    /// represented by <paramref name="value"/> repeatedly until a fixed point is reached.
    /// </returns>
    /// <remarks>This method is not available on platforms which do not support <see cref="ValueTask"/>.</remarks>
    public static async ValueTask<T> RewriteIter<T>(
        this IRewriter<T> rewriter,
        Func<T, ValueTask<T>> transformer,
        T value
    )
        where T : class
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        using var traversal = new RewriteIterAsyncTraversal<T>(rewriter, transformer);
        return await traversal.Go(value).ConfigureAwait(false);
    }

    private class RewriteIterAsyncTraversal<T> : RewriteAsyncTraversal<T>
    {
        public RewriteIterAsyncTraversal(IRewriter<T> rewriter, Func<T, ValueTask<T>> transformer)
            : base(rewriter, transformer)
        {
        }

        protected override async ValueTask<T> Transform(T value)
        {
            var newValue = await Transformer(value).ConfigureAwait(false);
            if (!ReferenceEquals(value, newValue))
            {
                return await Go(newValue).ConfigureAwait(false);
            }

            return value;
        }
    }
}
