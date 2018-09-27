using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Sawmill.Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Bench>();
        }
    }

    [MemoryDiagnoser]
    public class Bench
    {
        private BinTree _tree = CreateBinTree(20);

        [Benchmark(Baseline = true)]
        public void SelfAndDescendants()
        {
            _tree.SelfAndDescendants().Count();
        }

        [Benchmark]
        public void DescendantsAndSelf()
        {
            _tree.DescendantsAndSelf().Count();
        }

        [Benchmark]
        public void SelfAndDescendantsBreadthFirst()
        {
            _tree.SelfAndDescendantsBreadthFirst().Count();
        }


        private static BinTree CreateBinTree(int depth)
            => depth == 0
                ? null
                : new BinTree(CreateBinTree(depth - 1), CreateBinTree(depth - 1));
    }

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
