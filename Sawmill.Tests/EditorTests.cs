using Xunit;

namespace Sawmill.Tests
{
    public class EditorTests
    {
        private readonly Expr _expr = new Add(
            new Neg(new Lit(3)),
            new Lit(4)
        );

        private readonly IRewriter<Expr> _rewriter = new ExprRewriter();

        [Fact]
        public void UpFromTop()
        {
            var editor = _rewriter.Editor(_expr);

            Assert.Null(editor.Up());
            Assert.Same(editor, editor.TryUp());
        }

        [Fact]
        public void LeftFromStart()
        {
            var editor = _rewriter.Editor(_expr);

            Assert.Null(editor.Left());
            Assert.Same(editor, editor.TryLeft());
        }

        [Fact]
        public void RightFromEnd()
        {
            var editor = _rewriter.Editor(_expr);

            Assert.Null(editor.Right());
            Assert.Same(editor, editor.TryRight());
        }

        [Fact]
        public void DownFromBottom()
        {
            var editor = _rewriter.Editor(_expr).Down().Down();

            Assert.Null(editor.Down());
            Assert.Same(editor, editor.TryDown());
        }

        [Fact]
        public void Down()
        {
            var editor = _rewriter.Editor(_expr);
            
            var firstChild = editor.Down().Focus;

            var neg = Assert.IsType<Neg>(firstChild);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);

            Assert.Same(firstChild, editor.TryDown().Focus);
        }

        [Fact]
        public void DownUp()
        {
            var editor = _rewriter.Editor(_expr).Down();
            
            var up = editor.Up().Focus;

            Assert.Same(_expr, up);
            Assert.Same(_expr, editor.TryUp().Focus);
        }

        [Fact]
        public void DownRight()
        {
            var editor = _rewriter.Editor(_expr).Down();
            
            var right = editor.Right().Focus;

            var lit = Assert.IsType<Lit>(right);
            Assert.Equal(4, lit.Value);

            Assert.Same(right, editor.TryRight().Focus);
        }

        [Fact]
        public void DownRightUp()
        {
            var editor = _rewriter.Editor(_expr).Down().Right();
            
            var up = editor.Up().Focus;

            Assert.Same(_expr, up);
        }

        [Fact]
        public void DownRightUpDown()
        {
            var editor = _rewriter.Editor(_expr).Down().Right().Up();
            
            var down = editor.Down().Focus;

            var lit = Assert.IsType<Lit>(down);
            Assert.Equal(4, lit.Value);
        }

        [Fact]
        public void DownRightLeft()
        {
            var editor = _rewriter.Editor(_expr).Down().Right();
            
            var left = editor.Left().Focus;

            var neg = Assert.IsType<Neg>(left);
            var lit = Assert.IsType<Lit>(neg.Operand);
            Assert.Equal(3, lit.Value);
        }

        [Fact]
        public void Edit()
        {
            var replacement = new Lit(10);

            var editor = _rewriter.Editor(_expr).SetFocus(replacement);

            Assert.Same(replacement, editor.Focus);
        }

        [Fact]
        public void DownEditUp()
        {
            var replacement = new Lit(10);

            var editor = _rewriter.Editor(_expr)
                .Down()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, editor.Focus);
            var add = Assert.IsType<Add>(editor.Focus);
            Assert.Same(add.Right, ((Add)_expr).Right);
            Assert.Same(replacement, add.Left);
        }

        [Fact]
        public void DownRightEditUp()
        {
            var replacement = new Lit(10);

            var editor = _rewriter.Editor(_expr)
                .Down()
                .Right()
                .SetFocus(replacement)
                .Up();
            
            Assert.NotSame(_expr, editor.Focus);
            var add = Assert.IsType<Add>(editor.Focus);
            Assert.Same(add.Left, ((Add)_expr).Left);
            Assert.Same(replacement, add.Right);
        }

        [Fact]
        public void Top()
        {
            var editor = _rewriter.Editor(_expr)
                .Down()
                .Down()
                .Top();
            Assert.Same(_expr, editor.Focus);
            Assert.Same(editor, editor.Top());
        }

        [Fact]
        public void Leftmost()
        {
            var editor = _rewriter.Editor(_expr)
                .Down()
                .Leftmost();
            Assert.IsType<Neg>(editor.Focus);
            Assert.Same(editor, editor.Leftmost());
        }

        [Fact]
        public void Rightmost()
        {
            var editor = _rewriter.Editor(_expr)
                .Down()
                .Rightmost();
            Assert.IsType<Lit>(editor.Focus);
            Assert.Same(editor, editor.Rightmost());
        }
    }
}