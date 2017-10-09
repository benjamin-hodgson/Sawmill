using System;
using System.Collections.Immutable;

namespace Sawmill
{
    internal struct Step<T>
    {
        public ImmutableStack<Scarred<T>> PrevSiblings { get; }
        public Scarred<T> Focus { get; }
        public ImmutableStack<Scarred<T>> NextSiblings { get; }

        public Step(
            ImmutableStack<Scarred<T>> prevSiblings,
            Scarred<T> focus,
            ImmutableStack<Scarred<T>> nextSiblings
        )
        {
            if (prevSiblings == null)
            {
                throw new ArgumentNullException(nameof(prevSiblings));
            }
            if (focus == null)
            {
                throw new ArgumentNullException(nameof(focus));
            }
            if (nextSiblings == null)
            {
                throw new ArgumentNullException(nameof(nextSiblings));
            }

            PrevSiblings = prevSiblings;
            Focus = focus;
            NextSiblings = nextSiblings;
        }
    }
}