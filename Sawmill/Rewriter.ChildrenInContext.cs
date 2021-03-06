using System;
using System.Buffers;

namespace Sawmill
{
    public static partial class Rewriter
    {
        /// <summary>
        /// Returns an array containing each immediate child of
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
        public static (T item, Func<T, T> replace)[] ChildrenInContext<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            var stack = new ChunkStack<T>();

            var result = rewriter.WithChildren(
                (children, tup) =>
                {
                    var (r, v) = tup;

                    var array = new (T item, Func<T, T> replace)[children.Length];

                    for (var i = 0; i < children.Length; i++)
                    {
                        var j = i;
                        array[i] = (children[i], newChild => r.ReplaceChild(j, newChild, v));
                    }
                    return array;
                },
                (rewriter, value),
                value,
                ref stack
            );

            stack.Dispose();
            
            return result;
        }

        private static T ReplaceChild<T>(this IRewriter<T> rewriter, int position, T newChild, T value)
        {
            var count = rewriter.CountChildren(value);

            if (position >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(position), position, "Index was out of bounds");
            }

            var array = ArrayPool<T>.Shared.Rent(count);
            var span = array.AsSpan().Slice(0, count);
            
            rewriter.GetChildren(span, value);
            array[position] = newChild;
            return rewriter.SetChildren(span, value);
        }
    }
}
