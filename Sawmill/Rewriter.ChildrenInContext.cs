using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Returns an instance of <see cref="Children{T}"/> containing each immediate child of
        /// <paramref name="value"/> paired with a function to replace the child.
        /// This is typically useful when you need to replace a node's children one at a time,
        /// such as during mutation testing.
        /// 
        /// <para>
        /// The replacement function can be seen as the "context" of the child; calling the
        /// function with a new child "plugs the hole" in the context.
        /// </para>
        /// 
        /// <seealso cref="SelfAndDescendantsInContext"/>
        /// <seealso cref="DescendantsAndSelfInContext"/>
        /// </summary>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="value">The value to get the contexts for the immediate children</param>
        public static Children<(T item, Func<T, T> replace)> ChildrenInContext<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            var children = rewriter.GetChildren(value);
            switch (children.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return Children.None<(T, Func<T, T>)>();
                case NumberOfChildren.One:
                    return Children.One<(T, Func<T, T>)>(
                        (
                            children.First,
                            newChild => ReferenceEquals(newChild, children.First)
                                ? value
                                : rewriter.SetChildren(Children.One(newChild), value)
                        )
                    );
                case NumberOfChildren.Two:
                    return Children.Two<(T, Func<T, T>)>(
                        (
                            children.First,
                            newChild => ReferenceEquals(newChild, children.First)
                                ? value
                                : rewriter.SetChildren(Children.Two(newChild, children.Second), value)
                        ),
                        (
                            children.Second,
                            newChild => ReferenceEquals(newChild, children.Second)
                                ? value
                                : rewriter.SetChildren(Children.Two(children.First, newChild), value)
                        )
                    );
                case NumberOfChildren.Many:
                    var list = children.Many.ToImmutableList();
                    var contexts = list.Select<T, (T, Func<T, T>)>(
                        (child, i) => (
                            child,
                            newChild => ReferenceEquals(child, newChild)
                                ? value
                                : rewriter.SetChildren(Children.Many(list.SetItem(i, newChild)), value)
                        )
                    );
                    return Children.Many(contexts.ToImmutableList());
                default:
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
            }
        }
    }
}