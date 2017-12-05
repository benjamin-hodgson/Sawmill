#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill.Microsoft.CodeAnalysis.VisualBasic
{
    /// <summary>
    /// Extension methods for <see cref="global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode"/>s.
    /// </summary>
    public static class VisualBasicSyntaxNodeExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> GetChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode SetChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Children<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> newChildren)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> DescendantsAndSelf(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> DescendantsAndSelfLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendants(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendantsLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendantsBreadthFirst(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> SelfAndDescendantsBreadthFirstLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> ChildrenInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContextLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> DescendantsAndSelfInContext(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> DescendantsAndSelfInContextLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode item, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirstLazy(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Cursor{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Cursor<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> Cursor(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Cursor(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value1, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode[], IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.ZipFold(func, value1, value2);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode Rewrite(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode RewriteChildren(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode RewriteIter(this global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode value, Func<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode, IterResult<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>> transformer)
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
