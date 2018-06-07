using System;
using System.Collections.Immutable;
using Xunit;

namespace Sawmill.Tests
{
    public class ToTreeStringTests
    {
        [Fact]
        public void ToTreeStringTest()
        {
            var t = new Tree<string>(
                "Foo",
                ImmutableList.Create(
                    new Tree<string>(
                        "Barp",
                        ImmutableList.Create(
                            new Tree<string>("Clonk", ImmutableList<Tree<string>>.Empty),
                            new Tree<string>("Bonk", 
                                ImmutableList.Create(
                                    new Tree<string>("Ploop", ImmutableList<Tree<string>>.Empty)
                                )
                            )
                        )
                    ),
                    new Tree<string>(
                        "Baz",
                        ImmutableList.Create(
                            new Tree<string>("Nabble", ImmutableList<Tree<string>>.Empty),
                            new Tree<string>("Wubble", ImmutableList<Tree<string>>.Empty)
                        )
                    ),
                    new Tree<string>("Quux", ImmutableList<Tree<string>>.Empty)
                )
            );

            var expected = string.Join(
                Environment.NewLine,
                new[]
                {
                    "             Foo              ",
                    "              |               ",
                    "     +------------+--------+  ",
                    "     |            |        |  ",
                    "   Barp          Baz      Quux",
                    "     |            |           ",
                    "  +-----+     +------+        ",
                    "  |     |     |      |        ",
                    "Clonk Bonk  Nabble Wubble     ",
                    "        |                     ",
                    "        +                     ",
                    "        |                     ",
                    "      Ploop                   "
                }
            );

            Assert.Equal(expected, t.ToTreeString());
        }
    }
}