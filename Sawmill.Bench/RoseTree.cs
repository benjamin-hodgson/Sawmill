using System;
using System.Collections.Immutable;

namespace Sawmill.Bench
{
    public class RoseTree : IRewritable<RoseTree>
    {
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

        public void GetChildren(Span<RoseTree> children)
        {
            for (var i = 0; i < Children.Length; i++)
            {
                children[i] = Children[i];
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
}
