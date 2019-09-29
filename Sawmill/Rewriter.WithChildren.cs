using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sawmill
{
    public static partial class Rewriter
    {
        internal static void WithChildren_<T>(this IRewriter<T> rewriter, SpanAction<T> action, T value, ref ChunkStack<T> chunks)
        {
            var four = new StackallocFour<T>();

            var count = rewriter.CountChildren(value);
            var span = GetSpan(count, ref chunks, ref four);

            rewriter.GetChildren(span, value);
            action(span);

            ReturnSpan(span, ref chunks, ref four);
        }

        internal static void WithChildren_<T, U>(this IRewriter<T> rewriter, SpanAction<T, U> action, U ctx, T value, ref ChunkStack<T> chunks)
        {
            var four = new StackallocFour<T>();

            var count = rewriter.CountChildren(value);
            var span = GetSpan(count, ref chunks, ref four);

            rewriter.GetChildren(span, value);
            action(span, ctx);
            
            ReturnSpan(span, ref chunks, ref four);
        }


        internal static R WithChildren<T, R>(this IRewriter<T> rewriter, SpanFunc<T, R> action, T value, ref ChunkStack<T> chunks)
        {
            var four = new StackallocFour<T>();
            
            var count = rewriter.CountChildren(value);
            var span = GetSpan(count, ref chunks, ref four);

            rewriter.GetChildren(span, value);
            var result = action(span);

            ReturnSpan(span, ref chunks, ref four);
            return result;
        }

        internal static R WithChildren<T, U, R>(this IRewriter<T> rewriter, SpanFunc<T, U, R> action, U ctx, T value, ref ChunkStack<T> chunks)
        {
            var four = new StackallocFour<T>();
            
            var count = rewriter.CountChildren(value);
            var span = GetSpan(count, ref chunks, ref four);

            rewriter.GetChildren(span, value);
            var result = action(span, ctx);

            ReturnSpan(span, ref chunks, ref four);
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
        private static unsafe Span<T> GetSpan<T>(int count, ref ChunkStack<T> chunks, ref StackallocFour<T> four)
        {
            if (count == 0)
            {
                return new Span<T>();
            }
            else if (count <= 4)
            {
                return SpanConstructor<T>.Create(ref four.First, count);
            }
            else
            {
                return chunks.Allocate(count);
            }
        }
        private static void ReturnSpan<T>(Span<T> span, ref ChunkStack<T> chunks, ref StackallocFour<T> four)
        {
            if (span.Length > 4)
            {
                chunks.Free(span);
            }
        }
        private static class SpanConstructor<T>
        {
            private delegate Span<T> SpanCtor(ref T value, int length);
            private static SpanCtor _callSpanCtor;

            static SpanConstructor()
            {
                var ctor = typeof(Span<T>)
                    .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single(c => c.GetParameters().Length == 2 && c.GetParameters()[0].ParameterType.IsByRef);

                var method = new DynamicMethod("", typeof(Span<T>), new[] { typeof(T).MakeByRefType(), typeof(int) });

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Ret);

                _callSpanCtor = (SpanCtor)method.CreateDelegate(typeof(SpanCtor));
            }

            public static Span<T> Create(ref T value, int length)
                => _callSpanCtor(ref value, length);
        }
    }
}