using System;
using Xunit;

namespace Sawmill.Tests
{
    public class RewriterTests
    {
        readonly IRewriter<Expr> _rewriter = new ExprRewriter();

        [Fact]
        public void TestSelfAndDescendants()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { expr, one, minusTwo, two }, _rewriter.SelfAndDescendants(expr));
        }

        [Fact]
        public void TestSelfAndDescendantsLazy()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { expr, one, minusTwo, two }, _rewriter.SelfAndDescendantsLazy(expr));
        }

        [Fact]
        public void TestDescendantsAndSelf()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, _rewriter.DescendantsAndSelf(expr));
        }

        [Fact]
        public void TestDescendantsAndSelfLazy()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, _rewriter.DescendantsAndSelfLazy(expr));
        }

        [Fact]
        public void TestFold()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(-1, Eval(expr));
        }

        [Fact]
        public void TestDefaultRewriteChildren()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var rewritten = _rewriter.DefaultRewriteChildren(_ => new Lit(3), expr);

            Assert.Equal(6, Eval(rewritten));
        }

        [Fact]
        public void TestRewrite()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var rewritten = _rewriter.Rewrite(x => x is Lit l ? new Lit(l.Value * 2) : x, expr);

            Assert.Equal(-2, Eval(rewritten));
        }

        [Fact]
        public void TestRewriteIter()
        {
            // -((1+2)+3) --> ((-1)+(-2))+(-3)
            var expr = new Neg(
                new Add(
                    new Add(
                        new Lit(1),
                        new Lit(2)
                    ),
                    new Lit(3)
                )
            );

            var rewritten = _rewriter.RewriteIter(
                x => x is Neg n && n.Operand is Add a
                    ? IterResult.Continue<Expr>(new Add(new Neg(a.Left), new Neg(a.Right)))
                    : IterResult.Done<Expr>(),
                expr
            );

            Assert.Equal(-6, Eval(rewritten));
            // find the -1
            Assert.Equal(1, ((Lit)((Neg)((Add)((Add)rewritten).Left).Left).Operand).Value);
        }

        private int Eval(Expr expr)
            => _rewriter.Fold<Expr, int>(
                (x, next) =>
                {
                    switch (x)
                    {
                        case Lit l:
                            return l.Value;
                        case Neg n:
                            return -next.First;
                        case Add a:
                            return next.First + next.Second;
                    }
                    throw new ArgumentOutOfRangeException(nameof(x));
                },
                expr
            );
    }
}