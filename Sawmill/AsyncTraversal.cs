using System;
using System.Threading.Tasks;

namespace Sawmill;

internal abstract class AsyncTraversal<T> : IDisposable
{
    private readonly Box<ChunkStack<T>> _chunks = new(default);

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
