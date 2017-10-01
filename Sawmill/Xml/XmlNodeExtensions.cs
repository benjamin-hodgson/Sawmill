using System;
using System.Collections.Generic;
using System.Xml;

namespace Sawmill.Xml
{
    /// <summary>
    /// Extension methods for <see cref="XmlNode"/>s.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<XmlNode> GetChildren(this XmlNode value)
            => XmlNodeRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static XmlNode SetChildren(this XmlNode value, Children<XmlNode> newChildren)
            => XmlNodeRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<XmlNode> DescendantsAndSelf(this XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<XmlNode> DescendantsAndSelfLazy(this XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<XmlNode> SelfAndDescendants(this XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<XmlNode> SelfAndDescendantsLazy(this XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this XmlNode value, Func<XmlNode, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static XmlNode Rewrite(this XmlNode value, Func<XmlNode, XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static XmlNode RewriteChildren(this XmlNode value, Func<XmlNode, XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static XmlNode RewriteIter(this XmlNode value, Func<XmlNode, IterResult<XmlNode>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}