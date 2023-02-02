#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.HtmlAgilityPack
{
    /// <summary>
    /// Extension methods for <see cref="global::HtmlAgilityPack.HtmlNode" />s.
    /// </summary>
    public static class HtmlNodeExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static global::HtmlAgilityPack.HtmlNode[] GetChildren(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this global::HtmlAgilityPack.HtmlNode value, Span<global::HtmlAgilityPack.HtmlNode> childrenReceiver)
            => HtmlNodeRewriter.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static global::HtmlAgilityPack.HtmlNode SetChildren(this global::HtmlAgilityPack.HtmlNode value, ReadOnlySpan<global::HtmlAgilityPack.HtmlNode> newChildren)
            => HtmlNodeRewriter.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::HtmlAgilityPack.HtmlNode> DescendantsAndSelf(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::HtmlAgilityPack.HtmlNode> SelfAndDescendants(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::HtmlAgilityPack.HtmlNode> SelfAndDescendantsBreadthFirst(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (global::HtmlAgilityPack.HtmlNode item, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> replace)[] ChildrenInContext(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::HtmlAgilityPack.HtmlNode item, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> replace)> SelfAndDescendantsInContext(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::HtmlAgilityPack.HtmlNode item, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> replace)> DescendantsAndSelfInContext(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::HtmlAgilityPack.HtmlNode item, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static global::HtmlAgilityPack.HtmlNode DescendantAt(this global::HtmlAgilityPack.HtmlNode value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return HtmlNodeRewriter.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static global::HtmlAgilityPack.HtmlNode ReplaceDescendantAt<T>(this global::HtmlAgilityPack.HtmlNode value, IEnumerable<Direction> path, global::HtmlAgilityPack.HtmlNode newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return HtmlNodeRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static global::HtmlAgilityPack.HtmlNode RewriteDescendantAt<T>(this global::HtmlAgilityPack.HtmlNode value, IEnumerable<Direction> path, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return HtmlNodeRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::HtmlAgilityPack.HtmlNode> RewriteDescendantAt<T>(this global::HtmlAgilityPack.HtmlNode value, IEnumerable<Direction> path, Func<global::HtmlAgilityPack.HtmlNode, ValueTask<global::HtmlAgilityPack.HtmlNode>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return HtmlNodeRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<global::HtmlAgilityPack.HtmlNode> Cursor(this global::HtmlAgilityPack.HtmlNode value)
            => HtmlNodeRewriter.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this global::HtmlAgilityPack.HtmlNode value, SpanFunc<T, global::HtmlAgilityPack.HtmlNode, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return HtmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this global::HtmlAgilityPack.HtmlNode value, Func<Memory<T>, global::HtmlAgilityPack.HtmlNode, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return HtmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::HtmlAgilityPack.HtmlNode[] values, Func<global::HtmlAgilityPack.HtmlNode[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return HtmlNodeRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::HtmlAgilityPack.HtmlNode[] values, Func<global::HtmlAgilityPack.HtmlNode[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return HtmlNodeRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::HtmlAgilityPack.HtmlNode value1, global::HtmlAgilityPack.HtmlNode value2, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return HtmlNodeRewriter.Instance.ZipFold<global::HtmlAgilityPack.HtmlNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::HtmlAgilityPack.HtmlNode value1, global::HtmlAgilityPack.HtmlNode value2, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return HtmlNodeRewriter.Instance.ZipFold<global::HtmlAgilityPack.HtmlNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::HtmlAgilityPack.HtmlNode Rewrite(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::HtmlAgilityPack.HtmlNode> Rewrite(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, ValueTask<global::HtmlAgilityPack.HtmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::HtmlAgilityPack.HtmlNode RewriteChildren(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::HtmlAgilityPack.HtmlNode> RewriteChildren(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, ValueTask<global::HtmlAgilityPack.HtmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::HtmlAgilityPack.HtmlNode RewriteIter(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, global::HtmlAgilityPack.HtmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::HtmlAgilityPack.HtmlNode> RewriteIter(this global::HtmlAgilityPack.HtmlNode value, Func<global::HtmlAgilityPack.HtmlNode, ValueTask<global::HtmlAgilityPack.HtmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return HtmlNodeRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
