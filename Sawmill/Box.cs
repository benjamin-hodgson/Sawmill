#if NETSTANDARD2_1_OR_GREATER
namespace Sawmill
{
    internal class Box<T> where T : struct
    {
        // this is a public mutable field on purpose
        public T Value;
        public Box(T value)
        {
            Value = value;
        }
    }
}
#endif
