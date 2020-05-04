#if NETSTANDARD21
using FixedSizeBuffers;
#endif


namespace Sawmill
{
    public static partial class Rewriter
    {
        internal static R WithChildren<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
#if NETSTANDARD21
            if (count <= 4)
            {
                return WithChildren_Fast(rewriter, action, value, count);
            }
#endif

            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            var result = action(span);

            chunks.Free(span);
            return result;
        }

        internal static R WithChildren<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
#if NETSTANDARD21
            if (count <= 4)
            {
                return WithChildren_Fast(rewriter, action, ctx, value, count);
            }
#endif

            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            var result = action(span, ctx);

            chunks.Free(span);
            return result;
        }

#if NETSTANDARD21
        private static R WithChildren_Fast<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, int count)
        {
            var buffer = new FixedSizeBuffer4<T>();
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);
            var result = action(span);

            buffer.Dispose();
            return result;
        }

        private static R WithChildren_Fast<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, int count)
        {
            var buffer = new FixedSizeBuffer4<T>();
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);
            var result = action(span, ctx);

            buffer.Dispose();
            return result;
        }
#endif
    }
}