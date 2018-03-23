using System;

namespace Sawmill.Codegen
{
    class Program
    {
        static void Main(string[] args)
        {
            RewritableGenerator.Go();

            XmlDocGenerator.Go();
        }
    }
}
