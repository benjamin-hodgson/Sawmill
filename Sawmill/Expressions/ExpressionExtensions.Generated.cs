#region GeneratedCode
using System;
using System.Collections.Generic;

namespace Sawmill.Expressions
{
    /// <summary>
    /// Extension methods for <see cref="System.Linq.Expressions.Expression"/>s.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public static Children<System.Linq.Expressions.Expression> GetChildren(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.GetChildren(value);

        /// <summary>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public static System.Linq.Expressions.Expression SetChildren(this System.Linq.Expressions.Expression value, Children<System.Linq.Expressions.Expression> newChildren)
            => ExpressionRewriter.Instance.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelf{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Linq.Expressions.Expression> DescendantsAndSelf(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Linq.Expressions.Expression> DescendantsAndSelfLazy(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Linq.Expressions.Expression> SelfAndDescendants(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<System.Linq.Expressions.Expression> SelfAndDescendantsLazy(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.ChildrenInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static Children<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> ChildrenInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> SelfAndDescendantsInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.SelfAndDescendantsInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> SelfAndDescendantsInContextLazy(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContext{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> DescendantsAndSelfInContext(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref="Rewriter.DescendantsAndSelfInContextLazy{T}(IRewriter{T}, T)"/>
        /// </summary>
        public static IEnumerable<(System.Linq.Expressions.Expression item, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> replace)> DescendantsAndSelfInContextLazy(this System.Linq.Expressions.Expression value)
            => ExpressionRewriter.Instance.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref="Rewriter.Fold{T, U}(IRewriter{T}, Func{T, Children{U}, U}, T)"/>
        /// </summary>
        public static T Fold<T>(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, Children<T>, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return ExpressionRewriter.Instance.Fold(func, value);
        }

        /// <summary>
        /// <seealso cref="Rewriter.Rewrite{T}(IRewriter{T}, Func{T, T}, T)"/>
        /// </summary>
        public static System.Linq.Expressions.Expression Rewrite(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.Rewrite(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public static System.Linq.Expressions.Expression RewriteChildren(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteChildren(transformer, value);
        }
        
        /// <summary>
        /// <seealso cref="Rewriter.RewriteIter{T}(IRewriter{T}, Func{T, IterResult{T}}, T)"/>
        /// </summary>
        public static System.Linq.Expressions.Expression RewriteIter(this System.Linq.Expressions.Expression value, Func<System.Linq.Expressions.Expression, IterResult<System.Linq.Expressions.Expression>> transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return ExpressionRewriter.Instance.RewriteIter(transformer, value);
        }
    }
}
#endregion