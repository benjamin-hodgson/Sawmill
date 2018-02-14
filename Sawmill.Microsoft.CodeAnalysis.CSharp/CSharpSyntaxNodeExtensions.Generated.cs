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
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendants(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendants(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> SelfAndDescendantsBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsBreadthFirst(value);

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
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> DescendantsAndSelfInContext(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextBreadthFirst{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode item, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> replace)> SelfAndDescendantsInContextBreadthFirst(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value)
            => SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.SelfAndDescendantsInContextBreadthFirst(value);

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
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[] values, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode[], IEnumerable<U>, U> func)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold(func, values);
        }

        /// <summary>
        /// <seealso cref="Rewriter.ZipFold{T, U}(IRewriter{T}, Func{T[], IEnumerable{U}, U}, T[])"/>
        /// </summary>
        public static U ZipFold<U>(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value1, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value2, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, IEnumerable<U>, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance.ZipFold<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, U>((xs, cs) => func(xs[0], xs[1], cs), new[] { value1, value2 });
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
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode RewriteIter(this global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode value, Func<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode, global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode> transformer)
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
