using System.Collections.Immutable;

using HtmlAgilityPack;

using Sawmill.HtmlAgilityPack;

using Xunit;

namespace Sawmill.Tests;

public class HtmlNodeRewriterTests
{
    private static readonly string _exampleHtml = "<p class=\"baz\"><br/><span>nobble</span></p>";

    [Fact]
    public void TestGetChildren()
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(_exampleHtml);

        var pNode = doc.DocumentNode.GetChildren().Single();
        Assert.Equal("p", pNode.Name);

        var children = pNode.GetChildren();

        Assert.Equal(2, children.Length);
        Assert.Equal("br", children.ElementAt(0).Name);
        Assert.Equal("span", children.ElementAt(1).Name);
    }

    [Fact]
    public void TestSetChildren()
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(_exampleHtml);

        var pNode = doc.DocumentNode.GetChildren().Single();
        var children = pNode.GetChildren().ToImmutableList();

        var newChildren = children.SetItem(0, HtmlNode.CreateNode("<div></div>"));
        var newPNode = pNode.SetChildren(newChildren.ToArray());

        Assert.Equal("p", newPNode.Name);
        Assert.Equal(1, newPNode.Attributes.Count);
        Assert.Equal("baz", newPNode.Attributes["class"].Value);
        Assert.Equal(2, newPNode.GetChildren().Length);
        Assert.Equal("div", newPNode.GetChildren().ElementAt(0).Name);
        Assert.Equal("span", newPNode.GetChildren().ElementAt(1).Name);


        // fooNode should not have changed
        Assert.Equal(1, pNode.Attributes.Count);
        Assert.Equal(2, pNode.GetChildren().Length);
        Assert.Equal("br", pNode.GetChildren().ElementAt(0).Name);
        Assert.Equal("span", pNode.GetChildren().ElementAt(1).Name);
    }
}
