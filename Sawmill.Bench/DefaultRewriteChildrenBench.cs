using System;
using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Sawmill.Bench
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class DefaultRewriteChildrenBench
    {
        private readonly RoseTree _tree = new RoseTree(
            Enumerable
                .Repeat(new RoseTree(ImmutableList.Create<RoseTree>()), 1000000)
                .ToImmutableList()
        );
        private readonly RoseTree _replacement = new RoseTree(ImmutableList.Create<RoseTree>());

        [Benchmark]
        public RoseTree RewriteChildren_Rose_Id()
        {
            return _tree.DefaultRewriteChildren(x => x);
        }

        [Benchmark]
        public RoseTree RewriteChildren_Rose_Replace()
        {
            return _tree.DefaultRewriteChildren(x => _replacement);
        }
    }
}
