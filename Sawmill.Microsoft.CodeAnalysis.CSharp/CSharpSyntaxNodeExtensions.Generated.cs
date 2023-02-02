#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Microsoft.CodeAnalysis.CSharp
{
    /// <summary>
    /// Extension methods for <see cref="global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode" />s.
    /// </summary>
    public static class CSharpSyntaxNodeExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[] GetChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Span<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> childrenReceiver)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode SetChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, ReadOnlySpan<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> newChildren)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> DescendantsAndSelf(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendants(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendantsBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)[] ChildrenInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> DescendantsAndSelfInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode DescendantAt(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode ReplaceDescendantAt<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, IEnumerable<Direction> path, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteDescendantAt<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, IEnumerable<Direction> path, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> RewriteDescendantAt<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, IEnumerable<Direction> path, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> Cursor(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, SpanFunc<T, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<Memory<T>, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[] values, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[] values, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value1, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value1, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode Rewrite(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> Rewrite(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> RewriteChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteIter(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> RewriteIter(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, ValueTask<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
