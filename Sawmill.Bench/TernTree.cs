using System;

namespace Sawmill.Bench
{
    public class TernTree : IRewritable<TernTree>
    {
        public TernTree? Left { get; }
        public TernTree? Middle { get; }
        public TernTree? Right { get; }

        public TernTree(TernTree? left, TernTree? middle, TernTree? right)
        {
            Left = left;
            Middle = middle;
            Right = right;
        }

        public int CountChildren()
        {
            static int For(TernTree? x) => x == null ? 0 : 1;
            return For(Left) + For(Middle) + For(Right);
        }

        public void GetChildren(Span<TernTree> children)
        {
            var i = 0;
            if (Left != null)
            {
                children[i] = Left;
                i++;
            }
            if (Middle != null)
            {
                children[i] = Middle;
                i++;
            }
            if (Right != null)
            {
                children[i] = Right;
                i++;
            }
        }

        public TernTree SetChildren(ReadOnlySpan<TernTree> newChildren)
        {
            if (Left == null && Middle == null && Right == null)
            {
                return this;
            }
            
            var i = 0;
            TernTree? left = null;
            if (Left != null)
            {
                left = newChildren[i];
                i++;
            }
            TernTree? middle = null;
            if (Middle != null)
            {
                middle = newChildren[i];
                i++;
            }
            TernTree? right = null;
            if (Right != null)
            {
                right = newChildren[i];
                i++;
            }
            return new TernTree(left, middle, right);
        }
    }
}
