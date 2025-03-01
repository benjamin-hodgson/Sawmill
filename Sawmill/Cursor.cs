using System;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill;

/// <summary>
/// A <see cref="Cursor{T}" /> is a mutable view of a location in a <typeparamref name="T" />-structure,
/// allowing efficient access to (and editing of) a node and its parent, siblings, and immediate children.
///
/// <para>
/// You can think of a <see cref="Cursor{T}" /> as being focused on a particular node.
/// After zooming in on a node, you can efficiently go up to the node's parent, down to the node's first child,
/// or left or right to the node's immediate siblings.
/// </para>
///
/// <para>
/// <see cref="Cursor{T}" /> is generally not as efficient or useful as the
/// <see cref="Rewriter.SelfAndDescendantsInContext{T}(IRewriter{T}, T)" /> family for replacing single nodes,
/// but it efficiently supports longer sequences of edits to a location and its neighbours.
/// </para>
/// </summary>
///
/// <example>
/// Here we traverse to, and replace, the right child of a binary node.
/// <code>
/// Expr expr = new Add(new Lit(1), new Neg(new Lit(2)));
/// var cursor = expr.Cursor();
/// cursor.Down();
/// cursor.Right();
///
/// Assert.Equal(new Neg(new Lit(2)), cursor.Focus);
///
/// cursor.Focus = new Lit(10);
/// cursor.Top();
///
/// Assert.Equal(new Add(new Lit(1), new Lit(10)), cursor.Focus);
/// </code>
/// </example>
/// <typeparam name="T">The rewritable tree type.</typeparam>
public sealed class Cursor<T>
{
    private readonly IRewriter<T> _rewriter;

    private readonly Stack<Step<T>> _path;

    private Stack<T> _prevSiblings;

    private T _focus;

    /// <summary>
    /// Gets or sets the current focus of the <see cref="Cursor{T}" />.
    /// </summary>
    /// <returns>The current focus of the <see cref="Cursor{T}" />.</returns>
    public T Focus
    {
        get
        {
            return _focus;
        }

        set
        {
            _changed = _changed || !ReferenceEquals(_focus, value);
            _focus = value;
        }
    }

    private Stack<T> _nextSiblings;

    private bool _changed;

    internal Cursor(IRewriter<T> rewriter, T top)
    {
        ArgumentNullException.ThrowIfNull(rewriter);

        _rewriter = rewriter;
        _path = new Stack<Step<T>>();
        _prevSiblings = new Stack<T>();
        _focus = top;
        _nextSiblings = new Stack<T>();
    }

    /// <summary>
    /// Move in a given <see cref="Direction" />.
    /// </summary>
    /// <param name="direction">The <see cref="Direction" /> to move in.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the direction leads off the edge of the tree. The <see cref="Cursor{T}" /> is left in the last known good state.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="direction" /> is not a valid <see cref="Direction" />.
    /// </exception>
    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                Up();
                return;
            case Direction.Down:
                Down();
                return;
            case Direction.Left:
                Left();
                return;
            case Direction.Right:
                Right();
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction));
        }
    }

    /// <summary>
    /// Try to move in a given <see cref="Direction" />.
    /// </summary>
    /// <param name="direction">The <see cref="Direction" /> to move in.</param>
    /// <returns>
    /// True if the operation was successful, false if the direction leads off the edge of the tree.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="direction" /> is not a valid <see cref="Direction" />.
    /// </exception>
    public bool TryMove(Direction direction)
        => direction switch
        {
            Direction.Up => TryUp(),
            Direction.Down => TryDown(),
            Direction.Left => TryLeft(),
            Direction.Right => TryRight(),
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s parent.
    ///
    /// <para>
    /// Going <see cref="Up()" /> "plugs the hole" in the parent, replacing the parent's children as necessary.
    /// This releases old versions of the current <see cref="Focus" /> and its children so that they can be garbage collected.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">The <see cref="Cursor{T}" /> is already focused on the root node.</exception>
    public void Up()
    {
        if (!TryUp())
        {
            throw new InvalidOperationException("Can't go up from the top of a cursor");
        }
    }

    /// <summary>
    /// Go <see cref="Up()" /> <paramref name="count" /> times.
    ///
    /// <para>
    /// Going <see cref="Up(int)" /> "plugs the hole" in the ancestors, replacing their children as necessary.
    /// This releases old versions of the ancestors and their children, so that they can be garbage collected.
    /// </para>
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the root node.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, at the top of the tree.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    public void Up(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        while (count > 0)
        {
            Up();
            count--;
        }
    }

    /// <summary>
    /// Try to focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s parent.
    ///
    /// <para>
    /// This operation "plugs the hole" in the parent, replacing the parent's children as necessary.
    /// This releases old versions of the current <see cref="Focus" /> and its children, so that they can be garbage collected.
    /// </para>
    /// </summary>
    /// <returns>
    /// True if the operation was successful, false if the <see cref="Cursor{T}" /> is already focused on the root node.
    /// </returns>
    public bool TryUp()
    {
        if (_path.Count == 0)
        {
            return false;
        }

        var parent = _path.Pop();
        _focus = _changed ? SetChildren(parent.Focus) : parent.Focus;
        _prevSiblings = parent.PrevSiblings;
        _nextSiblings = parent.NextSiblings;
        _changed = parent.Changed;
        return true;
    }

    /// <summary>
    /// Go <see cref="Up()" /> <paramref name="count" /> times, stopping if you reach the top.
    ///
    /// <para>
    /// This operation "plugs the hole" in the parent, replacing the parent's children as necessary.
    /// This releases old versions of the ancestors and their children, so that they can be garbage collected.
    /// </para>
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    /// <returns>
    /// True if the operation was successful, false if the cursor went <see cref="Up()" /> fewer than <paramref name="count" /> times.
    /// </returns>
    public bool TryUp(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        var success = true;
        while (count > 0 && success)
        {
            success = TryUp();
            count--;
        }

        return success;
    }

    /// <summary>
    /// Move the <see cref="Cursor{T}" /> to the top of the tree.
    ///
    /// <para>
    /// This operation "plugs the hole" in all of the current node's ancestors, replacing their children as necessary.
    /// Going to the <see cref="Top" /> releases old versions of the tree so that they can be garbage collected.
    /// </para>
    /// </summary>
    public void Top()
    {
        var success = true;
        while (success)
        {
            success = TryUp();
        }
    }

    /// <summary>
    /// Go <see cref="Up()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />.
    /// In other words, find the first ancestor of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    ///
    /// <para>
    /// This operation "plugs the hole" in the ancestors, replacing their children as necessary.
    /// This releases old versions of the ancestors and their children, so that they can be garbage collected.
    /// </para>
    /// </summary>
    ///
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the root node.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, at the top of the tree.
    /// </exception>
    public void UpWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        while (predicate(Focus))
        {
            Up();
        }
    }

    /// <summary>
    /// Go <see cref="Up()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />, stopping if you reach the top.
    /// In other words, find the first ancestor of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    ///
    /// <para>
    /// This operation "plugs the hole" in the ancestors, replacing their children as necessary.
    /// This releases old versions of the ancestors and their children, so that they can be garbage collected.
    /// </para>
    /// </summary>
    ///
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <returns>
    /// True if an ancestor not satisfying <paramref name="predicate" /> was found, false if the <see cref="Cursor{T}" /> reached the top.
    /// </returns>
    public bool TryUpWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var success = true;
        while (success && predicate(Focus))
        {
            success = TryUp();
        }

        return success;
    }

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s first child.
    ///
    /// <para>
    /// This operation "opens a hole" in the current node, descending to the children so you can replace them one at a time.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">The current <see cref="Focus" />'s has no children.</exception>
    public void Down()
    {
        if (!TryDown())
        {
            throw new InvalidOperationException("Can't go down from here, focus has no children");
        }
    }

    /// <summary>
    /// Go <see cref="Down()" /> <paramref name="count" /> times.
    ///
    /// <para>
    /// This operation "opens a hole" in the current node and its <paramref name="count" /> first descendants.
    /// </para>
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached a node with no children.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, at the bottom of the tree.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    public void Down(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        while (count > 0)
        {
            Down();
            count--;
        }
    }

    /// <summary>
    /// Try to focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s first child.
    ///
    /// <para>
    /// This operation "opens a hole" in the current node, descending to the children so you can replace them one at a time.
    /// </para>
    /// </summary>
    /// <returns>True if the operation was successful, false if the current <see cref="Focus" /> has no children.</returns>
    public bool TryDown()
    {
        var children = _rewriter.GetChildren(Focus);
        if (children.Length == 0)
        {
            return false;
        }

        _path.Push(new Step<T>(_prevSiblings, Focus, _nextSiblings, _changed));
        _prevSiblings = new Stack<T>();
        _nextSiblings = new Stack<T>(children.Reverse());
        _focus = _nextSiblings.Pop();
        _changed = false;
        return true;
    }

    /// <summary>
    /// Go <see cref="Down()" /> <paramref name="count" /> times, stopping if you reach a node with no children.
    ///
    /// <para>
    /// This operation "opens a hole" in the current node and its <paramref name="count" /> first descendants.
    /// </para>
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    /// <returns>
    /// True if the operation was successful, false if the cursor went <see cref="Down()" /> fewer than <paramref name="count" /> times.
    /// </returns>
    public bool TryDown(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        var success = true;
        while (count > 0 && success)
        {
            success = TryDown();
            count--;
        }

        return success;
    }

    /// <summary>
    /// Move the current <see cref="Focus" /> to the bottom-left of the tree.
    /// Do nothing if the current <see cref="Focus" /> has no children.
    /// </summary>
    public void Bottom()
    {
        var success = true;
        while (success)
        {
            success = TryDown();
        }
    }

    /// <summary>
    /// Go <see cref="Down()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />.
    /// In other words, find the first leftmost descendant of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its leftmost descendants.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the root node.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, at the bottom of the tree.
    /// </exception>
    public void DownWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        while (predicate(Focus))
        {
            Down();
        }
    }

    /// <summary>
    /// Go <see cref="Down()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />, stopping if you reach the bottom.
    /// In other words, find the first leftmost descendant of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its leftmost descendants.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <returns>
    /// True if a leftmost descendant not satisfying <paramref name="predicate" /> was found, false if the <see cref="Cursor{T}" /> reached the bottom.
    /// </returns>
    public bool TryDownWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var success = true;
        while (success && predicate(Focus))
        {
            success = TryDown();
        }

        return success;
    }

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s immediate predecessor sibling.
    /// </summary>
    /// <exception cref="InvalidOperationException">The <see cref="Cursor{T}" /> is already focused on the leftmost sibling.</exception>
    public void Left()
    {
        if (!TryLeft())
        {
            throw new InvalidOperationException("Can't go left from here, already at the leftmost sibling");
        }
    }

    /// <summary>
    /// Go <see cref="Left()" /> <paramref name="count" /> times.
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the leftmost sibling.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, focused on the leftmost sibling.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    public void Left(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        while (count > 0)
        {
            Left();
            count--;
        }
    }

    /// <summary>
    /// Try to focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s immediate predecessor sibling.
    /// </summary>
    /// <returns>
    /// True if the operation was successful, false if the <see cref="Cursor{T}" /> is already focused on the leftmost sibling.
    /// </returns>
    public bool TryLeft()
    {
        if (_prevSiblings.Count == 0)
        {
            return false;
        }

        _nextSiblings.Push(Focus);
        _focus = _prevSiblings.Pop();
        return true;
    }

    /// <summary>
    /// Go <see cref="Left()" /> <paramref name="count" /> times, stopping if you reach the leftmost sibling.
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    /// <returns>
    /// True if the operation was successful, false if the cursor went <see cref="Left()" /> fewer than <paramref name="count" /> times.
    /// </returns>
    public bool TryLeft(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        var success = true;
        while (count > 0 && success)
        {
            success = TryLeft();
            count--;
        }

        return success;
    }

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s leftmost sibling.
    /// Do nothing if the <see cref="Cursor{T}" /> is already focused on the leftmost sibling.
    /// </summary>
    public void Leftmost()
    {
        var success = true;
        while (success)
        {
            success = TryLeft();
        }
    }

    /// <summary>
    /// Go <see cref="Left()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />.
    /// In other words, find the first left sibling of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the root node.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, at the leftmost sibling.
    /// </exception>
    public void LeftWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        while (predicate(Focus))
        {
            Left();
        }
    }

    /// <summary>
    /// Go <see cref="Left()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />, stopping if you reach the leftmost sibling.
    /// In other words, find the left sibling of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <returns>
    /// True if an ancestor not satisfying <paramref name="predicate" /> was found, false if the <see cref="Cursor{T}" /> reached the leftmost sibling.
    /// </returns>
    public bool TryLeftWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var success = true;
        while (success && predicate(Focus))
        {
            success = TryLeft();
        }

        return success;
    }

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s immediate successor sibling.
    /// </summary>
    /// <exception cref="InvalidOperationException">The <see cref="Cursor{T}" /> is already focused on the rightmost sibling.</exception>
    public void Right()
    {
        if (!TryRight())
        {
            throw new InvalidOperationException("Can't go left from here, already at the rightmost sibling");
        }
    }

    /// <summary>
    /// Go <see cref="Right()" /> <paramref name="count" /> times.
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the rightmost sibling.
    /// The <see cref="Cursor{T}" /> is left in the last good state, that is, focused on the rightmost sibling.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    public void Right(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        while (count > 0)
        {
            Right();
            count--;
        }
    }

    /// <summary>
    /// Try to focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s immediate successor sibling.
    /// </summary>
    /// <returns>True if the operation was successful, false if the <see cref="Cursor{T}" /> is already focused on the rightmost sibling.</returns>
    public bool TryRight()
    {
        if (_nextSiblings.Count == 0)
        {
            return false;
        }

        _prevSiblings.Push(Focus);
        _focus = _nextSiblings.Pop();
        return true;
    }

    /// <summary>
    /// Go <see cref="Right()" /> <paramref name="count" /> times, stopping if you reach the rightmost sibling.
    /// </summary>
    /// <param name="count">The distance.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> was negative.</exception>
    /// <returns>
    /// True if the operation was successful, false if the cursor went <see cref="Right()" /> fewer than <paramref name="count" /> times.
    /// </returns>
    public bool TryRight(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be non-negative");
        }

        var success = true;
        while (count > 0 && success)
        {
            success = TryRight();
            count--;
        }

        return success;
    }

    /// <summary>
    /// Focus the <see cref="Cursor{T}" /> on the current <see cref="Focus" />'s rightmost sibling.
    /// Do nothing if the <see cref="Cursor{T}" /> is already focused on the rightmost sibling.
    /// </summary>
    public void Rightmost()
    {
        var success = true;
        while (success)
        {
            success = TryRight();
        }
    }

    /// <summary>
    /// Go <see cref="Right()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />.
    /// In other words, find the first Right sibling of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Cursor{T}" /> reached the root node.
    /// The <see cref="Cursor{T}" /> is Right in the last good state, that is, at the Rightmost sibling.
    /// </exception>
    public void RightWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        while (predicate(Focus))
        {
            Right();
        }
    }

    /// <summary>
    /// Go <see cref="Right()" /> as long as <paramref name="predicate" /> returns true for the current <see cref="Focus" />, stopping if you reach the Rightmost sibling.
    /// In other words, find the Right sibling of <see cref="Focus" /> (including itself) which does not satisfy <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">The predicate to invoke on the current focus and its ancestors.</param>
    /// <exception name="ArgumentNullException"><paramref name="predicate" /> was null.</exception>
    /// <returns>
    /// True if an ancestor not satisfying <paramref name="predicate" /> was found, false if the <see cref="Cursor{T}" /> reached the Rightmost sibling.
    /// </returns>
    public bool TryRightWhile(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var success = true;
        while (success && predicate(Focus))
        {
            success = TryRight();
        }

        return success;
    }

    /// <summary>
    /// Yields a sequence of <see cref="Direction" />s describing how to get from the <see cref="Top" />
    /// of the tree to the current <see cref="Focus" />.
    /// The resulting path can be <see cref="Follow" />ed by a <see cref="Cursor{T}" />.
    /// This is useful if, for example, you need to compare the nodes at a given position in two different trees.
    /// </summary>
    /// <returns>A sequence of <see cref="Direction" />s.</returns>
    public IEnumerable<Direction> GetPath()
    {
        var prevSiblingsThisLevel = _prevSiblings.Count;
        var prevSiblingsAncestors = _path.Select(x => x.PrevSiblings.Count).ToList();

        IEnumerable<Direction> Iterator()
        {
            foreach (var x in prevSiblingsAncestors)
            {
                for (var i = 0; i < x; i++)
                {
                    yield return Direction.Right;
                }

                yield return Direction.Down;
            }

            for (var i = 0; i < prevSiblingsThisLevel; i++)
            {
                yield return Direction.Right;
            }
        }

        return Iterator();
    }

    /// <summary>
    /// Follow a path.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when path leads off the edge of the tree. The <see cref="Cursor{T}" /> is left in the last known good state.
    /// </exception>
    /// <param name="path">The path to follow.</param>
    public void Follow(IEnumerable<Direction> path)
    {
        ArgumentNullException.ThrowIfNull(path);

        foreach (var direction in path)
        {
            Move(direction);
        }
    }

    /// <summary>
    /// Follow a path.
    /// </summary>
    /// <param name="path">The path to follow.</param>
    /// <returns>
    /// True if the path was successfully followed in full, false if the path led off the edge of the tree.
    /// </returns>
    public bool TryFollow(IEnumerable<Direction> path)
    {
        ArgumentNullException.ThrowIfNull(path);

        foreach (var direction in path)
        {
            if (!TryMove(direction))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Release old versions of the tree for garbage collection.
    /// The <see cref="Cursor{T}" /> is left focused on the current node.
    ///
    /// <para>
    /// Typically you won't need to call this method yourself -
    /// just call <see cref="Top" /> at the end of your sequence of edits to get the new tree back.
    /// (This method is equivalent to calling <see cref="Top" /> and then returning to where you were.)
    /// </para>
    ///
    /// <para>
    /// The worst-case scenario for <see cref="Cursor{T}" />'s memory usage is code which traverses a large tree and
    /// alternates <see cref="Down()" /> calls with setting the <see cref="Focus" />,
    /// without any calls to <see cref="Up()" /> in between.
    /// If this is a typical usage pattern for your application, and you find that <see cref="Cursor{T}" />
    /// is causing high memory usage because it's holding on to old trees, some infrequent calls to
    /// this method (say, every 1000 edits) should improve the memory usage (at the cost of some speed).
    /// </para>
    /// </summary>
    public void ReleaseOldVersions()
    {
        var path = GetPath();
        Top();
        Follow(path);
    }

    /// <summary>
    /// Focus the current focus's first descendant or right sibling's descendant which satisfies <paramref name="predicate" />,
    /// searching descendants before siblings and ending at the current node's rightmost sibling.
    /// </summary>
    ///
    /// <remarks>
    /// This function searches the bottom-left part of the tree first, so will typically end up focusing a node lower down than <see cref="SearchRightAndDown" />.
    /// </remarks>
    ///
    /// <seealso cref="Rewriter.SelfAndDescendants{T}(IRewriter{T}, T)" />
    /// <param name="predicate">A predicate which returns true when the search should stop.</param>
    /// <returns>True if a matching focus was found, false if the search was exhaustive.</returns>
    public bool SearchDownAndRight(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        bool Go()
        {
            if (predicate(Focus))
            {
                return true;
            }

            if (TryDown())
            {
                if (Go())
                {
                    return true;
                }

                Up();
            }

            if (TryRight())
            {
                if (Go())
                {
                    return true;
                }
            }

            return false;
        }

        return Go();
    }

    /// <summary>
    /// Focus the current focus's first descendant or right sibling's descendant which satisfies <paramref name="predicate" />,
    /// searching siblings before descendants and ending at the current node's lowest leftmost descendant.
    ///
    /// <para>
    /// This function searches the top-right part of the tree first, so will typically end up focusing a node higher up than <see cref="SearchDownAndRight" />.
    /// </para>
    /// </summary>
    /// <param name="predicate">A predicate which returns true when the search should stop.</param>
    /// <returns>True if a matching focus was found, false if the search was exhaustive.</returns>
    public bool SearchRightAndDown(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        bool Go()
        {
            if (predicate(Focus))
            {
                return true;
            }

            if (TryRight())
            {
                if (Go())
                {
                    return true;
                }

                Left();
            }

            if (TryDown())
            {
                if (Go())
                {
                    return true;
                }
            }

            return false;
        }

        return Go();
    }

    private T SetChildren(T value)
    {
        var children = _rewriter.GetChildren(value);

        _nextSiblings.Push(Focus);
        while (_prevSiblings.Count > 0)
        {
            _nextSiblings.Push(_prevSiblings.Pop());
        }

        var changed = false;
        for (var i = 0; i < children.Length; i++)
        {
            var oldItem = children[i];
            var newItem = _nextSiblings.Pop();

            if (!ReferenceEquals(oldItem, newItem))
            {
                children[i] = newItem;
                changed = true;
            }
        }

        return changed
            ? _rewriter.SetChildren(children, value)
            : value;
    }
}
