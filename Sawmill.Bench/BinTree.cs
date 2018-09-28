using System;

namespace Sawmill.Bench
{
    public class BinTree : IRewritable<BinTree>
    {
        public BinTree Left { get; }
        public BinTree Right { get; }

        public BinTree(BinTree left, BinTree right)
        {
            Left = left;
            Right = right;
        }

        public Children<BinTree> GetChildren()
        {
            if (Left == null && Right == null)
            {
                return Children.None<BinTree>();
            }
            if (Left == null)
            {
                return Right;
            }
            if (Right == null)
            {
                return Left;
            }
            return (Left, Right);
        }

        public BinTree SetChildren(Children<BinTree> newChildren)
        {
            if (Left == null && Right == null)
            {
                return this;
            }
            if (Left == null)
            {
                return new BinTree(Left, newChildren.First);
            }
            if (Right == null)
            {
                return new BinTree(newChildren.First, Right);
            }
            return new BinTree(newChildren.First, newChildren.Second);
        }

        public BinTree RewriteChildren(Func<BinTree, BinTree> transformer)
            => this.DefaultRewriteChildren(transformer);
    }
}
