namespace Sawmill;

/// <summary>
/// Represents a direction that a <see cref="Cursor{T}" /> can move in.
/// </summary>
public enum Direction
{
    /// <summary>
    /// Move to the parent.
    /// </summary>
    Up,

    /// <summary>
    /// Move to the first child.
    /// </summary>
    Down,

    /// <summary>
    /// Move to the previous sibling.
    /// </summary>
    Left,

    /// <summary>
    /// Move to the next sibling.
    /// </summary>
    Right
}
