using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;

namespace Sawmill.Bench;

[MemoryDiagnoser]
public class RewriteChildrenBench
{
    private readonly RoseTree _tree = new(
        Enumerable
            .Repeat(new RoseTree(ImmutableArray.Create<RoseTree>()), 1000000)
            .ToImmutableArray()
    );

    private readonly RoseTree _replacement = new(ImmutableArray.Create<RoseTree>());

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
