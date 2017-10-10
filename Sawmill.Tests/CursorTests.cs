using System;
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

            Assert.Throws<InvalidOperationException>(() => cursor.Up());
            Assert.False(cursor.TryUp());
        }

        [Fact]
        public void LeftFromStart()
        {
            var cursor = _rewriter.Cursor(_expr);

            Assert.Throws<InvalidOperationException>(() => cursor.Left());
            Assert.False(cursor.TryLeft());
        }

        [Fact]
        public void RightFromEnd()
        {
            var cursor = _rewriter.Cursor(_expr);

            Assert.Throws<InvalidOperationException>(() => cursor.Right());
            Assert.False(cursor.TryRight());
        }

        [Fact]
        public void DownFromBottom()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Down();

            Assert.Throws<InvalidOperationException>(() => cursor.Down());
            Assert.False(cursor.TryDown());
        }

        [Fact]
        public void Down()
        {
            var cursor = _rewriter.Cursor(_expr);
            
            cursor.Down();

            var firstChild = cursor.Focus;
            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void DownUp()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Up();

            Assert.Same(_expr, cursor.Focus);

            Assert.False(cursor.TryUp());
            Assert.Same(_expr, cursor.Focus);
        }

        [Fact]
        public void DownRight()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();

            var right = cursor.Focus;
            var lit = Assert.IsType<Lit>(right);
            Assert.Equal(4, lit.Value);

            Assert.False(cursor.TryRight());
            Assert.Same(right, cursor.Focus);
        }

        [Fact]
        public void DownRightUp()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            cursor.Up();
            
            var up = cursor.Focus;

            Assert.Same(_expr, up);
        }

        [Fact]
        public void DownRightUpDown()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            cursor.Up();
            cursor.Down();

            var firstChild = cursor.Focus;
            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void DownRightLeft()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            cursor.Left();
            
            var firstChild = cursor.Focus;
            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void Edit()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr);
            cursor.Focus = replacement;

            Assert.Same(replacement, cursor.Focus);
        }

        [Fact]
        public void DownEditUp()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Focus = replacement;
            cursor.Up();
            
            Assert.NotSame(_expr, cursor.Focus);
            var add = Assert.IsType<Add>(cursor.Focus);
            Assert.Same(add.Right, ((Add)_expr).Right);
            Assert.Same(replacement, add.Left);
        }

        [Fact]
        public void DownRightEditUp()
        {
            var replacement = new Lit(10);

            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            cursor.Focus = replacement;
            cursor.Up();
            
            Assert.NotSame(_expr, cursor.Focus);
            var add = Assert.IsType<Add>(cursor.Focus);
            Assert.Same(add.Left, ((Add)_expr).Left);
            Assert.Same(replacement, add.Right);
        }

        [Fact]
        public void Top()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Down();
            cursor.Top();
            Assert.Same(_expr, cursor.Focus);
        }

        [Fact]
        public void Leftmost()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Leftmost();

            Assert.IsType<Neg>(cursor.Focus);
        }

        [Fact]
        public void Rightmost()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Rightmost();

            Assert.IsType<Lit>(cursor.Focus);
        }
    }
}