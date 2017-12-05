using System;
using System.Linq;

namespace Sawmill
{
    // todo: codegen-ed overloads for 3-8-ary zip?
    public static partial class Rewriter
    {
        /// <summary>
        /// Flatten all of the nodes in the trees represented by <paramref name="value1"/>
        /// and <paramref name="value2"/> into a single value at the same time,
        /// using an aggregation function to combine two nodes with the results of aggregating their children.
        /// The two trees are iterated in lock-step, much like <see cref="Enumerable.Zip"/>.
        /// 
        /// The larger of the two trees is truncated both horizontally and vertically.
        /// That is, if two nodes have a different number of children,
        /// the rightmost children of the larger of the two nodes are discarded.
        /// </summary>
        /// <example>
        /// Here's an example of using <see cref="ZipFold"/> to test if two trees are syntactically equal.
        /// <code>
        /// static bool Equals(this Expr left, Expr right)
        ///     =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///         right,
        ///         (x, y, results) =&gt;
        ///         {
        ///             switch (x)
        ///             {
        ///                 case Add a1 when y is Add a2:
        ///                     return results.All(x =&gt; x);
        ///                 case Lit l1 when y is Lit l2:
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
        /// <param name="value1">The first tree to fold</param>
        /// <param name="value2">The second tree to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        public static U ZipFold<T, U>(this IRewriter<T> rewriter, Func<T, T, Children<U>, U> func, T value1, T value2)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            Func<T, T, U> goDelegate = null;
            goDelegate = Go;
            U Go(T x, T y)
                => func(
                    x,
                    y,
                    Zip(
                        rewriter.GetChildren(x),
                        rewriter.GetChildren(y),
                        goDelegate
                    )
                );
            return Go(value1, value2);
        }

        private static Children<R> Zip<T, U, R>(Children<T> children1, Children<U> children2, Func<T, U, R> func)
        {
            if (children1.NumberOfChildren == NumberOfChildren.Many || children2.NumberOfChildren == NumberOfChildren.Many)
            {
                return Children.Many(children1.Zip(children2, func));
            }
            switch (children1.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return Children.None<R>();
                case NumberOfChildren.One:
                    switch (children2.NumberOfChildren)
                    {
                        case NumberOfChildren.None:
                            return Children.None<R>();
                        case NumberOfChildren.One:
                        case NumberOfChildren.Two:
                            return Children.One(func(children1.First, children2.First));
                        default:
                            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
                    }
                case NumberOfChildren.Two:
                    switch (children2.NumberOfChildren)
                    {
                        case NumberOfChildren.None:
                            return Children.None<R>();
                        case NumberOfChildren.One:
                            return Children.One(func(children1.First, children2.First));
                        case NumberOfChildren.Two:
                            return Children.Two(func(children1.First, children2.First), func(children1.Second, children2.Second));
                        default:
                            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
                    }
                default:
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
            }
        }
    }
}