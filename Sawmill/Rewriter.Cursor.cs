using System;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Create a <see cref="Cursor{T}"/> focused on the root node of <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="value">The root node on which the newly created <see cref="Cursor{T}"/> should be focused.</param>
    /// <returns>A <see cref="Cursor{T}"/> focused on the root node of <paramref name="value"/>.</returns>
    public static Cursor<T> Cursor<T>(this IRewriter<T> rewriter, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        return new Cursor<T>(rewriter, value);
    }
}
