using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Flatten all of the nodes in the trees represented by <paramref name="values"/>
        /// into a single value at the same time, using an aggregation function to combine
        /// nodes with the results of aggregating their children.
        /// The trees are iterated in lock-step, much like an n-ary <see cref="Enumerable.Zip"/>.
        /// 
        /// When trees are not the same size, the larger ones are
        /// truncated both horizontally and vertically.
        /// That is, if a pair of nodes have a different number of children,
        /// the rightmost children of the larger of the two nodes are discarded.
        /// </summary>
        /// <example>
        /// Here's an example of using <see cref="ZipFold"/> to test if two trees are syntactically equal.
        /// <code>
        /// static bool Equals(this Expr left, Expr right)
        ///     =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///         right,
        ///         (xs, results) =&gt;
        ///         {
        ///             switch (xs[0])
        ///             {
        ///                 case Add a1 when xs[1] is Add a2:
        ///                     return results.All(x =&gt; x);
        ///                 case Lit l1 when xs[1] is Lit l2:
        ///                     return l1.Value == l2.Value;
        ///                 default:
        ///                     return false;
        ///             }
        ///         }
        ///     );
        /// </code>
        /// </example>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <typeparam name="U">The return type of the aggregation</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="func">The aggregation function</param>
        /// <param name="values">The trees to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        public static U ZipFold<T, U>(
            this IRewriter<T> rewriter,
            Func<T[], IEnumerable<U>, U> func,
            params T[] values
        )
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Func<T[], U> goDelegate = null;
            goDelegate = Go;

            U Go(T[] xs)
                => func(
                    xs,
                    ZipChildren(xs)
                );

            IEnumerable<U> ZipChildren(T[] xs)
            {
                var enumerators = new IEnumerator<T>[xs.Length];
                for (var i = 0; i < xs.Length; i++)
                {
                    enumerators[i] = GetEnumerator<T, Children<T>>(rewriter.GetChildren(xs[i]));
                }

                while (true)
                {
                var currents = new T[xs.Length];
                    for (var i = 0; i < enumerators.Length; i++)
                    {
                        var e = enumerators[i];
                        var hasNext = e.MoveNext();
                        if (!hasNext)
                        {
                            yield break;
                        }
                        currents[i] = e.Current;
                    }
                    yield return goDelegate(currents);
                }
            }

            return Go(values);
        }

        static IEnumerator<T> GetEnumerator<T, TEnumerable>(TEnumerable enumerable) where TEnumerable : IEnumerable<T>
            => enumerable.GetEnumerator();
    }
}
