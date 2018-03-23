#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill.Xml
{
    /// <summary>
    /// Extension methods for <see cref="System.Xml.XmlNode"/>s.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<System.Xml.XmlNode> GetChildren(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static System.Xml.XmlNode SetChildren(this System.Xml.XmlNode value, Children<System.Xml.XmlNode> newChildren)
            => XmlNodeRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.XmlNode> DescendantsAndSelf(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.XmlNode> SelfAndDescendants(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendants(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Xml.XmlNode> SelfAndDescendantsBreadthFirst(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> ChildrenInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> SelfAndDescendantsInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> DescendantsAndSelfInContext(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Xml.XmlNode item, Func<System.Xml.XmlNode, System.Xml.XmlNode> replace)> SelfAndDescendantsInContextBreadthFirst(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.SelfAndDescendantsInContextBreadthFirst(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantAt"/>
        /// </summary>
        public static System.Xml.XmlNode DescendantAt(this System.Xml.XmlNode value, IEnumerable<Direction> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            return XmlNodeRewriter.Instance.DescendantAt(path, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ReplaceDescendantAt"/>
        /// </summary>
        public static System.Xml.XmlNode ReplaceDescendantAt<T>(this System.Xml.XmlNode value, IEnumerable<Direction> path, System.Xml.XmlNode newDescendant)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            return XmlNodeRewriter.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.RewriteDescendantAt"/>
        /// </summary>
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

        /// <summary>
        /// <seealso cref="Rewriter.Cursor{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Cursor<System.Xml.XmlNode> Cursor(this System.Xml.XmlNode value)
            => XmlNodeRewriter.Instance.Cursor(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return XmlNodeRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
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

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this System.Xml.XmlNode value1, System.Xml.XmlNode value2, Func<System.Xml.XmlNode, System.Xml.XmlNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return XmlNodeRewriter.Instance.ZipFold<System.Xml.XmlNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static System.Xml.XmlNode Rewrite(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
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
        public static System.Xml.XmlNode RewriteChildren(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return XmlNodeRewriter.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static System.Xml.XmlNode RewriteIter(this System.Xml.XmlNode value, Func<System.Xml.XmlNode, System.Xml.XmlNode> transformer)
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
