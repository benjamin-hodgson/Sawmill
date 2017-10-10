using System;
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
                ImmutableStack.Create<Step<T>>(),
                ImmutableStack.Create<Scarred<T>>(),
                Scarred.Create(rewriter, value),
                ImmutableStack.Create<Scarred<T>>(),
                false                
            );
        }
    }
}