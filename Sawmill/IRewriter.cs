using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// A rewriter is an object which knows how to access the immediate children of a value of type <typeparamref name="T"/>.
    /// 
    /// <para>
    /// Implementations should ensure that you always get the children you just set
    /// (<c>rewriter.GetChildren(rewriter.SetChildren(children, expr)) == children</c>),
    /// and that successive sets overwrite the earlier operation
    /// (<c>rewriter.SetChildren(children2, rewriter.SetChildren(children1, expr)) == rewriter.SetChildren(children2, expr)</c>).
    /// </para>
    ///
    /// <seealso cref="IRewritable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type for which the rewriter can get the immediate children</typeparam>
    public interface IRewriter<T>
    {
        /// <summary>
        /// Get the immediate children of the value.
        /// <seealso cref="IRewritable{T}.GetChildren"/>
        /// </summary>
        /// <example>
        /// Given a representation of the expression <c>(1+2)+3</c>,
        /// <code>
        /// Expr expr = new Add(
        ///     new Add(
        ///         new Lit(1),
        ///         new Lit(2)
        ///     ),
        ///     new Lit(3)
        /// );
        /// </code>
        /// <see cref="GetChildren"/> returns the immediate children of the topmost node.
        /// <code>
        /// Expr[] expected = new[]
        ///     {
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     };
        /// Assert.Equal(expected, rewriter.GetChildren(expr));
        /// </code>
        /// </example>
        /// <param name="value">The value</param>
        /// <returns>The immediate children of <paramref name="value"/></returns>
        Children<T> GetChildren(T value);

        /// <summary>
        /// Set the immediate children of the value.
        /// <para>
        /// Callers should ensure that <paramref name="newChildren"/> contains the same number of children as was returned by
        /// <see cref="GetChildren"/>.
        /// </para>
        /// <seealso cref="IRewritable{T}.SetChildren(Children{T})"/>
        /// </summary>
        /// <example>
        /// Given a representation of the expression <c>(1+2)+3</c>,
        /// <code>
        /// Expr expr = new Add(
        ///     new Add(
        ///         new Lit(1),
        ///         new Lit(2)
        ///     ),
        ///     new Lit(3)
        /// );
        /// </code>
        /// <see cref="SetChildren"/> replaces the immediate children of the topmost node.
        /// <code>
        /// Expr expected = new Add(
        ///     new Lit(4),
        ///     new Lit(5)
        /// );
        /// Assert.Equal(expected, rewriter.SetChildren(Children.Two(new Lit(4), new Lit(5)), expr));
        /// </code>
        /// </example>
        /// <param name="newChildren">The new children</param>
        /// <param name="value">The old value, whose immediate children should be replaced</param>
        /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
        T SetChildren(Children<T> newChildren, T value);

        /// <summary>
        /// Update the immediate children of the value by applying a transformation function to each one.
        /// <para>
        /// Implementations of <see cref="IRewriter{T}"/> can use <see cref="Rewriter.DefaultRewriteChildren{T}(IRewriter{T}, Func{T, T}, T)"/>,
        /// or you can write your own.
        /// </para>
        /// <para>
        /// NB: A hand-written implementation will not usually be faster
        /// than <see cref="Rewriter.DefaultRewriteChildren{T}(IRewriter{T}, Func{T, T}, T)"/>.
        /// If your type has a fixed number of children, and that number is greater than two,
        /// you may see some performance improvements from implementing this method yourself.
        /// Be careful not to rebuild <paramref name="value"/> if none of the children have changed.
        /// </para>
        /// <seealso cref="IRewritable{T}.RewriteChildren(Func{T, T})"/>
        /// </summary>
        /// <example>
        /// Given a representation of the expression <c>(1+2)+3</c>,
        /// <code>
        /// Expr expr = new Add(
        ///     new Add(
        ///         new Lit(1),
        ///         new Lit(2)
        ///     ),
        ///     new Lit(3)
        /// );
        /// </code>
        /// <see cref="RewriteChildren"/> only affects the immediate children of the topmost node.
        /// <code>
        /// Expr expected = new Add(
        ///     transformer(new Add(
        ///         new Lit(1),
        ///         new Lit(2)
        ///     )),
        ///     transformer(new Lit(3))
        /// );
        /// Assert.Equal(expected, rewriter.RewriteChildren(transformer, expr));
        /// </code>
        /// </example>
        /// <param name="transformer">A transformation function to apply to each of <paramref name="value"/>'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer"/>.</param>
        /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
        T RewriteChildren(Func<T, T> transformer, T value);
    }
}
