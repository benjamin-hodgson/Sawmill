using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Sawmill.Bench
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class DescendantsBench
    {
        private readonly BinTree _binTree = CreateBinTree(19)!;
        private readonly TernTree _ternTree = CreateTernTree(12)!;

        [Benchmark]
        [BenchmarkCategory("BinTree")]
        public int BinTree_SelfAndDescendants()
        {
            return _binTree.SelfAndDescendants().Count();
        }
        [Benchmark]
        [BenchmarkCategory("BinTree")]
        public int BinTree_DescendantsAndSelf()
        {
            return _binTree.DescendantsAndSelf().Count();
        }
        [Benchmark]
        [BenchmarkCategory("BinTree")]
        public int BinTree_SelfAndDescendantsBreadthFirst()
        {
            return _binTree.SelfAndDescendantsBreadthFirst().Count();
        }

        [Benchmark]
        [BenchmarkCategory("TernTree")]
        public int TernTree_SelfAndDescendants()
        {
            return _ternTree.SelfAndDescendants().Count();
        }
        [Benchmark]
        [BenchmarkCategory("TernTree")]
        public int TernTree_DescendantsAndSelf()
        {
            return _ternTree.DescendantsAndSelf().Count();
        }
        [Benchmark]
        [BenchmarkCategory("TernTree")]
        public int TernTree_SelfAndDescendantsBreadthFirst()
        {
            return _ternTree.SelfAndDescendantsBreadthFirst().Count();
        }


        private static BinTree? CreateBinTree(int depth)
        {
            if (depth == 0)
            {
                return null;
            }
            var child = CreateBinTree(depth - 1);
            return new BinTree(child, child);
        }

        private static TernTree? CreateTernTree(int depth)
        {
            if (depth == 0)
            {
                return null;
            }
            var child = CreateTernTree(depth - 1);
            return new TernTree(child, child, child);
        }
    }
}
