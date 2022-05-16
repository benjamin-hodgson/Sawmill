using System;
using System.Threading.Tasks;

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

            var closure = new FoldClosure<T, U>(rewriter, func);

            var result = closure.Go(value);

            closure.Dispose();

            return result;
        }

        private class FoldClosure<T, U> : Traversal<T>
        {
            private ChunkStack<U> _results = new ChunkStack<U>();
            private readonly SpanFunc<U, T, U> _func;

            public FoldClosure(IRewriter<T> rewriter, SpanFunc<U, T, U> func) : base(rewriter)
            {
                _func = func;
            }

            public U Go(T value)
                => WithChildren(
                    children =>
                    {
                        var span = _results.Allocate(children.Length);
                        for (var i = 0; i < children.Length; i++)
                        {
                            span[i] = Go(children[i]);
                        }
                        var result = _func(span, value);
                        _results.Free(span);
                        return result;
                    },
                    value
                );

            public override void Dispose()
            {
                _results.Dispose();
                _results = default;
                base.Dispose();
            }
        }

        /// <summary>
        /// Flattens all the nodes in the tree represented by <paramref name="value"/> into a single result,
        /// using an asynchronous aggregation function to combine each node with the results of folding its children.
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <typeparam name="U">The type of the result of aggregation</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="func">The asynchronous aggregation function</param>
        /// <param name="value">The value to fold</param>
        /// <returns>The result of aggregating the tree represented by <paramref name="value"/>.</returns>
        /// <remarks>This method is not available on platforms which do not support <see cref="ValueTask"/>.</remarks>
        public static async ValueTask<U> Fold<T, U>(this IRewriter<T> rewriter, Func<Memory<U>, T, ValueTask<U>> func, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var closure = new FoldAsyncClosure<T, U>(rewriter, func);

            var result = await closure.Go(value).ConfigureAwait(false);

            closure.Dispose();

            return result;
        }

        private class FoldAsyncClosure<T, U> : AsyncTraversal<T>
        {
            private readonly Box<ChunkStack<U>> _results = new Box<ChunkStack<U>>(new ChunkStack<U>());
            private readonly Func<Memory<U>, T, ValueTask<U>> _func;

            public FoldAsyncClosure(IRewriter<T> rewriter, Func<Memory<U>, T, ValueTask<U>> func) : base(rewriter)
            {
                _func = func;
            }

            public ValueTask<U> Go(T value)
                => WithChildren(
                    async children =>
                    {
                        var memory = _results.Value.AllocateMemory(children.Length);
                        for (var i = 0; i < children.Length; i++)
                        {
                            var newChild = await Go(children.Span[i]).ConfigureAwait(false);
                            memory.Span[i] = newChild;
                        }
                        var result = await _func(memory, value).ConfigureAwait(false);
                        _results.Value.Free(memory);
                        return result;
                    },
                    value
                );

            public override void Dispose()
            {
                _results.Value.Dispose();
                _results.Value = default;
                base.Dispose();
            }
        }
    }
}
