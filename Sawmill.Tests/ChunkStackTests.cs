using Xunit;

namespace Sawmill.Tests;

public class ChunkStackTests
{
    [Fact]
    public void TestEmptyChunk()
    {
        var stack = new ChunkStack<int>();
        var span = stack.Allocate(0);
        Assert.Equal(0, span.Length);

        stack.Free(span);
        stack.Dispose();
    }

    [Fact]
    public void TestSmallChunk()
    {
        var stack = new ChunkStack<int>();
        var span = stack.Allocate(1);
        Assert.Equal(1, span.Length);

        stack.Free(span);
        stack.Dispose();
    }

    [Fact]
    public void TestSomeSmallChunks()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(1);
        Assert.Equal(1, span1.Length);

        var span2 = stack.Allocate(2);
        Assert.Equal(2, span2.Length);

        var previousValues = span2.ToArray();
        span1[0]++;
        Assert.Equal(previousValues, span2.ToArray());

        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }

    [Fact]
    public void TestManySmallChunks()
    {
        var stack = new ChunkStack<int>();

        for (var i = 0; i < 1025; i++)
        {
            var span = stack.Allocate(1);
            Assert.Equal(1, span.Length);
        }

        stack.Dispose();
    }

    [Fact]
    public void TestEdgeOfRegion1_Fits()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(510);
        Assert.Equal(510, span1.Length);

        var span2 = stack.Allocate(2);
        Assert.Equal(2, span2.Length);

        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }

    [Fact]
    public void TestEdgeOfRegion1_DoesntFit()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(510);
        Assert.Equal(510, span1.Length);

        var span2 = stack.Allocate(3);
        Assert.Equal(3, span2.Length);

        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }

    [Fact]
    public void TestEdgeOfRegion2_Fits()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(512);
        Assert.Equal(512, span1.Length);

        var span2 = stack.Allocate(510);
        Assert.Equal(510, span2.Length);

        var span3 = stack.Allocate(2);
        Assert.Equal(2, span3.Length);

        stack.Free(span3);
        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }

    [Fact]
    public void TestEdgeOfRegion2_DoesntFit()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(512);
        Assert.Equal(512, span1.Length);

        var span2 = stack.Allocate(510);
        Assert.Equal(510, span2.Length);

        var span3 = stack.Allocate(3);
        Assert.Equal(3, span3.Length);

        stack.Free(span3);
        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }

    [Fact]
    public void TestLargeChunk_Region1()
    {
        var stack = new ChunkStack<int>();

        var span = stack.Allocate(513);
        Assert.Equal(513, span.Length);

        stack.Free(span);
        stack.Dispose();
    }

    [Fact]
    public void TestLargeChunk_Region2()
    {
        var stack = new ChunkStack<int>();

        var span1 = stack.Allocate(512);
        Assert.Equal(512, span1.Length);

        var span2 = stack.Allocate(513);
        Assert.Equal(513, span2.Length);

        stack.Free(span2);
        stack.Free(span1);
        stack.Dispose();
    }
}
