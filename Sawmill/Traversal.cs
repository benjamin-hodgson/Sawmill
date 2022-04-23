using System;
#if NETSTANDARD2_1_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Sawmill
{
    internal abstract class Traversal<T> : IDisposable
    {
        private ChunkStack<T> _chunks = new ChunkStack<T>();
        protected IRewriter<T> Rewriter { get; }

        public Traversal(IRewriter<T> rewriter)
        {
            Rewriter = rewriter;
        }

        protected R WithChildren<R>(SpanFunc<T, R> action, T value)
            => Rewriter.WithChildren(action, value, ref _chunks);

        protected T RewriteChildren(Func<T, T> transformer, T value)
            => Rewriter.RewriteChildrenInternal(transformer, value, ref _chunks);

        public virtual void Dispose()
        {
            _chunks.Dispose();
            _chunks = default;
        }
    }
#if NETSTANDARD2_1_OR_GREATER
    internal abstract class AsyncTraversal<T> : IDisposable
    {
        private readonly Box<ChunkStack<T>> _chunks = new Box<ChunkStack<T>>(new ChunkStack<T>());
        protected IRewriter<T> Rewriter { get; }

        public AsyncTraversal(IRewriter<T> rewriter)
        {
            Rewriter = rewriter;
        }

        protected ValueTask<R> WithChildren<R>(Func<Memory<T>, ValueTask<R>> action, T value)
            => Rewriter.WithChildren(action, value, _chunks);

        protected ValueTask<T> RewriteChildren(Func<T, ValueTask<T>> transformer, T value)
            => Rewriter.RewriteChildrenInternal(transformer, value, _chunks);

        public virtual void Dispose()
        {
            _chunks.Value.Dispose();
            _chunks.Value = default;
        }
    }
#endif
}
