#region GeneratedCode
using System;
using System.Collections.Generic;

namespace Sawmill.Xml
{
    /// <summary>
    /// Extension methods for <see cref="System.Xml.Linq.XElement"/>s.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<System.Xml.Linq.XElement> GetChildren(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static System.Xml.Linq.XElement SetChildren(this System.Xml.Linq.XElement value, Children<System.Xml.Linq.XElement> newChildren)
            => XElementRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.Linq.XElement> DescendantsAndSelf(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.Linq.XElement> DescendantsAndSelfLazy(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.Linq.XElement> SelfAndDescendants(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.Linq.XElement> SelfAndDescendantsLazy(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> ChildrenInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> SelfAndDescendantsInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> SelfAndDescendantsInContextLazy(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> DescendantsAndSelfInContext(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.Linq.XElement item, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> replace)> DescendantsAndSelfInContextLazy(this System.Xml.Linq.XElement value)
            => XElementRewriter.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XElementRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static System.Xml.Linq.XElement Rewrite(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static System.Xml.Linq.XElement RewriteChildren(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, System.Xml.Linq.XElement> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XElementRewriter.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static System.Xml.Linq.XElement RewriteIter(this System.Xml.Linq.XElement value, Func<System.Xml.Linq.XElement, IterResult<System.Xml.Linq.XElement>> transformer)
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
