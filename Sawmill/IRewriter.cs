using System;

namespace Sawmill;

/// <summary>
/// A rewriter is an object which knows how to access the immediate children of a value of type <typeparamref name="T" />.
/// </summary>
///
/// <remarks>
/// Implementations should ensure that you always get the children you just set
/// (<c>rewriter.GetChildren(rewriter.SetChildren(children, expr)) == children</c>),
/// and that successive sets overwrite the earlier operation
/// (<c>rewriter.SetChildren(children2, rewriter.SetChildren(children1, expr)) == rewriter.SetChildren(children2, expr)</c>).
/// </remarks>
///
/// <seealso cref="IRewritable{T}" />
/// <typeparam name="T">The type for which the rewriter can get the immediate children.</typeparam>
public interface IRewriter<T>
{
    /// <summary>
    /// Count the immediate children of the value.
    /// </summary>
    /// <example>
    /// Given a representation of the expression <c>(1+2)+3</c>.
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// </code>
    /// <see cref="CountChildren" /> counts the immediate children of the topmost (Add) node.
    /// <code>
    /// Assert.Equal(2, rewriter.CountChildren(expr));
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewritable{T}.CountChildren" />
    /// <param name="value">The value.</param>
    /// <returns><paramref name="value" />'s number of immediate children.</returns>
    public int CountChildren(T value);

    /// <summary>
    /// Copy the immediate children of the value into <paramref name="childrenReceiver" />.
    /// </summary>
    /// <example>
    /// Given a representation of the expression <c>(1+2)+3</c>.
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// </code>
    /// <see cref="GetChildren" /> copies the immediate children of the topmost node into the span.
    /// <code>
    /// Expr[] expected = new[]
    ///     {
    ///         new Add(
    ///             new Lit(1),
    ///             new Lit(2)
    ///         ),
    ///         new Lit(3)
    ///     };
    /// var array = new Expr[rewriter.CountChildren(expr)];
    /// rewriter.GetChildren(array, expr);
    /// Assert.Equal(expected, array);
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewritable{T}.GetChildren" />
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}" /> to copy <paramref name="value" />'s immediate children into.
    /// The <see cref="Span{T}" />'s <see cref="Span{T}.Length" /> will be equal to the number returned by <see cref="CountChildren" />.
    /// </param>
    /// <param name="value">The value.</param>
    public void GetChildren(Span<T> childrenReceiver, T value);

    /// <summary>
    /// Set the immediate children of the value.
    /// </summary>
    ///
    /// <remarks>
    /// Callers should ensure that <paramref name="newChildren" /> contains the same number of children as was returned by
    /// <see cref="GetChildren" />.
    /// </remarks>
    ///
    /// <example>
    /// Given a representation of the expression <c>(1+2)+3</c>.
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// </code>
    /// <see cref="SetChildren" /> replaces the immediate children of the topmost node.
    /// <code>
    /// Expr expected = new Add(
    ///     new Lit(4),
    ///     new Lit(5)
    /// );
    /// Assert.Equal(expected, rewriter.SetChildren(Children.Two(new Lit(4), new Lit(5)), expr));
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewritable{T}.SetChildren(ReadOnlySpan{T})" />
    /// <param name="newChildren">The new children.</param>
    /// <param name="value">The old value, whose immediate children should be replaced.</param>
    /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
    public T SetChildren(ReadOnlySpan<T> newChildren, T value);
}
