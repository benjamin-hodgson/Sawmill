using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Flattens all the nodes in the tree represented by <paramref name="value"/> into a single result,
        /// using an aggregation function to combine each node with the results of folding its children.
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <typeparam name="U">The type of the result of aggregation</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="func">The aggregation function</param>
        /// <param name="value">The value to fold</param>
        /// <returns>The result of aggregating the tree represented by <paramref name="value"/>.</returns>
        public static U Fold<T, U>(this IRewriter<T> rewriter, SpanFunc<U, T, U> func, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            T[] buffer = null;
            var result = rewriter.WithChildren(
                children =>
                {
                    var array = ArrayPool<U>.Shared.Rent(children.Length);
                    for (var i = 0; i < children.Length; i++)
                    {
                        array[i] = rewriter.Fold(func, children[i]);
                    }
                    return func(array.AsSpan().Slice(0, children.Length), value);
                },
                value,
                ref buffer
            );
            ArrayPool<T>.Shared.Return(buffer);
            return result;
        }
    }
}
