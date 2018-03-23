using Xunit;
using Sawmill.Xml;
using System.Linq;
using System.Collections.Immutable;
using System.IO;
using System.Xml.Linq;

namespace Sawmill.Tests
{
    public class XElementRewriterTests
    {
        private static readonly string exampleXml = "<foo bar=\"baz\"><quux/><nabble>nobble</nabble></foo>";

        [Fact]
        public void TestGetChildren()
        {
            var doc = XDocument.Parse(exampleXml);

            var fooNode = doc.Elements().Single();
            Assert.Equal("foo", fooNode.Name);

            var children = fooNode.GetChildren().Many;
            
            Assert.Equal(2, children.Count());
            Assert.Equal("quux", children.ElementAt(0).Name);
            Assert.Equal("nabble", children.ElementAt(1).Name);
        }

        [Fact]
        public void TestSetChildren()
        {
            var doc = XDocument.Parse(exampleXml);

            var fooNode = doc.Elements().Single();
            var children = fooNode.GetChildren().Many.ToImmutableList();
            
            var newChildren = children.SetItem(0, new XElement("ploop"));
            var newFooNode = fooNode.SetChildren(Children.Many(newChildren));

            Assert.Equal("foo", newFooNode.Name);
            Assert.Equal(1, newFooNode.Attributes().Count());
            Assert.Equal("baz", newFooNode.Attributes("bar").Single().Value);
            Assert.Equal(2, newFooNode.GetChildren().Count());
            Assert.Equal("ploop", newFooNode.GetChildren().ElementAt(0).Name);
            Assert.Equal("nabble", newFooNode.GetChildren().ElementAt(1).Name);


            // fooNode should not have changed
            Assert.Equal(1, fooNode.Attributes().Count());
            Assert.Equal(2, fooNode.GetChildren().Count());
            Assert.Equal("quux", fooNode.GetChildren().ElementAt(0).Name);
            Assert.Equal("nabble", fooNode.GetChildren().ElementAt(1).Name);
        }
    }
}
