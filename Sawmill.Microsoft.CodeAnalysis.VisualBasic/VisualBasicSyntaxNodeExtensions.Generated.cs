#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Microsoft.CodeAnalysis.VisualBasic
{
    /// <summary>
    /// Extension methods for <see cref="global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode" />s.
    /// </summary>
    public static class VisualBasicSyntaxNodeExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[] GetChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Span<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> childrenReceiver)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode SetChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, ReadOnlySpan<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> newChildren)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> DescendantsAndSelf(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendants(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendantsBreadthFirst(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)[] ChildrenInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> DescendantsAndSelfInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode DescendantAt(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode ReplaceDescendantAt<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, IEnumerable<Direction> path, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode RewriteDescendantAt<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, IEnumerable<Direction> path, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> RewriteDescendantAt<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, IEnumerable<Direction> path, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> Cursor(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, SpanFunc<T, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<Memory<T>, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[] values, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[] values, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value1, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ZipFold<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value1, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ZipFold<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode Rewrite(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> Rewrite(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode RewriteChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> RewriteChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode RewriteIter(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> RewriteIter(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
