using Xunit;

namespace Sawmill.Tests
{
    public class ChildrenTests
    {
        [Fact]
        public void TestRemoveAll()
        {
            {
                var c = Children.Two(1, 2);
                Assert.Equal(Children.None<int>(), c.RemoveAll(x => true));
            }
            {
                var c = Children.Two(1, 2);
                Assert.Equal(Children.One(2), c.RemoveAll(x => x == 1));
            }
            {
                var c = Children.Two(1, 2);
                Assert.Equal(Children.One(1), c.RemoveAll(x => x == 2));
            }
            {
                var c = Children.Two(1, 2);
                Assert.Equal(c, c.RemoveAll(x => false));
            }
        }
    }
}