using System;
using System.Collections.Immutable;

namespace Sawmill.Tests
{
    class Tree<T> : IRewritable<Tree<T>>
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

        public Children<Tree<T>> GetChildren() => Sawmill.Children.Many(Children);

        public Tree<T> RewriteChildren(Func<Tree<T>, Tree<T>> transformer)
            => this.DefaultRewriteChildren(transformer);

        public Tree<T> SetChildren(Children<Tree<T>> newChildren) => new Tree<T>(Value, newChildren.Many.ToImmutableList());
    }
}
