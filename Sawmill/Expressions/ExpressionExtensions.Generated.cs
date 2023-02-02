#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Expressions
{
    /// <summary>
    /// Extension methods for <see cref="System.Linq.Expressions.Expression" />s.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static System.Linq.Expressions.Expression[] GetChildren(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this System.Linq.Expressions.Expression value, Span<System.Linq.Expressions.Expression> childrenReceiver)
            => ExpressionRewriter.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static System.Linq.Expressions.Expression SetChildren(this System.Linq.Expressions.Expression value, ReadOnlySpan<System.Linq.Expressions.Expression> newChildren)
            => ExpressionRewriter.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Linq.Expressions.Expression> DescendantsAndSelf(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Linq.Expressions.Expression> SelfAndDescendants(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<System.Linq.Expressions.Expression> SelfAndDescendantsBreadthFirst(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)[] ChildrenInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> SelfAndDescendantsInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> DescendantsAndSelfInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> SelfAndDescendantsInContextBreadthFirst(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static System.Linq.Expressions.Expression DescendantAt(this System.Linq.Expressions.Expression value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return ExpressionRewriter.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static System.Linq.Expressions.Expression ReplaceDescendantAt<T>(this System.Linq.Expressions.Expression value, IEnumerable<Direction> path, System.Linq.Expressions.Expression newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return ExpressionRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static System.Linq.Expressions.Expression RewriteDescendantAt<T>(this System.Linq.Expressions.Expression value, IEnumerable<Direction> path, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return ExpressionRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Linq.Expressions.Expression> RewriteDescendantAt<T>(this System.Linq.Expressions.Expression value, IEnumerable<Direction> path, Func<System.Linq.Expressions.Expression, ValueTask<System.Linq.Expressions.Expression>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return ExpressionRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<System.Linq.Expressions.Expression> Cursor(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this System.Linq.Expressions.Expression value, SpanFunc<T, System.Linq.Expressions.Expression, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return ExpressionRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this System.Linq.Expressions.Expression value, Func<Memory<T>, System.Linq.Expressions.Expression, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return ExpressionRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Linq.Expressions.Expression[] values, Func<System.Linq.Expressions.Expression[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return ExpressionRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Linq.Expressions.Expression[] values, Func<System.Linq.Expressions.Expression[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return ExpressionRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this System.Linq.Expressions.Expression value1, System.Linq.Expressions.Expression value2, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return ExpressionRewriter.Instance.ZipFold<System.Linq.Expressions.Expression, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this System.Linq.Expressions.Expression value1, System.Linq.Expressions.Expression value2, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return ExpressionRewriter.Instance.ZipFold<System.Linq.Expressions.Expression, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Linq.Expressions.Expression Rewrite(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Linq.Expressions.Expression> Rewrite(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, ValueTask<System.Linq.Expressions.Expression>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Linq.Expressions.Expression RewriteChildren(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Linq.Expressions.Expression> RewriteChildren(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, ValueTask<System.Linq.Expressions.Expression>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static System.Linq.Expressions.Expression RewriteIter(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<System.Linq.Expressions.Expression> RewriteIter(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, ValueTask<System.Linq.Expressions.Expression>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
