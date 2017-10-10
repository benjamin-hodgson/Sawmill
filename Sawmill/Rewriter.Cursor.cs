using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill
{
    public static partial class Rewriter
    {
        public static Cursor<T> Cursor<T>(this IRewriter<T> rewriter, T value)
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            
            return new Cursor<T>(
                rewriter,
                new Stack<Step<T>>(),
                new Stack<T>(),
                value,
                new Stack<T>()
            );
        }
    }
}