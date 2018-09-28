using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Update the immediate children of the value by applying a transformation function to each one.
        /// <para>
        /// Most implementations of <see cref="IRewriter{T}.RewriteChildren(Func{T, T}, T)"/> will simply delegate to this method.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The rewritable tree type</typeparam>
        /// <param name="rewriter">The rewriter</param>
        /// <param name="transformer">A transformation function to apply to each of <paramref name="value"/>'s immediate children.</param>
        /// <param name="value">The old value, whose immediate children should be transformed by <paramref name="transformer"/>.</param>
        /// <returns>A copy of <paramref name="value"/> with updated children.</returns>
        public static T DefaultRewriteChildren<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }


            var children = rewriter.GetChildren(value);
            var changed = false;
            Children<T> newChildren;

            switch (children.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return value;
                case NumberOfChildren.One:
                {
                    var child = children.First;
                    var newChild = transformer(child);
                    if (!ReferenceEquals(newChild, child))
                    {
                        changed = true;
                    }

                    newChildren = Children.One(newChild);
                    break;
                }
                case NumberOfChildren.Two:
                {
                    var first = children.First;
                    var newFirst = transformer(first);
                    if (!ReferenceEquals(newFirst, first))
                    {
                        changed = true;
                    }

                    var second = children.Second;
                    var newSecond = transformer(second);
                    if (!ReferenceEquals(newSecond, second))
                    {
                        changed = true;
                    }

                    newChildren = Children.Two(newFirst, newSecond);
                    break;
                }
                case NumberOfChildren.Many:
                {
                    var many = children.Many;
                    var builder = many.ToBuilder();

                    var i = 0;
                    foreach (var oldChild in many)
                    {
                        var newChild = transformer(oldChild);

                        if (!ReferenceEquals(oldChild, newChild))
                        {
                            changed = true;
                            builder[i] = newChild;
                        }
                        i++;
                    }

                    newChildren = Children.Many(builder.ToImmutable());

                    break;
                }
                default:
                {
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
                }
            }


            return changed ? rewriter.SetChildren(newChildren, value) : value;
        }
    }
}
