using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for <see cref="JToken"/>s.
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<JToken> GetChildren(this JToken value)
            => JTokenRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static JToken SetChildren(this JToken value, Children<JToken> newChildren)
            => JTokenRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<JToken> DescendantsAndSelf(this JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<JToken> DescendantsAndSelfLazy(this JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<JToken> SelfAndDescendants(this JToken value)
            => JTokenRewriter.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<JToken> SelfAndDescendantsLazy(this JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(JToken item, Func<JToken, JToken> replace)> ChildrenInContext(this JToken value)
            => JTokenRewriter.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(JToken item, Func<JToken, JToken> replace)> SelfAndDescendantsInContext(this JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(JToken item, Func<JToken, JToken> replace)> SelfAndDescendantsInContextLazy(this JToken value)
            => JTokenRewriter.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(JToken item, Func<JToken, JToken> replace)> DescendantsAndSelfInContext(this JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(JToken item, Func<JToken, JToken> replace)> DescendantsAndSelfInContextLazy(this JToken value)
            => JTokenRewriter.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this JToken value, Func<JToken, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return JTokenRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static JToken Rewrite(this JToken value, Func<JToken, JToken> transformer)
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
        public static JToken RewriteChildren(this JToken value, Func<JToken, JToken> transformer)
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
        public static JToken RewriteIter(this JToken value, Func<JToken, IterResult<JToken>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return JTokenRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}