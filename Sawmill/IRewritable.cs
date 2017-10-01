using System;
using System.Collections.Generic;

namespace Sawmill
{
    /// <summary>
    /// A object is rewriterable if it knows how to access its immediate children.
    /// <seealso cref="IRewriter{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object implementing the interface</typeparam>
    public interface IRewritable<T> where T : IRewritable<T>
    {
        /// <summary>
        /// Get the immediate children of the current instance.
        /// <seealso cref="IRewriter{T}.GetChildren(T)"/>
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
        /// Assert.Equal(expected, expr.GetChildren());
        /// </code>
        /// </example>
        /// <returns>The immediate children of the current instance</returns>
        Children<T> GetChildren();

        /// <summary>
        /// Set the immediate children of the currentInstance.
        /// <para>
        /// Callers should ensure that <paramref name="newChildren"/> contains the same number of children as was returned by
        /// <see cref="GetChildren"/>.
        /// </para>
        /// <seealso cref="IRewriter{T}.SetChildren(Children{T}, T)"/>
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
        /// Assert.Equal(expected, expr.SetChildren(Children.Two(new Lit(4), new Lit(5))));
        /// </code>
        /// </example>
        /// <param name="newChildren">The new children</param>
        /// <returns>A copy of the current instance with updated children.</returns>
        T SetChildren(Children<T> newChildren);

        /// <summary>
        /// Update the immediate children of the current instance by applying a transformation function to each one.
        /// <para>
        /// Implementations of <see cref="IRewritable{T}"/> can use <see cref="Rewritable.DefaultRewriteChildren{T}(T, Func{T, T})"/>,
        /// or you can write your own.
        /// </para>
        /// <para>
        /// NB: A hand-written implementation will not usually be faster
        /// than <see cref="Rewritable.DefaultRewriteChildren{T}(T, Func{T, T})"/>.
        /// If your type has a fixed number of children, and that number is greater than two,
        /// you may see some performance improvements from implementing this method yourself.
        /// Be careful not to rebuild the object if none of the children have changed.
        /// </para>
        /// <seealso cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
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
        /// Assert.Equal(expected, expr.RewriteChildren(transformer));
        /// </code>
        /// </example>
        /// <param name="transformer">A transformation function to apply to each of the current instance's immediate children.</param>
        /// <returns>A copy of the current instance with updated children.</returns>
        T RewriteChildren(Func<T, T> transformer);
    }
}