using Xunit;

namespace Sawmill.Tests
{
    public class ZipperTests
    {
        private readonly Expr _expr = new Add(
            new Neg(new Lit(3)),
            new Lit(4)
        );

        private readonly IRewriter<Expr> _rewriter = new ExprRewriter();

        [Fact]
        public void UpFromTop()
        {
            var zipper = _rewriter.Zipper(_expr);

            Assert.Null(zipper.Up());
            Assert.Same(zipper, zipper.TryUp());
        }

        [Fact]
        public void LeftFromStart()
        {
            var zipper = _rewriter.Zipper(_expr);

            Assert.Null(zipper.Left());
            Assert.Same(zipper, zipper.TryLeft());
        }

        [Fact]
        public void RightFromEnd()
        {
            var zipper = _rewriter.Zipper(_expr);

            Assert.Null(zipper.Right());
            Assert.Same(zipper, zipper.TryRight());
        }

        [Fact]
        public void DownFromBottom()
        {
            var zipper = _rewriter.Zipper(_expr).Down().Down();

            Assert.Null(zipper.Down());
            Assert.Same(zipper, zipper.TryDown());
        }

        [Fact]
        public void Down()
        {
            var zipper = _rewriter.Zipper(_expr);
            
            var firstChild = zipper.Down().Focus;

            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);

            Assert.Same(firstChild, zipper.TryDown().Focus);
        }

        [Fact]
        public void DownUp()
        {
            var zipper = _rewriter.Zipper(_expr).Down();
            
            var up = zipper.Up().Focus;

            Assert.Same(_expr, up);
            Assert.Same(_expr, zipper.TryUp().Focus);
        }

        [Fact]
        public void DownRight()
        {
            var zipper = _rewriter.Zipper(_expr).Down();
            
            var right = zipper.Right().Focus;

            var lit = Assert.IsType<Lit>(right);
            Assert.Equal(4, lit.Value);

            Assert.Same(right, zipper.TryRight().Focus);
        }

        [Fact]
        public void DownRightUp()
        {
            var zipper = _rewriter.Zipper(_expr).Down().Right();
            
            var up = zipper.Up().Focus;

            Assert.Same(_expr, up);
        }

        [Fact]
        public void DownRightUpDown()
        {
            var zipper = _rewriter.Zipper(_expr).Down().Right().Up();
            
            var down = zipper.Down().Focus;

            var lit = Assert.IsType<Lit>(down);
            Assert.Equal(4, lit.Value);
        }

        [Fact]
        public void DownRightLeft()
        {
            var zipper = _rewriter.Zipper(_expr).Down().Right();
            
            var left = zipper.Left().Focus;

            var neg = Assert.IsType<Neg>(left);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void Edit()
        {
            var replacement = new Lit(10);

            var zipper = _rewriter.Zipper(_expr).SetFocus(replacement);

            Assert.Same(replacement, zipper.Focus);
        }

        [Fact]
        public void DownEditUp()
        {
            var replacement = new Lit(10);

            var zipper = _rewriter.Zipper(_expr)
                .Down()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, zipper.Focus);
            var add = Assert.IsType<Add>(zipper.Focus);
            Assert.Same(add.Right, ((Add)_expr).Right);
            Assert.Same(replacement, add.Left);
        }

        [Fact]
        public void DownRightEditUp()
        {
            var replacement = new Lit(10);

            var zipper = _rewriter.Zipper(_expr)
                .Down()
                .Right()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, zipper.Focus);
            var add = Assert.IsType<Add>(zipper.Focus);
            Assert.Same(add.Left, ((Add)_expr).Left);
            Assert.Same(replacement, add.Right);
        }

        [Fact]
        public void Top()
        {
            var zipper = _rewriter.Zipper(_expr)
                .Down()
                .Down()
                .Top();
            Assert.Same(_expr, zipper.Focus);
            Assert.Same(zipper, zipper.Top());
        }

        [Fact]
        public void Leftmost()
        {
            var zipper = _rewriter.Zipper(_expr)
                .Down()
                .Leftmost();
            Assert.IsType<Neg>(zipper.Focus);
            Assert.Same(zipper, zipper.Leftmost());
        }

        [Fact]
        public void Rightmost()
        {
            var zipper = _rewriter.Zipper(_expr)
                .Down()
                .Rightmost();
            Assert.IsType<Lit>(zipper.Focus);
            Assert.Same(zipper, zipper.Rightmost());
        }
    }
}