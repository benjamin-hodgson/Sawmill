using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Sawmill.Bench
{
    [MemoryDiagnoser]
    public class RewriteChildrenBench
    {
        private readonly RoseTree _tree = new RoseTree(
            Enumerable
                .Repeat(new RoseTree(ImmutableArray.Create<RoseTree>()), 1000000)
                .ToImmutableArray()
        );
        private readonly RoseTree _replacement = new RoseTree(ImmutableArray.Create<RoseTree>());

        [Benchmark]
        public RoseTree RewriteChildren_Rose_Id()
        {
            return _tree.RewriteChildren(x => x);
        }

        [Benchmark]
        public RoseTree RewriteChildren_Rose_Replace()
        {
            return _tree.RewriteChildren(x => _replacement);
        }
    }
}
