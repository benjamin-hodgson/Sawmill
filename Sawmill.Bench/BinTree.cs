using System;

namespace Sawmill.Bench
{
    public class BinTree : IRewritable<BinTree>
    {
        public BinTree? Left { get; }
        public BinTree? Right { get; }

        public BinTree(BinTree? left, BinTree? right)
        {
            Left = left;
            Right = right;
        }

        public int CountChildren()
        {
            static int For(BinTree? x) => x == null ? 0 : 1;
            return For(Left) + For(Right);
        }

        public void GetChildren(Span<BinTree> children)
        {
            var i = 0;
            if (Left != null)
            {
                children[i] = Left;
                i++;
            }
            if (Right != null)
            {
                children[i] = Right;
                i++;
            }
        }

        public BinTree SetChildren(ReadOnlySpan<BinTree> newChildren)
        {
            if (Left == null && Right == null)
            {
                return this;
            }
            if (Left == null)
            {
                return new BinTree(Left, newChildren[0]);
            }
            if (Right == null)
            {
                return new BinTree(newChildren[0], Right);
            }
            return new BinTree(newChildren[0], newChildren[1]);
        }
    }
}
