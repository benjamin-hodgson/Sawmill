using System.Collections.Immutable;

namespace Sawmill.Tests;

internal sealed record Tree<T>(T Value, ImmutableList<Tree<T>> Children) : IRewritable<Tree<T>>
{
    public int CountChildren() => Children.Count;

    public void GetChildren(Span<Tree<T>> children)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            children[i] = Children[i];
        }
    }

    public Tree<T> SetChildren(ReadOnlySpan<Tree<T>> newChildren)
        => new(Value, newChildren.ToArray().ToImmutableList());
}
