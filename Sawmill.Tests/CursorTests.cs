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
        public void Move_DownRightLeft()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Move(Direction.Down);
            cursor.Move(Direction.Right);
            cursor.Move(Direction.Left);
            
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

        [Fact]
        public void UpN()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Down();

                cursor.Up(2);

                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Down();

                var success = cursor.TryUp(2);

                Assert.True(success);
                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<InvalidOperationException>(() => cursor.Up(5));

                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.False(cursor.TryUp(5));

                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.Up(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.TryUp(-1));
            }
        }

        [Fact]
        public void DownN()
        {
            {
                var cursor = _rewriter.Cursor(_expr);

                cursor.Down(2);

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                var success = cursor.TryDown(2);

                Assert.True(success);
                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<InvalidOperationException>(() => cursor.Down(5));

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.False(cursor.TryDown(5));

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.Down(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.TryDown(-1));
            }
        }

        [Fact]
        public void RightN()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                cursor.Right(1);

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(4, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                var success = cursor.TryRight(1);

                Assert.True(success);
                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(4, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                Assert.Throws<InvalidOperationException>(() => cursor.Right(5));

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(4, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                Assert.False(cursor.TryRight(5));

                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(4, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.Right(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.TryRight(-1));
            }
        }

        [Fact]
        public void LeftN()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                cursor.Left(1);

                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                var success = cursor.TryLeft(1);

                Assert.True(success);
                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                Assert.Throws<InvalidOperationException>(() => cursor.Left(5));

                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                Assert.False(cursor.TryLeft(5));

                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.Left(-1));
                Assert.Throws<ArgumentOutOfRangeException>(() => cursor.TryLeft(-1));
            }
        }

        [Fact]
        public void UpWhile()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                cursor.UpWhile(e => e is Neg);

                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                var success = cursor.TryUpWhile(e => e is Neg);

                Assert.True(success);
                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                Assert.Throws<InvalidOperationException>(() => cursor.UpWhile(e => true));
                Assert.Same(_expr, cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                var success = cursor.TryUpWhile(e => true);

                Assert.False(success);
                Assert.Same(_expr, cursor.Focus);
            }
        }

        [Fact]
        public void DownWhile()
        {
            {
                var cursor = _rewriter.Cursor(_expr);

                cursor.DownWhile(e => e is Add);

                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                var success = cursor.TryDownWhile(e => e is Add);

                Assert.True(success);
                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                Assert.Throws<InvalidOperationException>(() => cursor.DownWhile(e => true));
                Assert.IsType<Lit>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);

                var success = cursor.TryDownWhile(e => true);

                Assert.False(success);
                Assert.IsType<Lit>(cursor.Focus);
            }
        }

        [Fact]
        public void RightWhile()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                cursor.RightWhile(e => e is Neg);

                Assert.IsType<Lit>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                var success = cursor.TryRightWhile(e => e is Neg);

                Assert.True(success);
                Assert.IsType<Lit>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                Assert.Throws<InvalidOperationException>(() => cursor.RightWhile(e => true));
                Assert.IsType<Lit>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();

                var success = cursor.TryRightWhile(e => true);

                Assert.False(success);
                Assert.IsType<Lit>(cursor.Focus);
            }
        }

        [Fact]
        public void LeftWhile()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                cursor.LeftWhile(e => e is Lit);

                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                var success = cursor.TryLeftWhile(e => e is Lit);

                Assert.True(success);
                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                Assert.Throws<InvalidOperationException>(() => cursor.LeftWhile(e => true));
                Assert.IsType<Neg>(cursor.Focus);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                cursor.Down();
                cursor.Right();

                var success = cursor.TryLeftWhile(e => true);

                Assert.False(success);
                Assert.IsType<Neg>(cursor.Focus);
            }
        }

        [Fact]
        public void GetPath()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            
            Assert.Equal(new[]{ Direction.Down, Direction.Right }, System.Linq.Enumerable.ToArray(cursor.GetPath()));
        }

        [Fact]
        public void Follow_DownRightLeft()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Follow(new[] { Direction.Down, Direction.Right, Direction.Left });
            
            var firstChild = cursor.Focus;
            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void ReleaseOldVersions()
        {
            var cursor = _rewriter.Cursor(_expr);
            cursor.Down();
            cursor.Right();
            cursor.Left();

            cursor.ReleaseOldVersions();

            var firstChild = cursor.Focus;
            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void SearchDownAndRight()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                var success = cursor.SearchDownAndRight(n => n is Lit);

                Assert.True(success);
                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                var success = cursor.SearchDownAndRight(n => false);

                Assert.False(success);
                Assert.IsType<Add>(cursor.Focus);
            }
        }

        [Fact]
        public void SearchRightAndDown()
        {
            {
                var cursor = _rewriter.Cursor(_expr);
                var success = cursor.SearchRightAndDown(n => n is Lit);

                Assert.True(success);
                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(4, lit.Value);
            }
            {
                var cursor = _rewriter.Cursor(_expr);
                var success = cursor.SearchRightAndDown(n => false);

                Assert.False(success);
                var lit = Assert.IsType<Lit>(cursor.Focus);
                Assert.Equal(3, lit.Value);
            }
        }
    }
}