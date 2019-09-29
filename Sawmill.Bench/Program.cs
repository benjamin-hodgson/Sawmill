using BenchmarkDotNet.Running;

namespace Sawmill.Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll();
        }
    }
}
