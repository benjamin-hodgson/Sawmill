using System;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Get the immediate children of the value.
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
    /// <see cref="GetChildren" /> returns the immediate children of the topmost node.
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
    /// <seealso cref="IRewritable{T}.GetChildren" />
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="value">The value.</param>
    /// <returns>The immediate children of <paramref name="value" />.</returns>
    public static T[] GetChildren<T>(this IRewriter<T> rewriter, T value)
    {
        ArgumentNullException.ThrowIfNull(rewriter);

        var count = rewriter.CountChildren(value);
        var array = new T[count];
        rewriter.GetChildren(array, value);
        return array;
    }
}
