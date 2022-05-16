namespace Sawmill.Bench;

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
        }
    }

    public BinTree SetChildren(ReadOnlySpan<BinTree> newChildren)
        => Left == null && Right == null
            ? this
            : Left == null
                ? new BinTree(Left, newChildren[0])
                : Right == null
                    ? new BinTree(newChildren[0], Right)
                    : new BinTree(newChildren[0], newChildren[1]);
}
