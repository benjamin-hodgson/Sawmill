using FixedSizeBuffers;


namespace Sawmill;

public static partial class Rewriter
{
    internal static R WithChildren<T, R>(
        this IRewriter<T> rewriter,
        SpanFunc<T, R> action,
        T value,
        ref ChunkStack<T> chunks
    )
    {
        var count = rewriter.CountChildren(value);
        if (count <= 4)
        {
            return WithChildren_Fast(rewriter, action, value, count);
        }

        var span = chunks.Allocate(count);

        rewriter.GetChildren(span, value);
        var result = action(span);

        chunks.Free(span);
        return result;
    }

    internal static R WithChildren<T, U, R>(
        this IRewriter<T> rewriter,
        SpanFunc<T, U, R> action,
        U ctx,
        T value,
        ref ChunkStack<T> chunks
    )
    {
        var count = rewriter.CountChildren(value);
        if (count <= 4)
        {
            return WithChildren_Fast(rewriter, action, ctx, value, count);
        }

        var span = chunks.Allocate(count);

        rewriter.GetChildren(span, value);
        var result = action(span, ctx);

        chunks.Free(span);
        return result;
    }

    internal static async ValueTask<R> WithChildren<T, R>(
        this IRewriter<T> rewriter,
        Func<Memory<T>, ValueTask<R>> action,
        T value,
        Box<ChunkStack<T>> chunks
    )
    {
        var count = rewriter.CountChildren(value);
        var memory = chunks.Value.AllocateMemory(count);

        rewriter.GetChildren(memory.Span, value);
        var result = await action(memory).ConfigureAwait(false);

        chunks.Value.Free(memory);
        return result;
    }

    internal static async ValueTask<R> WithChildren<T, U, R>(
        this IRewriter<T> rewriter,
        Func<Memory<T>, U, ValueTask<R>> action,
        U ctx,
        T value,
        Box<ChunkStack<T>> childrenChunks
    )
    {
        var count = rewriter.CountChildren(value);
        var memory = childrenChunks.Value.AllocateMemory(count);

        rewriter.GetChildren(memory.Span, value);
        var result = await action(memory, ctx).ConfigureAwait(false);

        childrenChunks.Value.Free(memory);
        return result;
    }

    private static R WithChildren_Fast<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, int count)
    {
        var buffer = new FixedSizeBuffer4<T>();
        var span = buffer.AsSpan()[..count];

        rewriter.GetChildren(span, value);
        var result = action(span);

        buffer.Dispose();
        return result;
    }

    private static R WithChildren_Fast<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, int count)
    {
        var buffer = new FixedSizeBuffer4<T>();
        var span = buffer.AsSpan()[..count];

        rewriter.GetChildren(span, value);
        var result = action(span, ctx);

        buffer.Dispose();
        return result;
    }
}
