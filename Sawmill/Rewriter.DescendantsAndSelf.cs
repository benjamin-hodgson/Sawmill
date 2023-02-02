using System;
using System.Buffers;
using System.Collections.Generic;

namespace Sawmill;

public static partial class Rewriter
{
    /// <summary>
    /// Yields all of the nodes in the tree represented by <paramref name="value" />, starting at the bottom.
    ///
    /// <para>
    /// This is a depth-first post-order traversal.
    /// </para>
    ///
    /// See <seealso cref="SelfAndDescendants" />.
    /// </summary>
    /// <example>
    /// <code>
    /// Expr expr = new Add(
    ///     new Add(
    ///         new Lit(1),
    ///         new Lit(2)
    ///     ),
    ///     new Lit(3)
    /// );
    /// Expr[] expected = new[]
    ///     {
    ///         new Lit(1),
    ///         new Lit(2),
    ///         new Add(new Lit(1), new Lit(2)),
    ///         new Lit(3),
    ///         expr
    ///     };
    /// Assert.Equal(expected, rewriter.DescendantsAndSelf(expr));
    /// </code>
    /// </example>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <param name="rewriter">The rewriter.</param>
    /// <param name="value">The value to traverse.</param>
    /// <returns>An enumerable containing all of the nodes in the tree represented by <paramref name="value" />, starting at the bottom.</returns>
    public static IEnumerable<T> DescendantsAndSelf<T>(this IRewriter<T> rewriter, T value)
    {
        if (rewriter == null)
        {
            throw new ArgumentNullException(nameof(rewriter));
        }

        IEnumerable<T> Iterator()
        {
            var stack = new Stack<DescendantsAndSelfFrame<T>>();

            var initialArray = ArrayPool<T>.Shared.Rent(1);
            initialArray[0] = value;
            var enumerator = new DescendantsAndSelfFrame<T>(initialArray, 1);
            do
            {
                while (enumerator.MoveNext())
                {
                    stack.Push(enumerator);

                    var count = rewriter.CountChildren(enumerator.Current);
                    var array = ArrayPool<T>.Shared.Rent(count);

                    rewriter.GetChildren(array.AsSpan()[..count], enumerator.Current);

                    enumerator = new DescendantsAndSelfFrame<T>(array, count);
                }

                enumerator.Dispose();
                enumerator = stack.Pop();
                yield return enumerator.Current;
            }
            while (stack.Count != 0);
        }

        return Iterator();
    }

    private struct DescendantsAndSelfFrame<T>
    {
        private T[]? _array;
        private readonly int _count;
        private int _position;

        public DescendantsAndSelfFrame(T[] array, int count)
        {
            _array = array;
            _count = count;
            _position = -1;
        }

        public T Current => _array![_position];

        public bool MoveNext()
        {
            _position++;
            return _position < _count;
        }

        public void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array);
                _array = null;
            }
        }
    }
}
