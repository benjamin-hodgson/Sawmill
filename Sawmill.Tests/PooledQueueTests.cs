using Xunit;

namespace Sawmill.Tests
{
    public class PooledQueueTests
    {
        [Fact]
        public void TestAllocateThenPop()
        {
            var queue = new PooledQueue<int>();

            var span = queue.AllocateRight(3);
            span[0] = 0;
            span[1] = 1;
            span[2] = 2;

            Assert.Equal(0, queue.PopLeft());
            Assert.Equal(1, queue.PopLeft());
            Assert.Equal(2, queue.PopLeft());

            queue.Dispose();
        }

        [Fact]
        public void TestAllocateTwiceThenPop()
        {
            var queue = new PooledQueue<int>();

            var span1 = queue.AllocateRight(2);
            span1[0] = 0;
            span1[1] = 1;
            var span2 = queue.AllocateRight(2);
            span2[0] = 2;
            span2[1] = 3;

            Assert.Equal(0, queue.PopLeft());
            Assert.Equal(1, queue.PopLeft());
            Assert.Equal(2, queue.PopLeft());
            Assert.Equal(3, queue.PopLeft());

            queue.Dispose();
        }

        [Fact]
        public void TestAllocateThenPopTwice()
        {
            var queue = new PooledQueue<int>();


            var span1 = queue.AllocateRight(2);
            span1[0] = 0;
            span1[1] = 1;

            Assert.Equal(0, queue.PopLeft());
            Assert.Equal(1, queue.PopLeft());


            var span2 = queue.AllocateRight(2);
            span2[0] = 2;
            span2[1] = 3;

            Assert.Equal(2, queue.PopLeft());
            Assert.Equal(3, queue.PopLeft());


            queue.Dispose();
        }

        [Fact]
        public void TestFillBufferThenRefill()
        {
            var queue = new PooledQueue<int>();

            var span1 = queue.AllocateRight(512);
            span1.Fill(1);
            for (var i = 0; i < 512; i++)
            {
                Assert.Equal(1, queue.PopLeft());
            }

            var span2 = queue.AllocateRight(512);
            span2.Fill(2);
            for (var i = 0; i < 512; i++)
            {
                Assert.Equal(2, queue.PopLeft());
            }

            queue.Dispose();
        }

        [Fact]
        public void TestOverfillBuffer()
        {
            var queue = new PooledQueue<int>();

            var span1 = queue.AllocateRight(513);
            span1.Fill(1);
            for (var i = 0; i < 513; i++)
            {
                Assert.Equal(1, queue.PopLeft());
            }

            queue.Dispose();
        }

        [Fact]
        public void TestFillBufferThenContinueFilling()
        {
            var queue = new PooledQueue<int>();

            var span1 = queue.AllocateRight(512);
            span1.Fill(1);
            var span2 = queue.AllocateRight(1);
            span2.Fill(1);

            for (var i = 0; i < 513; i++)
            {
                Assert.Equal(1, queue.PopLeft());
            }

            queue.Dispose();
        }

        [Fact]
        public void TestFillBufferThenOverfill()
        {
            var queue = new PooledQueue<int>();

            var span1 = queue.AllocateRight(512);
            span1.Fill(1);
            var span2 = queue.AllocateRight(513);
            span2.Fill(1);
            
            for (var i = 0; i < 1025; i++)
            {
                Assert.Equal(1, queue.PopLeft());
            }

            queue.Dispose();
        }
    }
}
