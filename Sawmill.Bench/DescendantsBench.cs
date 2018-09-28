using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Sawmill.Bench
{
    [MemoryDiagnoser]
    public class DescendantsBench
    {
        private readonly BinTree _tree = CreateBinTree(20);

        [Benchmark]
        public int SelfAndDescendants()
        {
            return _tree.SelfAndDescendants().Count();
        }

        [Benchmark]
        public int DescendantsAndSelf()
        {
            return _tree.DescendantsAndSelf().Count();
        }

        [Benchmark]
        public int SelfAndDescendantsBreadthFirst()
        {
            return _tree.SelfAndDescendantsBreadthFirst().Count();
        }


        private static BinTree CreateBinTree(int depth)
            => depth == 0
                ? null
                : new BinTree(CreateBinTree(depth - 1), CreateBinTree(depth - 1));
    }
}
