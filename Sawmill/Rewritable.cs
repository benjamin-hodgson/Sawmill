using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// Extension methods for <see cref="IRewritable{T}"/> implementations.
    /// </summary>
    public static class Rewritable
    {
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<T> DescendantsAndSelf<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<T> DescendantsAndSelfLazy<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelfLazy(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<T> SelfAndDescendants<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<T> SelfAndDescendantsLazy<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(T item, Func<T, T> replace)> ChildrenInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContextLazy<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContextLazy<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelfInContextLazy(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static U Fold<T, U>(this T value, Func<T, Children<U>, U> func) where T : IRewritable<T>
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.Fold(func, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static T Rewrite<T>(this T value, Func<T, T> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.DefaultRewriteChildren{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static T DefaultRewriteChildren<T>(this T value, Func<T, T> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.DefaultRewriteChildren(transformer, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static T RewriteIter<T>(this T value, Func<T, IterResult<T>> transformer) where T : class, IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.RewriteIter(transformer, value);
        }
    }
}