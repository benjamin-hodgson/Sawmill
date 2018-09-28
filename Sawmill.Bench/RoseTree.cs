using System;
using System.Collections.Immutable;

namespace Sawmill.Bench
{
    public class RoseTree : IRewritable<RoseTree>
    {
        public ImmutableList<RoseTree> Children { get; }

        public RoseTree(ImmutableList<RoseTree> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }
            Children = children;
        }

        public Children<RoseTree> GetChildren() => Children;

        public RoseTree SetChildren(Children<RoseTree> newChildren)
            => new RoseTree(newChildren.Many);

        public RoseTree RewriteChildren(Func<RoseTree, RoseTree> transformer)
            => this.DefaultRewriteChildren(transformer);
    }
}
