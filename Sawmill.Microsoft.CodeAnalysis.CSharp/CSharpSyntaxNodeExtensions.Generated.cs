#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill.Microsoft.CodeAnalysis.CSharp
{
    /// <summary>
    /// Extension methods for <see cref="global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode"/>s.
    /// </summary>
    public static class CSharpSyntaxNodeExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> GetChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode SetChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Children<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> newChildren)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> DescendantsAndSelf(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> DescendantsAndSelfLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendants(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendantsLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendantsBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendantsBreadthFirstLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> ChildrenInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContextLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> DescendantsAndSelfInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> DescendantsAndSelfInContextLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirstLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirstLazy(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Cursor{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Cursor<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> Cursor(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Cursor(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{ImmutableArray{T}, IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value1, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value2, Func<ImmutableArray<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold(func, value1, value2);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode Rewrite(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteChildren(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteIter(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, IterResult<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>> transformer)
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
