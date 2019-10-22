using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sawmill
{
    public static partial class Rewriter
    {
        internal static R WithChildren<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, ref ChunkStack<T> chunks)
        {
            var count = rewriter.CountChildren(value);
#if NETCOREAPP
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
#if NETCOREAPP
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

#if NETCOREAPP
        private static R WithChildren_Fast<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, int count)
        {
            var four = new StackallocFour<T>();
            var span = MemoryMarshal.CreateSpan(ref four.First, count);

            rewriter.GetChildren(span, value);
            var result = action(span);

            KeepAlive(ref four);
            return result;
        }

        private static R WithChildren_Fast<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, int count)
        {
            var four = new StackallocFour<T>();
            var span = MemoryMarshal.CreateSpan(ref four.First, count);

            rewriter.GetChildren(span, value);
            var result = action(span, ctx);

            KeepAlive(ref four);
            return result;
        }

        private struct StackallocFour<T>
        {
#pragma warning disable CS0649  // Field 'FieldName' is never assigned to, and will always have its default value
            public T First;
            public T Second;
            public T Third;
            public T Fourth;
#pragma warning restore CS0649
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void KeepAlive<T>(ref StackallocFour<T> four)
        {
        }
#endif
    }
}