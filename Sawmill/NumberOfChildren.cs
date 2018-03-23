namespace Sawmill
{
    /// <summary>
    /// The various possible states of an instance of <see cref="Children{T}"/>
    /// <seealso cref="Children{T}.NumberOfChildren"/>
    /// </summary>
    public enum NumberOfChildren : byte
    {
        /// <summary>
        /// The <see cref="Children{T}"/> contains no elements.
        /// </summary>
        None,
        /// <summary>
        /// The <see cref="Children{T}"/> contains one element.
        /// </summary>
        One,
        /// <summary>
        /// The <see cref="Children{T}"/> contains two elements.
        /// </summary>
        Two,
        /// <summary>
        /// The <see cref="Children{T}"/> contains an arbitrary number of elements.
        /// </summary>
        Many
    }
}
