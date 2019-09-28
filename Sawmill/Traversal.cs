using System;

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

        protected void WithChildren_(SpanAction<T> action, T value)
            => Rewriter.WithChildren_(action, value, ref _chunks);

        protected void WithChildren_<U>(SpanAction<T, U> action, U ctx, T value)
            => Rewriter.WithChildren_(action, ctx, value, ref _chunks);

        protected R WithChildren<R>(SpanFunc<T, R> action, T value)
            => Rewriter.WithChildren(action, value, ref _chunks);

        protected R WithChildren<U, R>(SpanFunc<T, U, R> action, U ctx, T value)
            => Rewriter.WithChildren(action, ctx, value, ref _chunks);

        protected T RewriteChildren(Func<T, T> transformer, T value)
            => Rewriter.RewriteChildrenInternal(transformer, value, ref _chunks);

        public virtual void Dispose()
        {
            _chunks.Dispose();
            _chunks = default;
        }
    }
}