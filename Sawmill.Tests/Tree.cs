using System.Collections.Immutable;

namespace Sawmill.Tests;

internal class Tree<T> : IRewritable<Tree<T>>
{
    public T Value { get; }
    public ImmutableList<Tree<T>> Children { get; }

    public Tree(T value, ImmutableList<Tree<T>> children)
    {
        if (children == null)
        {
            throw new ArgumentNullException(nameof(children));
        }
        Value = value;
        Children = children;
    }

    public override string? ToString() => Value?.ToString();

    public int CountChildren() => Children.Count;

    public void GetChildren(Span<Tree<T>> children)
    {
        for (var i = 0; i < Children.Count; i++)
        {
            children[i] = Children[i];
        }
    }

    public Tree<T> SetChildren(ReadOnlySpan<Tree<T>> newChildren) => new(Value, newChildren.ToArray().ToImmutableList());
}
