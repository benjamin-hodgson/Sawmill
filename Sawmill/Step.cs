using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sawmill
{
    [StructLayout(LayoutKind.Auto)]
    internal readonly struct Step<T>
    {
        public Stack<T> PrevSiblings { get; }
        public T Focus { get; }
        public Stack<T> NextSiblings { get; }
        public bool Changed { get; }

        public Step(
            Stack<T> prevSiblings,
            T focus,
            Stack<T> nextSiblings,
            bool changed
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
            Changed = changed;
        }
    }
}
