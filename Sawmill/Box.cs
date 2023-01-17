namespace Sawmill;

internal class Box<T>
    where T : struct
{
#pragma warning disable SA1401  // "Field should be private"
    // this is a public mutable field on purpose
    public T Value;
#pragma warning restore SA1401

    public Box()
    {
        Value = default;
    }

    public Box(T value)
    {
        Value = value;
    }
}
