using System;
using System.Collections.Generic;
#if NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Sawmill
{
    /// <summary>
    /// Extension methods for <see cref="IRewritable{T}"/> implementations.
    /// </summary>
    public static class Rewritable
    {
        //!pastedoc M:Sawmill.Rewriter.GetChildren``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Get the immediate children of the value.
        ///     <seealso cref="M:Sawmill.IRewritable`1.GetChildren(System.Span{`0})" /></summary>
        /// <example>
        ///     Given a representation of the expression <c>(1+2)+3</c>,
        ///     <code>
        ///     Expr expr = new Add(
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     );
        ///     </code><see cref="M:Sawmill.Rewriter.GetChildren``1(Sawmill.IRewriter{``0},``0)" /> returns the immediate children of the topmost node.
        ///     <code>
        ///     Expr[] expected = new[]
        ///         {
        ///             new Add(
        ///                 new Lit(1),
        ///                 new Lit(2)
        ///             ),
        ///             new Lit(3)
        ///         };
        ///     Assert.Equal(expected, rewriter.GetChildren(expr));
        ///     </code></example>
        /// <param name="value">The value</param>
        /// <returns>The immediate children of <paramref name="value" /></returns>
        /// <seealso cref="M:Sawmill.Rewriter.GetChildren``1(Sawmill.IRewriter{``0},``0)"/>
        public static T[] GetChildren<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.GetChildren(value);

        //!pastedoc M:Sawmill.Rewriter.DescendantsAndSelf``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Yields all of the nodes in the tree represented by <paramref name="value" />, starting at the bottom.
        ///     
        ///     <para>
        ///     This is a depth-first post-order traversal.
        ///     </para><seealso cref="M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <example>
        ///   <code>
        ///     Expr expr = new Add(
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     );
        ///     Expr[] expected = new[]
        ///         {
        ///             new Lit(1),
        ///             new Lit(2),
        ///             new Add(new Lit(1), new Lit(2)),
        ///             new Lit(3),
        ///             expr    
        ///         };
        ///     Assert.Equal(expected, rewriter.DescendantsAndSelf(expr));
        ///     </code>
        /// </example>
        /// <param name="value">The value to traverse</param>
        /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value" />, starting at the bottom.</returns>
        /// <seealso cref="M:Sawmill.Rewriter.DescendantsAndSelf``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<T> DescendantsAndSelf<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelf(value);
        
        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Yields all of the nodes in the tree represented by <paramref name="value" />, starting at the top.
        ///     
        ///     <para>
        ///     This is a depth-first pre-order traversal.
        ///     </para><seealso cref="M:Sawmill.Rewriter.DescendantsAndSelf``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <example>
        ///   <code>
        ///     Expr expr = new Add(
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     );
        ///     Expr[] expected = new[]
        ///         {
        ///             expr,
        ///             new Add(new Lit(1), new Lit(2)),
        ///             new Lit(1),
        ///             new Lit(2),
        ///             new Lit(3),
        ///         };
        ///     Assert.Equal(expected, rewriter.SelfAndDescendants(expr));
        ///     </code>
        /// </example>
        /// <param name="value">The value to traverse</param>
        /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value" />, starting at the top.</returns>
        /// <seealso cref="M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<T> SelfAndDescendants<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendants(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsBreadthFirst``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Yields all of the nodes in the tree represented by <paramref name="value" /> in a breadth-first traversal order.
        ///     
        ///     <para>
        ///     This is a breadth-first pre-order traversal.
        ///     </para></summary>
        /// <param name="value">The value to traverse</param>
        /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value" /> in a breadth-first traversal order.</returns>
        /// <seealso cref="M:Sawmill.Rewriter.SelfAndDescendantsBreadthFirst``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<T> SelfAndDescendantsBreadthFirst<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsBreadthFirst(value);

        //!pastedoc M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Returns an array containing each immediate child of
        ///     <paramref name="value" /> paired with a function to replace the child.
        ///     This is typically useful when you need to replace a node's children one at a time,
        ///     such as during mutation testing.
        ///     
        ///     <para>
        ///     The replacement function can be seen as the "context" of the child; calling the
        ///     function with a new child "plugs the hole" in the context.
        ///     </para><seealso cref="M:Sawmill.Rewriter.SelfAndDescendantsInContext``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <param name="value">The value to get the contexts for the immediate children</param>
        /// <seealso cref="M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{``0},``0)"/>
        public static (T item, Func<T, T> replace)[] ChildrenInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.ChildrenInContext(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsInContext``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///      Yields each node in the tree represented by <paramref name="value" />
        ///      paired with a function to replace the node, starting at the top.
        ///      This is typically useful when you need to replace nodes one at a time,
        ///      such as during mutation testing.
        ///      
        ///      <para>
        ///      The replacement function can be seen as the "context" of the node; calling the
        ///      function with a new node "plugs the hole" in the context.
        ///      </para><para>
        ///      This is a depth-first pre-order traversal.
        ///      </para><seealso cref="M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <param name="value">The value to get the contexts for the descendants</param>
        /// <seealso cref="M:Sawmill.Rewriter.SelfAndDescendantsInContext``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsInContext(value);

        //!pastedoc M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Yields each node in the tree represented by <paramref name="value" />
        ///     paired with a function to replace the node, starting at the bottom.
        ///     This is typically useful when you need to replace nodes one at a time,
        ///     such as during mutation testing.
        ///     
        ///     <para>
        ///     The replacement function can be seen as the "context" of the node; calling the
        ///     function with a new node "plugs the hole" in the context.
        ///     </para><para>
        ///     This is a depth-first post-order traversal.
        ///     </para><seealso cref="M:Sawmill.Rewriter.DescendantsAndSelf``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.SelfAndDescendantsInContext``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <param name="value">The value to get the contexts for the descendants</param>
        /// <seealso cref="M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContext<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.DescendantsAndSelfInContext(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsInContextBreadthFirst``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Yields each node in the tree represented by <paramref name="value" />
        ///     paired with a function to replace the node, in a breadth-first traversal order.
        ///     This is typically useful when you need to replace nodes one at a time,
        ///     such as during mutation testing.
        ///     
        ///     <para>
        ///     The replacement function can be seen as the "context" of the node; calling the
        ///     function with a new node "plugs the hole" in the context.
        ///     </para><para>
        ///     This is a breadth-first pre-order traversal.
        ///     </para><seealso cref="M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{``0},``0)" /><seealso cref="M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{``0},``0)" /></summary>
        /// <param name="value">The value to get the contexts for the descendants</param>
        /// <seealso cref="M:Sawmill.Rewriter.SelfAndDescendantsInContextBreadthFirst``1(Sawmill.IRewriter{``0},``0)"/>
        public static IEnumerable<(T item, Func<T, T> replace)> SelfAndDescendantsInContextBreadthFirst<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.SelfAndDescendantsInContextBreadthFirst(value);
        
        //!pastedoc M:Sawmill.Rewriter.DescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},``0)
        /// <summary>
        ///     Returns the descendant at a particular location in <paramref name="value" /></summary>
        /// <param name="value">The rewritable tree type</param>
        /// <param name="path">The route to take to find the descendant</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Thrown if <paramref name="path" /> leads off the edge of the tree
        ///     </exception>
        /// <returns>The descendant found by following the directions in <paramref name="path" /></returns>
        /// <seealso cref="M:Sawmill.Rewriter.DescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},``0)"/>
        public static T DescendantAt<T>(this T value, IEnumerable<Direction> path) where T : IRewritable<T>
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            return RewritableRewriter<T>.Instance.DescendantAt(path, value);
        }

        //!pastedoc M:Sawmill.Rewriter.ReplaceDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},``0,``0)
        /// <summary>
        ///     Replaces the descendant at a particular location in <paramref name="value" /></summary>
        /// <param name="value">The rewritable tree type</param>
        /// <param name="path">The route to take to find the descendant</param>
        /// <param name="newDescendant">The replacement descendant</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Thrown if <paramref name="path" /> leads off the edge of the tree
        ///     </exception>
        /// <returns>
        ///     A copy of <paramref name="value" /> with <paramref name="newDescendant" /> placed at the location indicated by <paramref name="path" /></returns>
        /// <seealso cref="M:Sawmill.Rewriter.ReplaceDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},``0,``0)"/>
        public static T ReplaceDescendantAt<T>(this T value, IEnumerable<Direction> path, T newDescendant) where T : IRewritable<T>
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            return RewritableRewriter<T>.Instance.ReplaceDescendantAt(path, newDescendant, value);
        }

        //!pastedoc M:Sawmill.Rewriter.RewriteDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},System.Func{``0,``0},``0)
        /// <summary>
        ///     Apply a function at a particular location in <paramref name="value" /></summary>
        /// <param name="value">The rewritable tree type</param>
        /// <param name="path">The route to take to find the descendant</param>
        /// <param name="transformer">A function to calculate a replacement for the descendant</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Thrown if <paramref name="path" /> leads off the edge of the tree
        ///     </exception>
        /// <returns>
        ///     A copy of <paramref name="value" /> with the result of <paramref name="transformer" /> placed at the location indicated by <paramref name="path" /></returns>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},System.Func{``0,``0},``0)"/>
        public static T RewriteDescendantAt<T>(this T value, IEnumerable<Direction> path, Func<T, T> transformer) where T : IRewritable<T>
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            
            return RewritableRewriter<T>.Instance.RewriteDescendantAt(path, transformer, value);
        }

#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.RewriteDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)
        /// <summary>
        ///     Apply an asynchronous function at a particular location in <paramref name="value" /></summary>
        /// <param name="value">The rewritable tree type</param>
        /// <param name="path">The route to take to find the descendant</param>
        /// <param name="transformer">An asynchronous function to calculate a replacement for the descendant</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     Thrown if <paramref name="path" /> leads off the edge of the tree
        ///     </exception>
        /// <returns>
        ///     A copy of <paramref name="value" /> with the result of <paramref name="transformer" /> placed at the location indicated by <paramref name="path" /></returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" />.</remarks>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteDescendantAt``1(Sawmill.IRewriter{``0},System.Collections.Generic.IEnumerable{Sawmill.Direction},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)"/>
        public static ValueTask<T> RewriteDescendantAt<T>(this T value, IEnumerable<Direction> path, Func<T, ValueTask<T>> transformer) where T : IRewritable<T>
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            
            return RewritableRewriter<T>.Instance.RewriteDescendantAt(path, transformer, value);
        }
#endif

        //!pastedoc M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{``0},``0)
        /// <summary>
        ///     Create a <see cref="M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{``0},``0)" /> focused on the root node of <paramref name="value" />.
        ///     </summary>
        /// <param name="value">The root node on which the newly created <see cref="M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{``0},``0)" /> should be focused</param>
        /// <returns>A <see cref="M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{``0},``0)" /> focused on the root node of <paramref name="value" /></returns>
        /// <seealso cref="M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{``0},``0)"/>
        public static Cursor<T> Cursor<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.Cursor(value);

        //!pastedoc M:Sawmill.Rewriter.Fold``2(Sawmill.IRewriter{``0},Sawmill.SpanFunc{``1,``0,``1},``0)
        /// <summary>
        ///     Flattens all the nodes in the tree represented by <paramref name="value" /> into a single result,
        ///     using an aggregation function to combine each node with the results of folding its children.
        ///     </summary>
        /// <param name="func">The aggregation function</param>
        /// <param name="value">The value to fold</param>
        /// <returns>The result of aggregating the tree represented by <paramref name="value" />.</returns>
        /// <seealso cref="M:Sawmill.Rewriter.Fold``2(Sawmill.IRewriter{``0},Sawmill.SpanFunc{``1,``0,``1},``0)"/>
        public static U Fold<T, U>(this T value, SpanFunc<U, T, U> func) where T : IRewritable<T>
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.Fold(func, value);
        }
        
#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.Fold``2(Sawmill.IRewriter{``0},System.Func{System.Memory{``1},``0,System.Threading.Tasks.ValueTask{``1}},``0)
        /// <summary>
        ///     Flattens all the nodes in the tree represented by <paramref name="value" /> into a single result,
        ///     using an asynchronous aggregation function to combine each node with the results of folding its children.
        ///     </summary>
        /// <param name="func">The asynchronous aggregation function</param>
        /// <param name="value">The value to fold</param>
        /// <returns>The result of aggregating the tree represented by <paramref name="value" />.</returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" />.</remarks>
        /// <seealso cref="M:Sawmill.Rewriter.Fold``2(Sawmill.IRewriter{``0},System.Func{System.Memory{``1},``0,System.Threading.Tasks.ValueTask{``1}},``0)"/>
        public static ValueTask<U> Fold<T, U>(this T value, Func<Memory<U>, T, ValueTask<U>> func) where T : IRewritable<T>
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.Fold(func, value);
        }
#endif


        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])
        /// <summary>
        ///     Flatten all of the nodes in the trees represented by <paramref name="values" />
        ///     into a single value at the same time, using an aggregation function to combine
        ///     nodes with the results of aggregating their children.
        ///     The trees are iterated in lock-step, much like an n-ary
        ///     <see cref="M:System.Linq.Enumerable.Zip``3(System.Collections.Generic.IEnumerable{``0},System.Collections.Generic.IEnumerable{``1},System.Func{``0,``1,``2})" />.
        ///     
        ///     When trees are not the same size, the larger ones are
        ///     truncated both horizontally and vertically.
        ///     That is, if a pair of nodes have a different number of children,
        ///     the rightmost children of the larger of the two nodes are discarded.
        ///     </summary>
        /// <example>
        ///     Here's an example of using <see cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])" /> to test if two trees are syntactically equal.
        ///     <code>
        ///     static bool Equals(this Expr left, Expr right)
        ///         =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///             right,
        ///             (xs, results) =&gt;
        ///             {
        ///                 switch (xs[0])
        ///                 {
        ///                     case Add a1 when xs[1] is Add a2:
        ///                         return results.All(x =&gt; x);
        ///                     case Lit l1 when xs[1] is Lit l2:
        ///                         return l1.Value == l2.Value;
        ///                     default:
        ///                         return false;
        ///                 }
        ///             }
        ///         );
        ///     </code></example>
        /// <param name="func">The aggregation function</param>
        /// <param name="values">The trees to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        /// <seealso cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])"/>
        public static U ZipFold<T, U>(this T[] values, Func<T[], IEnumerable<U>, U> func) where T : IRewritable<T>
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.ZipFold(func, values);
        }
        
#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])
        /// <summary>
        ///     Flatten all of the nodes in the trees represented by <paramref name="values" />
        ///     into a single value at the same time, using an aggregation function to combine
        ///     nodes with the results of aggregating their children.
        ///     The trees are iterated in lock-step, much like an n-ary
        ///     <see cref="M:System.Linq.Enumerable.Zip``3(System.Collections.Generic.IEnumerable{``0},System.Collections.Generic.IEnumerable{``1},System.Func{``0,``1,``2})" />.
        ///     
        ///     When trees are not the same size, the larger ones are
        ///     truncated both horizontally and vertically.
        ///     That is, if a pair of nodes have a different number of children,
        ///     the rightmost children of the larger of the two nodes are discarded.
        ///     </summary>
        /// <example>
        ///     Here's an example of using <see cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])" /> to test if two trees are syntactically equal.
        ///     <code>
        ///     static bool Equals(this Expr left, Expr right)
        ///         =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///             right,
        ///             (xs, results) =&gt;
        ///             {
        ///                 switch (xs[0])
        ///                 {
        ///                     case Add a1 when xs[1] is Add a2:
        ///                         return results.All(x =&gt; x);
        ///                     case Lit l1 when xs[1] is Lit l2:
        ///                         return l1.Value == l2.Value;
        ///                     default:
        ///                         return false;
        ///                 }
        ///             }
        ///         );
        ///     </code></example>
        /// <param name="func">The aggregation function</param>
        /// <param name="values">The trees to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        /// <remarks>
        ///     This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" /> and <see cref="T:System.Collections.Generic.IAsyncEnumerable`1" />.
        ///     </remarks>
        /// <seealso cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])"/>
        public static ValueTask<U> ZipFold<T, U>(this T[] values, Func<T[], IAsyncEnumerable<U>, ValueTask<U>> func) where T : IRewritable<T>
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.ZipFold(func, values);
        }
#endif

        #pragma warning disable CS1734, CS1572, CS1573
        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])
        /// <summary>
        ///     Flatten all of the nodes in the trees represented by <paramref name="values" />
        ///     into a single value at the same time, using an aggregation function to combine
        ///     nodes with the results of aggregating their children.
        ///     The trees are iterated in lock-step, much like an n-ary
        ///     <see cref="M:System.Linq.Enumerable.Zip``3(System.Collections.Generic.IEnumerable{``0},System.Collections.Generic.IEnumerable{``1},System.Func{``0,``1,``2})" />.
        ///     
        ///     When trees are not the same size, the larger ones are
        ///     truncated both horizontally and vertically.
        ///     That is, if a pair of nodes have a different number of children,
        ///     the rightmost children of the larger of the two nodes are discarded.
        ///     </summary>
        /// <example>
        ///     Here's an example of using <see cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])" /> to test if two trees are syntactically equal.
        ///     <code>
        ///     static bool Equals(this Expr left, Expr right)
        ///         =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///             right,
        ///             (xs, results) =&gt;
        ///             {
        ///                 switch (xs[0])
        ///                 {
        ///                     case Add a1 when xs[1] is Add a2:
        ///                         return results.All(x =&gt; x);
        ///                     case Lit l1 when xs[1] is Lit l2:
        ///                         return l1.Value == l2.Value;
        ///                     default:
        ///                         return false;
        ///                 }
        ///             }
        ///         );
        ///     </code></example>
        /// <param name="func">The aggregation function</param>
        /// <param name="values">The trees to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        /// <seealso cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IEnumerable{``1},``1},``0[])"/>
        public static U ZipFold<T, U>(this T value1, T value2, Func<T, T, IEnumerable<U>, U> func) where T : IRewritable<T>
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.ZipFold<T, U>((xs, cs) => func(xs[0], xs[1], cs), value1, value2);
        }
        #pragma warning restore CS1734, CS1572, CS1573 

#if NETSTANDARD2_1_OR_GREATER
        #pragma warning disable CS1734, CS1572, CS1573
        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])
        /// <summary>
        ///     Flatten all of the nodes in the trees represented by <paramref name="values" />
        ///     into a single value at the same time, using an aggregation function to combine
        ///     nodes with the results of aggregating their children.
        ///     The trees are iterated in lock-step, much like an n-ary
        ///     <see cref="M:System.Linq.Enumerable.Zip``3(System.Collections.Generic.IEnumerable{``0},System.Collections.Generic.IEnumerable{``1},System.Func{``0,``1,``2})" />.
        ///     
        ///     When trees are not the same size, the larger ones are
        ///     truncated both horizontally and vertically.
        ///     That is, if a pair of nodes have a different number of children,
        ///     the rightmost children of the larger of the two nodes are discarded.
        ///     </summary>
        /// <example>
        ///     Here's an example of using <see cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])" /> to test if two trees are syntactically equal.
        ///     <code>
        ///     static bool Equals(this Expr left, Expr right)
        ///         =&gt; left.ZipFold&lt;Expr, bool&gt;(
        ///             right,
        ///             (xs, results) =&gt;
        ///             {
        ///                 switch (xs[0])
        ///                 {
        ///                     case Add a1 when xs[1] is Add a2:
        ///                         return results.All(x =&gt; x);
        ///                     case Lit l1 when xs[1] is Lit l2:
        ///                         return l1.Value == l2.Value;
        ///                     default:
        ///                         return false;
        ///                 }
        ///             }
        ///         );
        ///     </code></example>
        /// <param name="func">The aggregation function</param>
        /// <param name="values">The trees to fold</param>
        /// <returns>The result of aggregating the two trees</returns>
        /// <remarks>
        ///     This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" /> and <see cref="T:System.Collections.Generic.IAsyncEnumerable`1" />.
        ///     </remarks>
        /// <seealso cref="M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{``0},System.Func{``0[],System.Collections.Generic.IAsyncEnumerable{``1},System.Threading.Tasks.ValueTask{``1}},``0[])"/>
        public static ValueTask<U> ZipFold<T, U>(this T value1, T value2, Func<T, T, IAsyncEnumerable<U>, ValueTask<U>> func) where T : IRewritable<T>
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return RewritableRewriter<T>.Instance.ZipFold<T, U>((xs, cs) => func(xs[0], xs[1], cs), value1, value2);
        }
        #pragma warning restore CS1734, CS1572, CS1573 
#endif

        //!pastedoc M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)
        /// <summary>
        ///     Rebuild a tree by applying a transformation function to every node from bottom to top.
        ///     </summary>
        /// <example>
        ///     Given a representation of the expression <c>(1+2)+3</c>,
        ///     <code>
        ///     Expr expr = new Add(
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     );
        ///     </code><see cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)" /> replaces the leaves of the tree with the result of calling <paramref name="transformer" />,
        ///     then replaces their parents with the result of calling <paramref name="transformer" />, and so on.
        ///     By the end, <see cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)" /> has traversed the whole tree.
        ///     <code>
        ///     Expr expected = transformer(new Add(
        ///         transformer(new Add(
        ///             transformer(new Lit(1)),
        ///             transformer(new Lit(2))
        ///         )),
        ///         transformer(new Lit(3))
        ///     ));
        ///     Assert.Equal(expected, rewriter.Rewrite(transformer, expr));
        ///     </code></example>
        /// <param name="transformer">The transformation function to apply to every node in the tree</param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        ///     The result of applying <paramref name="transformer" /> to every node in the tree represented by <paramref name="value" />.
        ///     </returns>
        /// <seealso cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)"/>
        public static T Rewrite<T>(this T value, Func<T, T> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.Rewrite(transformer, value);
        }
#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)
        /// <summary>
        ///     Rebuild a tree by applying an asynchronous transformation function to every node from bottom to top.
        ///     </summary>
        /// <example>
        ///     Given a representation of the expression <c>(1+2)+3</c>,
        ///     <code>
        ///     Expr expr = new Add(
        ///         new Add(
        ///             new Lit(1),
        ///             new Lit(2)
        ///         ),
        ///         new Lit(3)
        ///     );
        ///     </code><see cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)" /> replaces the leaves of the tree with the result of calling <paramref name="transformer" />,
        ///     then replaces their parents with the result of calling <paramref name="transformer" />, and so on.
        ///     By the end, <see cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)" /> has traversed the whole tree.
        ///     <code>
        ///     Expr expected = await transformer(new Add(
        ///         await transformer(new Add(
        ///             await transformer(new Lit(1)),
        ///             await transformer(new Lit(2))
        ///         )),
        ///         await transformer(new Lit(3))
        ///     ));
        ///     Assert.Equal(expected, await rewriter.Rewrite(transformer, expr));
        ///     </code></example>
        /// <param name="transformer">The asynchronous transformation function to apply to every node in the tree</param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        ///     The result of applying <paramref name="transformer" /> to every node in the tree represented by <paramref name="value" />.
        ///     </returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" />.</remarks>
        /// <seealso cref="M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)"/>
        public static ValueTask<T> Rewrite<T>(this T value, Func<T, ValueTask<T>> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.Rewrite(transformer, value);
        }
#endif
        
        //!pastedoc M:Sawmill.Rewriter.RewriteChildren``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)
        /// <summary>
        ///     Update the immediate children of the value by applying a transformation function to each one.
        ///     </summary>
        /// <param name="transformer">A transformation function to apply to each of <paramref name="value" />'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer" />.</param>
        /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteChildren``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)"/>
        public static T RewriteChildren<T>(this T value, Func<T, T> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.RewriteChildren(transformer, value);
        }

#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.RewriteChildren``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)
        /// <summary>
        ///     Update the immediate children of the value by applying an asynchronous transformation function to each one.
        ///     </summary>
        /// <param name="transformer">An asynchronous transformation function to apply to each of <paramref name="value" />'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer" />.</param>
        /// <returns>A copy of <paramref name="value" /> with updated children.</returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" />.</remarks>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteChildren``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)"/>
        public static ValueTask<T> RewriteChildren<T>(this T value, Func<T, ValueTask<T>> transformer) where T : IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.RewriteChildren(transformer, value);
        }
#endif

        //!pastedoc M:Sawmill.Rewriter.RewriteIter``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)
        /// <summary>
        ///     Rebuild a tree by repeatedly applying a transformation function to every node in the tree,
        ///     until a fixed point is reached. <paramref name="transformer" /> should always eventually return
        ///     its argument unchanged, or this method will loop.
        ///     That is, <c>x.RewriteIter(transformer).SelfAndDescendants().All(x =&gt; transformer(x) == x)</c>.
        ///     <para>
        ///     This is typically useful when you want to put your tree into a normal form
        ///     by applying a collection of rewrite rules until none of them can fire any more.
        ///     </para></summary>
        /// <param name="transformer">
        ///     A transformation function to apply to every node in <paramref name="value" /> repeatedly.
        ///     </param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        ///     The result of applying <paramref name="transformer" /> to every node in the tree
        ///     represented by <paramref name="value" /> repeatedly until a fixed point is reached.
        ///     </returns>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteIter``1(Sawmill.IRewriter{``0},System.Func{``0,``0},``0)"/>
        public static T RewriteIter<T>(this T value, Func<T, T> transformer) where T : class, IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.RewriteIter(transformer, value);
        }

#if NETSTANDARD2_1_OR_GREATER
        //!pastedoc M:Sawmill.Rewriter.RewriteIter``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)
        /// <summary>
        ///     Rebuild a tree by repeatedly applying an asynchronous transformation function to every node in the tree,
        ///     until a fixed point is reached. <paramref name="transformer" /> should always eventually return
        ///     its argument unchanged, or this method will loop.
        ///     That is, <c>x.RewriteIter(transformer).SelfAndDescendants().All(x =&gt; await transformer(x) == x)</c>.
        ///     <para>
        ///     This is typically useful when you want to put your tree into a normal form
        ///     by applying a collection of rewrite rules until none of them can fire any more.
        ///     </para></summary>
        /// <param name="transformer">
        ///     An asynchronous transformation function to apply to every node in <paramref name="value" /> repeatedly.
        ///     </param>
        /// <param name="value">The value to rewrite</param>
        /// <returns>
        ///     The result of applying <paramref name="transformer" /> to every node in the tree
        ///     represented by <paramref name="value" /> repeatedly until a fixed point is reached.
        ///     </returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="T:System.Threading.Tasks.ValueTask" />.</remarks>
        /// <seealso cref="M:Sawmill.Rewriter.RewriteIter``1(Sawmill.IRewriter{``0},System.Func{``0,System.Threading.Tasks.ValueTask{``0}},``0)"/>
        public static ValueTask<T> RewriteIter<T>(this T value, Func<T, ValueTask<T>> transformer) where T : class, IRewritable<T>
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }
            return RewritableRewriter<T>.Instance.RewriteIter(transformer, value);
        }
#endif
    }
}
