using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Sawmill;

/// <summary>
/// Extension methods for <see cref="IRewritable{T}" /> implementations.
/// </summary>
public static class Rewritable
{
    /// <inheritdoc cref="Rewriter.GetChildren" />
    public static T[] GetChildren<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.GetChildren(value);

    /// <inheritdoc cref="Rewriter.DescendantsAndSelf" />
    public static IEnumerable<T> DescendantsAndSelf<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.DescendantsAndSelf(value);

    /// <inheritdoc cref="Rewriter.SelfAndDescendants" />
    public static IEnumerable<T> SelfAndDescendants<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.SelfAndDescendants(value);

    /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst" />
    public static IEnumerable<T> SelfAndDescendantsBreadthFirst<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.SelfAndDescendantsBreadthFirst(value);

    /// <inheritdoc cref="Rewriter.ChildrenInContext" />
    [SuppressMessage("naming", "SA1316", Justification = "Breaking change")] // "Tuple element names should use correct casing"
    public static (T item, Func<T, T> replace)[] ChildrenInContext<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.ChildrenInContext(value);

    /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext" />
    [SuppressMessage("naming", "SA1316", Justification = "Breaking change")] // "Tuple element names should use correct casing"
    public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContext<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.SelfAndDescendantsInContext(value);

    /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext" />
    [SuppressMessage("naming", "SA1316", Justification = "Breaking change")] // "Tuple element names should use correct casing"
    public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContext<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.DescendantsAndSelfInContext(value);

    /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst" />
    [SuppressMessage("naming", "SA1316", Justification = "Breaking change")] // "Tuple element names should use correct casing"
    public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContextBreadthFirst<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

    /// <inheritdoc cref="Rewriter.DescendantAt" />
    public static T DescendantAt<T>(this T value, IEnumerable<Direction> path)
        where T : IRewritable<T>
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        return RewritableRewriter<T>.Instance.DescendantAt(path, value);
    }

    /// <inheritdoc cref="Rewriter.ReplaceDescendantAt" />
    public static T ReplaceDescendantAt<T>(this T value, IEnumerable<Direction> path, T newDescendant)
        where T : IRewritable<T>
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        return RewritableRewriter<T>.Instance.ReplaceDescendantAt(path, newDescendant, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
    public static T RewriteDescendantAt<T>(this T value, IEnumerable<Direction> path, Func<T, T> transformer)
        where T : IRewritable<T>
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteDescendantAt(path, transformer, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
    public static ValueTask<T> RewriteDescendantAt<T>(this T value, IEnumerable<Direction> path, Func<T, ValueTask<T>> transformer)
        where T : IRewritable<T>
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteDescendantAt(path, transformer, value);
    }

    /// <inheritdoc cref="Rewriter.Cursor" />
    public static Cursor<T> Cursor<T>(this T value)
        where T : IRewritable<T>
        => RewritableRewriter<T>.Instance.Cursor(value);

    /// <inheritdoc cref="Rewriter.Fold{T, U}(IRewriter{T}, SpanFunc{U, T, U}, T)" />
    public static U Fold<T, U>(this T value, SpanFunc<U, T, U> func)
        where T : IRewritable<T>
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.Fold(func, value);
    }

    /// <inheritdoc cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{Memory{U}, T, ValueTask{U}}, T)" />
    public static ValueTask<U> Fold<T, U>(this T value, Func<Memory<U>, T, ValueTask<U>> func)
        where T : IRewritable<T>
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.Fold(func, value);
    }

    /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
    public static U ZipFold<T, U>(this T[] values, Func<T[], IEnumerable<U>, U> func)
        where T : IRewritable<T>
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.ZipFold(func, values);
    }

    /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
    public static ValueTask<U> ZipFold<T, U>(this T[] values, Func<T[], IAsyncEnumerable<U>, ValueTask<U>> func)
        where T : IRewritable<T>
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.ZipFold(func, values);
    }

    /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
    public static U ZipFold<T, U>(this T value1, T value2, Func<T, T, IEnumerable<U>, U> func)
        where T : IRewritable<T>
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.ZipFold<T, U>((xs, cs) => func(xs[0], xs[1], cs), value1, value2);
    }

    /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
    public static ValueTask<U> ZipFold<T, U>(this T value1, T value2, Func<T, T, IAsyncEnumerable<U>, ValueTask<U>> func)
        where T : IRewritable<T>
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return RewritableRewriter<T>.Instance.ZipFold<T, U>((xs, cs) => func(xs[0], xs[1], cs), value1, value2);
    }

    /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
    public static T Rewrite<T>(this T value, Func<T, T> transformer)
        where T : IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.Rewrite(transformer, value);
    }

    /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
    public static ValueTask<T> Rewrite<T>(this T value, Func<T, ValueTask<T>> transformer)
        where T : IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.Rewrite(transformer, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
    public static T RewriteChildren<T>(this T value, Func<T, T> transformer)
        where T : IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteChildren(transformer, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
    public static ValueTask<T> RewriteChildren<T>(this T value, Func<T, ValueTask<T>> transformer)
        where T : IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteChildren(transformer, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
    public static T RewriteIter<T>(this T value, Func<T, T> transformer)
        where T : class, IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteIter(transformer, value);
    }

    /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
    public static ValueTask<T> RewriteIter<T>(this T value, Func<T, ValueTask<T>> transformer)
        where T : class, IRewritable<T>
    {
        if (transformer == null)
        {
            throw new ArgumentNullException(nameof(transformer));
        }

        return RewritableRewriter<T>.Instance.RewriteIter(transformer, value);
    }
}
