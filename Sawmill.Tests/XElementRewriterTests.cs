using System.Collections.Immutable;
using System.Xml.Linq;

using Sawmill.Xml;

using Xunit;

namespace Sawmill.Tests;

public class XElementRewriterTests
{
    private const string _exampleXml = "<foo bar=\"baz\"><quux /><nabble>nobble</nabble></foo>";

    [Fact]
    public void TestGetChildren()
    {
        var doc = XDocument.Parse(_exampleXml);

        var fooNode = doc.Elements().Single();
        Assert.Equal("foo", fooNode.Name);

        var children = fooNode.GetChildren();

        Assert.Equal(2, children.Length);
        Assert.Equal("quux", children.ElementAt(0).Name);
        Assert.Equal("nabble", children.ElementAt(1).Name);
    }

    [Fact]
    public void TestSetChildren()
    {
        var doc = XDocument.Parse(_exampleXml);

        var fooNode = doc.Elements().Single();
        var children = fooNode.GetChildren().ToImmutableList();

        var newChildren = children.SetItem(0, new XElement("ploop"));
        var newFooNode = fooNode.SetChildren(newChildren.ToArray());

        Assert.Equal("foo", newFooNode.Name);
        Assert.Single(newFooNode.Attributes());
        Assert.Equal("baz", newFooNode.Attributes("bar").Single().Value);
        Assert.Equal(2, newFooNode.GetChildren().Length);
        Assert.Equal("ploop", newFooNode.GetChildren().ElementAt(0).Name);
        Assert.Equal("nabble", newFooNode.GetChildren().ElementAt(1).Name);

        // fooNode should not have changed
        Assert.Single(fooNode.Attributes());
        Assert.Equal(2, fooNode.GetChildren().Length);
        Assert.Equal("quux", fooNode.GetChildren().ElementAt(0).Name);
        Assert.Equal("nabble", fooNode.GetChildren().ElementAt(1).Name);
    }
}
