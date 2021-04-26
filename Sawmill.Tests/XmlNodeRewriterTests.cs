using System.Xml;
using Xunit;
using Sawmill.Xml;
using System.Linq;
using System.Collections.Immutable;

namespace Sawmill.Tests
{
    public class XmlNodeRewriterTests
    {
        private static readonly string exampleXml = "<foo bar=\"baz\"><quux/><nabble>nobble</nabble></foo>";

        [Fact]
        public void TestGetChildren()
        {
            var doc = new XmlDocument();
            doc.LoadXml(exampleXml);

            var fooNode = doc.GetChildren().Single();
            Assert.Equal("foo", fooNode.Name);

            var children = fooNode.GetChildren();
            
            Assert.Equal(2, children.Count());
            Assert.Equal("quux", children.ElementAt(0).Name);
            Assert.Equal("nabble", children.ElementAt(1).Name);
        }

        [Fact]
        public void TestSetChildren()
        {
            var doc = new XmlDocument();
            doc.LoadXml(exampleXml);

            var fooNode = doc.GetChildren().Single();
            var children = fooNode.GetChildren().ToImmutableList();
            
            var newChildren = children.SetItem(0, doc.CreateElement("ploop"));
            var newFooNode = fooNode.SetChildren(newChildren.ToArray());

            Assert.Equal("foo", newFooNode.Name);
            Assert.Equal(1, newFooNode.Attributes!.Count);
            Assert.Equal("baz", newFooNode.Attributes.GetNamedItem("bar")!.Value);
            Assert.Equal(2, newFooNode.ChildNodes.Count);
            Assert.Equal("ploop", newFooNode.GetChildren().ElementAt(0).Name);
            Assert.Equal("nabble", newFooNode.GetChildren().ElementAt(1).Name);


            // fooNode should not have changed
            Assert.Equal(1, fooNode.Attributes!.Count);
            Assert.Equal(2, fooNode.ChildNodes.Count);
            Assert.Equal("quux", fooNode.GetChildren().ElementAt(0).Name);
            Assert.Equal("nabble", fooNode.GetChildren().ElementAt(1).Name);
        }
    }
}
