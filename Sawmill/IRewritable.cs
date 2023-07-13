using System;

namespace Sawmill;

/// <summary>
/// A object is rewritable if it knows how to access its immediate children.
/// </summary>
///
/// <remarks>
/// Implementations should ensure that you always get the children you just set
/// (<c>rewritable.SetChildren(children).GetChildren() == children</c>),
/// and that successive sets overwrite the earlier operation
/// (<c>rewritable.SetChildren(children1).SetChildren(children2) == rewritable.SetChildren(children2)</c>).
/// </remarks>
///
/// <seealso cref="IRewriter{T}" />
/// <typeparam name="T">The type of the object implementing the interface.</typeparam>
public interface IRewritable<T>
    where T : IRewritable<T>
{
    /// <summary>
    /// Count the immediate children of the current instance.
    /// </summary>
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
    /// <see cref="CountChildren" /> counts the immediate children of the topmost (Add) node.
    /// <code>
    /// Assert.Equal(2, expr.CountChildren());
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewriter{T}.CountChildren" />
    /// <returns>The current instance's number of immediate children.</returns>
    int CountChildren();

    /// <summary>
    /// Copy the immediate children of the current instance into <paramref name="childrenReceiver" />.
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
    /// var array = new Expr[expr.CountChildren()];
    /// expr.GetChildren(array);
    /// Assert.Equal(expected, array);
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewriter{T}.GetChildren" />
    /// <param name="childrenReceiver">
    /// A <see cref="Span{T}" /> to copy the current instance's immediate children into.
    /// The <see cref="Span{T}" />'s <see cref="Span{T}.Length" /> should be equal to the number returned by <see cref="CountChildren" />.
    /// </param>
    void GetChildren(Span<T> childrenReceiver);

    /// <summary>
    /// Set the immediate children of the current instance.
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
    /// Assert.Equal(expected, expr.SetChildren(Children.Two(new Lit(4), new Lit(5))));
    /// </code>
    /// </example>
    ///
    /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)" />.
    /// <param name="newChildren">The new children.</param>
    /// <returns>A copy of the current instance with updated children.</returns>
    T SetChildren(ReadOnlySpan<T> newChildren);
}
