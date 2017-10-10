using Xunit;

namespace Sawmill.Tests
{
    public class CursorTests
    {
        private readonly Expr _expr = new Add(
            new Neg(new Lit(3)),
            new Lit(4)
        );

        private readonly IRewriter<Expr> _rewriter = new ExprRewriter();

        [Fact]
        public void UpFromTop()
        {
            var cursor = _rewriter.Cursor(_expr);

            Assert.Null(cursor.Up());
            Assert.Same(cursor, cursor.TryUp());
        }

        [Fact]
        public void LeftFromStart()
        {
            var cursor = _rewriter.Cursor(_expr);

            Assert.Null(cursor.Left());
            Assert.Same(cursor, cursor.TryLeft());
        }

        [Fact]
        public void RightFromEnd()
        {
            var cursor = _rewriter.Cursor(_expr);

            Assert.Null(cursor.Right());
            Assert.Same(cursor, cursor.TryRight());
        }

        [Fact]
        public void DownFromBottom()
        {
            var cursor = _rewriter.Cursor(_expr).Down().Down();

            Assert.Null(cursor.Down());
            Assert.Same(cursor, cursor.TryDown());
        }

        [Fact]
        public void Down()
        {
            var cursor = _rewriter.Cursor(_expr);
            
            var firstChild = cursor.Down().Focus;

            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);

            Assert.Same(firstChild, cursor.TryDown().Focus);
        }

        [Fact]
        public void DownUp()
        {
            var cursor = _rewriter.Cursor(_expr).Down();
            
            var up = cursor.Up().Focus;

            Assert.Same(_expr, up);
            Assert.Same(_expr, cursor.TryUp().Focus);
        }

        [Fact]
        public void DownRight()
        {
            var cursor = _rewriter.Cursor(_expr).Down();
            
            var right = cursor.Right().Focus;

            var lit = Assert.IsType<Lit>(right);
            Assert.Equal(4, lit.Value);

            Assert.Same(right, cursor.TryRight().Focus);
        }

        [Fact]
        public void DownRightUp()
        {
            var cursor = _rewriter.Cursor(_expr).Down().Right();
            
            var up = cursor.Up().Focus;

            Assert.Same(_expr, up);
        }

        [Fact]
        public void DownRightUpDown()
        {
            var cursor = _rewriter.Cursor(_expr).Down().Right().Up();
            
            var down = cursor.Down().Focus;

            var lit = Assert.IsType<Lit>(down);
            Assert.Equal(4, lit.Value);
        }

        [Fact]
        public void DownRightLeft()
        {
            var cursor = _rewriter.Cursor(_expr).Down().Right();
            
            var left = cursor.Left().Focus;

            var neg = Assert.IsType<Neg>(left);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void Edit()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr).SetFocus(replacement);

            Assert.Same(replacement, cursor.Focus);
        }

        [Fact]
        public void DownEditUp()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr)
                .Down()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, cursor.Focus);
            var add = Assert.IsType<Add>(cursor.Focus);
            Assert.Same(add.Right, ((Add)_expr).Right);
            Assert.Same(replacement, add.Left);
        }

        [Fact]
        public void DownRightEditUp()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr)
                .Down()
                .Right()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, cursor.Focus);
            var add = Assert.IsType<Add>(cursor.Focus);
            Assert.Same(add.Left, ((Add)_expr).Left);
            Assert.Same(replacement, add.Right);
        }

        [Fact]
        public void Top()
        {
            var cursor = _rewriter.Cursor(_expr)
                .Down()
                .Down()
                .Top();
            Assert.Same(_expr, cursor.Focus);
            Assert.Same(cursor, cursor.Top());
        }

        [Fact]
        public void Leftmost()
        {
            var cursor = _rewriter.Cursor(_expr)
                .Down()
                .Leftmost();
            Assert.IsType<Neg>(cursor.Focus);
            Assert.Same(cursor, cursor.Leftmost());
        }

        [Fact]
        public void Rightmost()
        {
            var cursor = _rewriter.Cursor(_expr)
                .Down()
                .Rightmost();
            Assert.IsType<Lit>(cursor.Focus);
            Assert.Same(cursor, cursor.Rightmost());
        }
    }
}