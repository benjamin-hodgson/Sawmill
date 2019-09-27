using System;
using System.Buffers;

namespace Sawmill
{
    public static partial class Rewriter
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
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value</param>
        /// <returns>The immediate children of <paramref name="value"/></returns>
        public static T[] GetChildren<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            
            T[] buffer = null;
            var result = rewriter.WithChildren(children => children.ToArray(), value, ref buffer);
            ArrayPool<T>.Shared.Return(buffer);
            return result;
        }
    }
}