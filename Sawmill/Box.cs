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
