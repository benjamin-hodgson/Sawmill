using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Sawmill;

internal static class EnumerableExtensions
{
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
