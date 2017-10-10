using System;
using System.Collections.Immutable;

namespace Sawmill
{
    internal struct Step<T>
    {
        public ImmutableStack<Scarred<T>> PrevSiblings { get; }
        public Scarred<T> Focus { get; }
        public ImmutableStack<Scarred<T>> NextSiblings { get; }

        public bool FocusOrSiblingsChanged { get; }

        public Step(
            ImmutableStack<Scarred<T>> prevSiblings,
            Scarred<T> focus,
            ImmutableStack<Scarred<T>> nextSiblings,
            bool focusOrSiblingsChanged
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
            FocusOrSiblingsChanged = focusOrSiblingsChanged;
        }
    }
}