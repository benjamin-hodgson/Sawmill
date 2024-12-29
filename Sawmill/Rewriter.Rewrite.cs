using System;
using System.Threading.Tasks;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Rebuild a tree by applying a transformation function to every node from bottom to top.
    /// </summary>
    /// <example>
    /// Given a representation of the expression <c>(1+2)+3</c>.
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// </code>
    /// <see cref="Rewrite{T}(IRewriter{T}, Func{T, T}, T)" /> replaces the leaves of the tree with the result of calling <paramref name="transformer" />,
    /// then replaces their parents with the result of calling <paramref name="transformer" />, and so on.
    /// By the end, <see cref="Rewrite{T}(IRewriter{T}, Func{T, T}, T)" /> has traversed the whole tree.
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
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="transformer">The transformation function to apply to every node in the tree.</param>
    /// <param name="value">The value to rewrite.</param>
    /// <returns>
    /// The result of applying <paramref name="transformer" /> to every node in the tree represented by <paramref name="value" />.
    /// </returns>
    public static T Rewrite<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value)
    {
        ArgumentNullException.ThrowIfNull(nameof(rewriter));
        ArgumentNullException.ThrowIfNull(nameof(transformer));

        using var traversal = new RewriteTraversal<T>(rewriter, transformer);
        return traversal.Go(value);
    }

    private class RewriteTraversal<T> : Traversal<T>
    {
        protected Func<T, T> Transformer { get; }

        public RewriteTraversal(IRewriter<T> rewriter, Func<T, T> transformer)
            : base(rewriter)
        {
            Transformer = transformer;
        }

        public T Go(T value)
            => Transform(RewriteChildren(Go, value));

        protected virtual T Transform(T value)
            => Transformer(value);
    }

    /// <summary>
    /// Rebuild a tree by applying an asynchronous transformation function to every node from bottom to top.
    /// </summary>
    /// <example>
    /// Given a representation of the expression <c>(1+2)+3</c>.
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// </code>
    /// <see cref="Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" /> replaces the leaves of the tree with the result of calling <paramref name="transformer" />,
    /// then replaces their parents with the result of calling <paramref name="transformer" />, and so on.
    /// By the end, <see cref="Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" /> has traversed the whole tree.
    /// <code>
    /// Expr expected = await transformer(new Add(
    ///     await transformer(new Add(
    ///         await transformer(new Lit(1)),
    ///         await transformer(new Lit(2))
    ///     )),
    ///     await transformer(new Lit(3))
    /// ));
    /// Assert.Equal(expected, await rewriter.Rewrite(transformer, expr));
    /// </code>
    /// </example>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="transformer">The asynchronous transformation function to apply to every node in the tree.</param>
    /// <param name="value">The value to rewrite.</param>
    /// <returns>
    /// The result of applying <paramref name="transformer" /> to every node in the tree represented by <paramref name="value" />.
    /// </returns>
    /// <remarks>This method is not available on platforms which do not support <see cref="ValueTask" />.</remarks>
    public static async ValueTask<T> Rewrite<T>(this IRewriter<T> rewriter, Func<T, ValueTask<T>> transformer, T value)
    {
        ArgumentNullException.ThrowIfNull(nameof(rewriter));
        ArgumentNullException.ThrowIfNull(nameof(transformer));

        using var traversal = new RewriteAsyncTraversal<T>(rewriter, transformer);
        return await traversal.Go(value).ConfigureAwait(false);
    }

    private class RewriteAsyncTraversal<T> : AsyncTraversal<T>
    {
        protected Func<T, ValueTask<T>> Transformer { get; }

        public RewriteAsyncTraversal(IRewriter<T> rewriter, Func<T, ValueTask<T>> transformer)
            : base(rewriter)
        {
            Transformer = transformer;
        }

        public async ValueTask<T> Go(T value)
            => await Transform(
                await RewriteChildren(Go, value).ConfigureAwait(false)
            ).ConfigureAwait(false);

        protected virtual ValueTask<T> Transform(T value)
            => Transformer(value);
    }
}
