#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sawmill.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for <see cref="global::Newtonsoft.Json.Linq.JToken" />s.
    /// </summary>
    public static class JTokenExtensions
    {
        /// <inheritdoc cref="IRewriter{T}.CountChildren" />
        public static int CountChildren(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.CountChildren(value);

        /// <inheritdoc cref="Rewriter.GetChildren{T}(IRewriter{T}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken[] GetChildren(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.GetChildren(value);

        /// <inheritdoc cref="IRewriter{T}.GetChildren" />" />
        public static void GetChildren(this global::Newtonsoft.Json.Linq.JToken value, Span<global::Newtonsoft.Json.Linq.JToken> childrenReceiver)
            => JTokenRewriter.Instance.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref="IRewriter{T}.SetChildren" />" />
        public static global::Newtonsoft.Json.Linq.JToken SetChildren(this global::Newtonsoft.Json.Linq.JToken value, ReadOnlySpan<global::Newtonsoft.Json.Linq.JToken> newChildren)
            => JTokenRewriter.Instance.SetChildren(newChildren, value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> DescendantsAndSelf(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelf(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendants(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendants(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendantsBreadthFirst(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)" />
        public static (global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)[] ChildrenInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.ChildrenInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> DescendantsAndSelfInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)" />
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref="Rewriter.DescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken DescendantAt(this global::Newtonsoft.Json.Linq.JToken value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return JTokenRewriter.Instance.DescendantAt(path, value);
        }

        /// <inheritdoc cref="Rewriter.ReplaceDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, T, T)" />
        public static global::Newtonsoft.Json.Linq.JToken ReplaceDescendantAt<T>(this global::Newtonsoft.Json.Linq.JToken value, IEnumerable<Direction> path, global::Newtonsoft.Json.Linq.JToken newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return JTokenRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, T}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken RewriteDescendantAt<T>(this global::Newtonsoft.Json.Linq.JToken value, IEnumerable<Direction> path, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return JTokenRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteDescendantAt{T}(IRewriter{T}, IEnumerable{Direction}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Newtonsoft.Json.Linq.JToken> RewriteDescendantAt<T>(this global::Newtonsoft.Json.Linq.JToken value, IEnumerable<Direction> path, Func<global::Newtonsoft.Json.Linq.JToken, ValueTask<global::Newtonsoft.Json.Linq.JToken>> transformer)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            return JTokenRewriter.Instance.RewriteDescendantAt(path, transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Cursor{T}(IRewriter{T}, T)" />
        public static Cursor<global::Newtonsoft.Json.Linq.JToken> Cursor(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.Cursor(value);

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, SpanFunc{U, T, U})" />
        public static T Fold<T>(this global::Newtonsoft.Json.Linq.JToken value, SpanFunc<T, global::Newtonsoft.Json.Linq.JToken, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return JTokenRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewritable.Fold{T, U}(T, Func{Memory{U}, T, ValueTask{U}})" />
        public static ValueTask<T> Fold<T>(this global::Newtonsoft.Json.Linq.JToken value, Func<Memory<T>, global::Newtonsoft.Json.Linq.JToken, ValueTask<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return JTokenRewriter.Instance.Fold(func, value);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Newtonsoft.Json.Linq.JToken[] values, Func<global::Newtonsoft.Json.Linq.JToken[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return JTokenRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Newtonsoft.Json.Linq.JToken[] values, Func<global::Newtonsoft.Json.Linq.JToken[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return JTokenRewriter.Instance.ZipFold(func, values);
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])" />
        public static U ZipFold<U>(this global::Newtonsoft.Json.Linq.JToken value1, global::Newtonsoft.Json.Linq.JToken value2, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return JTokenRewriter.Instance.ZipFold<global::Newtonsoft.Json.Linq.JToken, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IAsyncEnumerable{U}, ValueTask{U}}, T[])" />
        public static ValueTask<U> ZipFold<U>(this global::Newtonsoft.Json.Linq.JToken value1, global::Newtonsoft.Json.Linq.JToken value2, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken, IAsyncEnumerable<U>, ValueTask<U>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return JTokenRewriter.Instance.ZipFold<global::Newtonsoft.Json.Linq.JToken, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken Rewrite(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Newtonsoft.Json.Linq.JToken> Rewrite(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, ValueTask<global::Newtonsoft.Json.Linq.JToken>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.Rewrite(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken RewriteChildren(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteChildren{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Newtonsoft.Json.Linq.JToken> RewriteChildren(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, ValueTask<global::Newtonsoft.Json.Linq.JToken>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteChildren(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)" />
        public static global::Newtonsoft.Json.Linq.JToken RewriteIter(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteIter(transformer, value);
        }

        /// <inheritdoc cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, ValueTask{T}}, T)" />
        public static ValueTask<global::Newtonsoft.Json.Linq.JToken> RewriteIter(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, ValueTask<global::Newtonsoft.Json.Linq.JToken>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion
