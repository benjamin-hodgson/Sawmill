using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Sawmill
{
    internal static class EnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> GetEnumerator<TEnumerable, T>(in TEnumerable enumerable) where TEnumerable : IEnumerable<T>
            => enumerable.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> ToImmutableAndClear<T>(this ImmutableArray<T>.Builder builder)
        {
            if (builder.Count == builder.Capacity)
            {
                return builder.MoveToImmutable();
            }
            var immutable = builder.ToImmutable();
            builder.Clear();
            return immutable;
        }
    }
}
