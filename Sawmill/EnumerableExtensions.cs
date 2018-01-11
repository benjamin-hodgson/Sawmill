using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sawmill
{
    internal static class EnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> GetEnumerator<TEnumerable, T>(in TEnumerable enumerable) where TEnumerable : IEnumerable<T>
            => enumerable.GetEnumerator();
    }
}