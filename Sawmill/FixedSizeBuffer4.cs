#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sawmill;

[InlineArray(4)]
internal struct FixedSizeBuffer4<T>
{
    [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Required to be non-readonly for InlineArray")]
    private T _element0;
}
#endif
