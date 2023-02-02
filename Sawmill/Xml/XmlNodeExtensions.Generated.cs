#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Xml
{
    /// <summary>
    /// Extension methods for <see cref="System.Xml.XmlNode" />s.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static System.Xml.XmlNode[] GetChildren(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this System.Xml.XmlNode value, Span<System.Xml.XmlNode> childrenReceiver)
            => XmlNodeRewriter.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static System.Xml.XmlNode SetChildren(this System.Xml.XmlNode value, ReadOnlySpan<System.Xml.XmlNode> newChildren)
            => XmlNodeRewriter.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.XmlNode> DescendantsAndSelf(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.XmlNode> SelfAndDescendants(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Xml.XmlNode> SelfAndDescendantsBreadthFirst(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)[] ChildrenInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> SelfAndDescendantsInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> DescendantsAndSelfInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> SelfAndDescendantsInContextBreadthFirst(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static System.Xml.XmlNode DescendantAt(this System.Xml.XmlNode value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return XmlNodeRewriter.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static System.Xml.XmlNode ReplaceDescendantAt<T>(this System.Xml.XmlNode value, IEnumerable<Direction> path, System.Xml.XmlNode newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return XmlNodeRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static System.Xml.XmlNode RewriteDescendantAt<T>(this System.Xml.XmlNode value, IEnumerable<Direction> path, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return XmlNodeRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.XmlNode> RewriteDescendantAt<T>(this System.Xml.XmlNode value, IEnumerable<Direction> path, Func<System.Xml.XmlNode, ValueTask<System.Xml.XmlNode>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return XmlNodeRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<System.Xml.XmlNode> Cursor(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this System.Xml.XmlNode value, SpanFunc<T, System.Xml.XmlNode, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this System.Xml.XmlNode value, Func<Memory<T>, System.Xml.XmlNode, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Xml.XmlNode[] values, Func<System.Xml.XmlNode[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XmlNodeRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Xml.XmlNode[] values, Func<System.Xml.XmlNode[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XmlNodeRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Xml.XmlNode value1, System.Xml.XmlNode value2, Func<System.Xml.XmlNode, System.Xml.XmlNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XmlNodeRewriter.Instance.ZipFold<System.Xml.XmlNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Xml.XmlNode value1, System.Xml.XmlNode value2, Func<System.Xml.XmlNode, System.Xml.XmlNode, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XmlNodeRewriter.Instance.ZipFold<System.Xml.XmlNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.XmlNode Rewrite(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.XmlNode> Rewrite(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, ValueTask<System.Xml.XmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.XmlNode RewriteChildren(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.XmlNode> RewriteChildren(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, ValueTask<System.Xml.XmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Xml.XmlNode RewriteIter(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Xml.XmlNode> RewriteIter(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, ValueTask<System.Xml.XmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
