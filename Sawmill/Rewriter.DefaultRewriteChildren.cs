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

                    var mapped = EnumerableBuilder<T>.Map(many, transformer);

                    if (mapped.HasValue)
                    {
                        changed = mapped.Value.changed;
                        newChildren = Children.Many(mapped.Value.result);
                    }
                    else  // wasn't a type we know how to rebuild. let's just build an ImmutableArray
                    {
                        var builder = many is ICollection<T> c
                            ? ImmutableArray.CreateBuilder<T>(c.Count)
                            : ImmutableArray.CreateBuilder<T>();
                        
                        foreach (var oldChild in many)
                        {
                            var newChild = transformer(oldChild);
                            changed = changed || !ReferenceEquals(oldChild, newChild);
                            builder.Add(newChild);
                        }

                        newChildren = Children.Many(ToImmutableAndClear(builder));
                    }

                    break;
                }
                default:
                {
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
                }
            }


            return changed ? rewriter.SetChildren(newChildren, oldValue) : oldValue;
        }

        private static ImmutableArray<T> ToImmutableAndClear<T>(ImmutableArray<T>.Builder builder)
        {
            if (builder.Capacity == builder.Count)
            {
                return builder.MoveToImmutable();
            }
            var array = builder.ToImmutable();
            builder.Clear();
            return array;
        }
    }
}