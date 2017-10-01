using System;
using System.Collections.Generic;

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
        /// <param name="transformer">A transformation function to apply to each of <paramref name="oldValue"/>'s immediate children.</param>
        /// <param name="oldValue">The old value, whose immediate children should be transformed by <paramref name="transformer"/>.</param>
        /// <returns>A copy of <paramref name="oldValue"/> with updated children.</returns>
        public static T DefaultRewriteChildren<T>(this IRewriter<T> rewriter, Func<T, T> transformer, T oldValue)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }


            var children = rewriter.GetChildren(oldValue);
            var changed = false;
            Children<T> newChildren;

            switch (children.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return oldValue;
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
                    var list = many is ICollection<T> c ? new List<T>(c.Count) : new List<T>();
                    foreach (var child in many)
                    {
                        var newChild = transformer(child);
                        list.Add(newChild);
                        if (!ReferenceEquals(newChild, child))
                        {
                            changed = true;
                        }
                    }
                    newChildren = Children.Many(list);
                    break;
                }
                default:
                {
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
                }
            }


            return changed ? rewriter.SetChildren(newChildren, oldValue) : oldValue;
        }
    }
}