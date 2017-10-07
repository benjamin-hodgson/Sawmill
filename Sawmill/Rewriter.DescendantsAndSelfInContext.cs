using System;
using System.Collections.Generic;

namespace Sawmill
{
    public static partial class Rewriter
    {
        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContext<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            var result = new List<(T item, Func<T, T> replace)>();

            void Go(T t, Func<T, T> replace)
            {
                foreach (var (child, replaceChild) in rewriter.ChildrenInContext(t))
                {
                    Go(child, newDescendant => replace(replaceChild(newDescendant)));
                }

                result.Add((t, replace));                
            }
            Go(value, x => x);
            return result;
        }

        public static IEnumerable<(T item, Func<T, T> replace)> DescendantsAndSelfInContextLazy<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }

            IEnumerable<(T item, Func<T, T> context)> Go(T t)
            {
                foreach (var (child, replaceChild) in ChildrenInContext(rewriter, t))
                {
                    foreach (var (descendant, replaceDescendant) in Go(child))
                    {
                        yield return (descendant, newDescendant => replaceChild(replaceDescendant(newDescendant)));
                    }
                }
                
                yield return (t, newT => newT);                
            }
            return Go(value);
        }
    }
}