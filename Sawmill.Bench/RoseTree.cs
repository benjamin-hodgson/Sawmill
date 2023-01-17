using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Sawmill.Bench;

public class RoseTree : IRewritable<RoseTree>
{
    [SuppressMessage("naming", "CA1721", Justification = "bench")] // "The property name 'Children' is confusing given the existence of method 'GetChildren'"
    public ImmutableArray<RoseTree> Children { get; }

    public RoseTree(ImmutableArray<RoseTree> children)
    {
        if (children == null)
        {
            throw new ArgumentNullException(nameof(children));
        }

        Children = children;
    }

    public int CountChildren() => Children.Length;

    public void GetChildren(Span<RoseTree> childrenReceiver)
    {
        for (var i = 0; i < Children.Length; i++)
        {
            childrenReceiver[i] = Children[i];
        }
    }

    public RoseTree SetChildren(ReadOnlySpan<RoseTree> newChildren)
    {
        var builder = ImmutableArray.CreateBuilder<RoseTree>(newChildren.Length);
        foreach (var child in newChildren)
        {
            builder.Add(child);
        }

        return new RoseTree(builder.ToImmutable());
    }
}
