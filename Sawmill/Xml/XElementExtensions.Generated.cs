#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Xml
{
    /// <summary>
    /// Extension methods for <see cref="System.Xml.Linq.XElement"/>s.
    /// </summary>
    public static class XElementExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static System.Xml.Linq.XElement[] GetChildren(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this System.Xml.Linq.XElement value, Span<System.Xml.Linq.XElement> childrenReceiver)
            => XElementRewriter.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static System.Xml.Linq.XElement SetChildren(this System.Xml.Linq.XElement value, ReadOnlySpan<System.Xml.Linq.XElement> newChildren)
            => XElementRewriter.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.Linq.XElement> DescendantsAndSelf(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.Linq.XElement> SelfAndDescendants(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.Linq.XElement> SelfAndDescendantsBreadthFirst(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)[] ChildrenInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> SelfAndDescendantsInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> DescendantsAndSelfInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> SelfAndDescendantsInContextBreadthFirst(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static System.Xml.Linq.XElement DescendantAt(this System.Xml.Linq.XElement value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return XElementRewriter.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static System.Xml.Linq.XElement ReplaceDescendantAt<T>(this System.Xml.Linq.XElement value, IEnumerable<Direction> path, System.Xml.Linq.XElement newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return XElementRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static System.Xml.Linq.XElement RewriteDescendantAt<T>(this System.Xml.Linq.XElement value, IEnumerable<Direction> path, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return XElementRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.Linq.XElement> RewriteDescendantAt<T>(this System.Xml.Linq.XElement value, IEnumerable<Direction> path, Func<System.Xml.Linq.XElement, ValueTask<System.Xml.Linq.XElement>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return XElementRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<System.Xml.Linq.XElement> Cursor(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this System.Xml.Linq.XElement value, SpanFunc<T, System.Xml.Linq.XElement, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XElementRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this System.Xml.Linq.XElement value, Func<Memory<T>, System.Xml.Linq.XElement, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XElementRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Xml.Linq.XElement[] values, Func<System.Xml.Linq.XElement[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XElementRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Xml.Linq.XElement[] values, Func<System.Xml.Linq.XElement[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XElementRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Xml.Linq.XElement value1, System.Xml.Linq.XElement value2, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XElementRewriter.Instance.ZipFold<System.Xml.Linq.XElement, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Xml.Linq.XElement value1, System.Xml.Linq.XElement value2, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XElementRewriter.Instance.ZipFold<System.Xml.Linq.XElement, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.Linq.XElement Rewrite(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.Linq.XElement> Rewrite(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, ValueTask<System.Xml.Linq.XElement>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.Linq.XElement RewriteChildren(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.Linq.XElement> RewriteChildren(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, ValueTask<System.Xml.Linq.XElement>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.Linq.XElement RewriteIter(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.Linq.XElement> RewriteIter(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, ValueTask<System.Xml.Linq.XElement>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
