using System;
using System.Buffers;

namespace Sawmill
{
    public static partial class Rewriter
    {
        private static void WithChildren_<T>(this IRewriter<T> rewriter, SpanAction<T> action, T value, ref T[] buffer)
        {
            var count = rewriter.CountChildren(value);

            if (buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            if (buffer.Length < count)
            {
                ArrayPool<T>.Shared.Return(buffer);
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);

            action(span);
        }

        private static void WithChildren_<T, U>(this IRewriter<T> rewriter, SpanAction<T, U> action, U ctx, T value, ref T[] buffer)
        {
            var count = rewriter.CountChildren(value);

            if (buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            if (buffer.Length < count)
            {
                ArrayPool<T>.Shared.Return(buffer);
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);

            action(span, ctx);
        }

        


        private static R WithChildren<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, ref T[] buffer)
        {
            var count = rewriter.CountChildren(value);

            if (buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            if (buffer.Length < count)
            {
                ArrayPool<T>.Shared.Return(buffer);
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);

            return action(span);
        }

        private static R WithChildren<T, U,R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, ref T[] buffer)
        {
            var count = rewriter.CountChildren(value);

            if (buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            if (buffer.Length < count)
            {
                ArrayPool<T>.Shared.Return(buffer);
                buffer = ArrayPool<T>.Shared.Rent(count);
            }
            var span = buffer.AsSpan().Slice(0, count);

            rewriter.GetChildren(span, value);

            return action(span, ctx);
        }
    }
}
