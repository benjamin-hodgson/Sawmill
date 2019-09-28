namespace Sawmill
{
    public static partial class Rewriter
    {
        internal static void WithChildren_<T>(this IRewriter<T> rewriter, SpanAction<T> action, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            action(span);

            chunks.Free(span);
        }

        internal static void WithChildren_<T, U>(this IRewriter<T> rewriter, SpanAction<T, U> action, U ctx, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            action(span, ctx);
            
            chunks.Free(span);
        }


        internal static R WithChildren<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            var result = action(span);

            chunks.Free(span);
            return result;
        }

        internal static R WithChildren<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
            var span = chunks.Allocate(count);

            rewriter.GetChildren(span, value);
            var result = action(span, ctx);

            chunks.Free(span);
            return result;
        }
    }
}