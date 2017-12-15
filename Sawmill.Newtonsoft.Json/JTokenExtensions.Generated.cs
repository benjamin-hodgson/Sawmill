#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for <see cref="global::Newtonsoft.Json.Linq.JToken"/>s.
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<global::Newtonsoft.Json.Linq.JToken> GetChildren(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static global::Newtonsoft.Json.Linq.JToken SetChildren(this global::Newtonsoft.Json.Linq.JToken value, Children<global::Newtonsoft.Json.Linq.JToken> newChildren)
            => JTokenRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> DescendantsAndSelf(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> DescendantsAndSelfLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendants(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendantsLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendantsBreadthFirst(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Newtonsoft.Json.Linq.JToken> SelfAndDescendantsBreadthFirstLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> ChildrenInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContextLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> DescendantsAndSelfInContext(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> DescendantsAndSelfInContextLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Newtonsoft.Json.Linq.JToken item, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> replace)> SelfAndDescendantsInContextBreadthFirstLazy(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContextBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Cursor{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Cursor<global::Newtonsoft.Json.Linq.JToken> Cursor(this global::Newtonsoft.Json.Linq.JToken value)
            => JTokenRewriter.Instance.Cursor(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return JTokenRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
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

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this global::Newtonsoft.Json.Linq.JToken value1, global::Newtonsoft.Json.Linq.JToken value2, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return JTokenRewriter.Instance.ZipFold<global::Newtonsoft.Json.Linq.JToken, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static global::Newtonsoft.Json.Linq.JToken Rewrite(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static global::Newtonsoft.Json.Linq.JToken RewriteChildren(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, global::Newtonsoft.Json.Linq.JToken> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static global::Newtonsoft.Json.Linq.JToken RewriteIter(this global::Newtonsoft.Json.Linq.JToken value, Func<global::Newtonsoft.Json.Linq.JToken, IterResult<global::Newtonsoft.Json.Linq.JToken>> transformer)
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
