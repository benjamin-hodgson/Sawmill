using System;
using BenchmarkDotNet.Running;

namespace Sawmill.Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DescendantsBench>();
            BenchmarkRunner.Run<DefaultRewriteChildrenBench>();
        }
    }
}
