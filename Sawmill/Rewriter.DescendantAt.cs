using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Returns the descendant at a particular location in <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="path">The route to take to find the descendant.</param>
    /// <param name="value">The rewritable tree.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="path" /> leads off the edge of the tree.
    /// </exception>
    /// <returns>The descendant found by following the directions in <paramref name="path" />.</returns>
    public static T DescendantAt<T>(this IRewriter<T> rewriter, IEnumerable<Direction> path, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var cursor = rewriter.Cursor(value);
        cursor.Follow(path);
        return cursor.Focus;
    }

    /// <summary>
    /// Replaces the descendant at a particular location in <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="path">The route to take to find the descendant.</param>
    /// <param name="newDescendant">The replacement descendant.</param>
    /// <param name="value">The rewritable tree.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="path" /> leads off the edge of the tree.
    /// </exception>
    /// <returns>
    /// A copy of <paramref name="value" /> with <paramref name="newDescendant" /> placed at the location indicated by <paramref name="path" />.
    /// </returns>
    public static T ReplaceDescendantAt<T>(this IRewriter<T> rewriter, IEnumerable<Direction> path, T newDescendant, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var cursor = rewriter.Cursor(value);
        cursor.Follow(path);
        cursor.Focus = newDescendant;
        cursor.Top();
        return cursor.Focus;
    }

    /// <summary>
    /// Apply a function at a particular location in <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="path">The route to take to find the descendant.</param>
    /// <param name="transformer">A function to calculate a replacement for the descendant.</param>
    /// <param name="value">The rewritable tree.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="path" /> leads off the edge of the tree.
    /// </exception>
    /// <returns>
    /// A copy of <paramref name="value" /> with the result of <paramref name="transformer" /> placed at the location indicated by <paramref name="path" />.
    /// </returns>
    public static T RewriteDescendantAt<T>(this IRewriter<T> rewriter, IEnumerable<Direction> path, Func<T, T> transformer, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        var cursor = rewriter.Cursor(value);
        cursor.Follow(path);
        cursor.Focus = transformer(cursor.Focus);
        cursor.Top();
        return cursor.Focus;
    }

    /// <summary>
    /// Apply an asynchronous function at a particular location in <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="path">The route to take to find the descendant.</param>
    /// <param name="transformer">An asynchronous function to calculate a replacement for the descendant.</param>
    /// <param name="value">The rewritable tree.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="path" /> leads off the edge of the tree.
    /// </exception>
    /// <returns>
    /// A copy of <paramref name="value" /> with the result of <paramref name="transformer" /> placed at the location indicated by <paramref name="path" />.
    /// </returns>
    /// <remarks>This method is not available on platforms which do not support <see cref="ValueTask" />.</remarks>
    public static async ValueTask<T> RewriteDescendantAt<T>(this IRewriter<T> rewriter, IEnumerable<Direction> path, Func<T, ValueTask<T>> transformer, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        var cursor = rewriter.Cursor(value);
        cursor.Follow(path);
        cursor.Focus = await transformer(cursor.Focus).ConfigureAwait(false);
        cursor.Top();
        return cursor.Focus;
    }
}
