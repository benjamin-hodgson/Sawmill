using System;
using System.Collections.Immutable;
using System.Linq;
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
        public void TestSelfAndDescendantsBreadthFirst()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(minusOne, minusTwo);

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, _rewriter.SelfAndDescendantsBreadthFirst(expr));
        }

        [Fact]
        public void TestSelfAndDescendantsBreadthFirstLazy()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(minusOne, minusTwo);

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, _rewriter.SelfAndDescendantsBreadthFirstLazy(expr));
        }

        [Fact]
        public void TestChildrenInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var childrenInContext = _rewriter.ChildrenInContext(expr);

            Assert.Equal(new Expr[] { one, minusTwo }, childrenInContext.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = childrenInContext.First.replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestSelfAndDescendantsInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var contexts = _rewriter.SelfAndDescendantsInContext(expr);

            Assert.Equal(new Expr[] { expr, one, minusTwo, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestSelfAndDescendantsInContextLazy()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var contexts = _rewriter.SelfAndDescendantsInContextLazy(expr);

            Assert.Equal(new Expr[] { expr, one, minusTwo, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestDescendantsAndSelfInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var contexts = _rewriter.DescendantsAndSelfInContext(expr);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(0).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestDescendantsAndSelfInContextLazy()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            var contexts = _rewriter.DescendantsAndSelfInContextLazy(expr);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(0).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestSelfAndDescendantsInContextBreadthFirst()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(minusOne, minusTwo);

            var contexts = _rewriter.SelfAndDescendantsInContextBreadthFirst(expr);

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
        }

        [Fact]
        public void TestSelfAndDescendantsInContextBreadthFirstLazy()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(minusOne, minusTwo);

            var contexts = _rewriter.SelfAndDescendantsInContextBreadthFirstLazy(expr);

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, _rewriter.GetChildren(newExpr));
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
        public void TestDefaultRewriteChildren_ImmutableList_NoOp()
        {
            var t = new Tree<int>(
                1,
                new[]
                {
                    new Tree<int>(2, ImmutableList.Create<Tree<int>>()),
                    new Tree<int>(3, ImmutableList.Create<Tree<int>>())
                }.ToImmutableList()
            );

            var rewritten = t.DefaultRewriteChildren(x => x);

            Assert.Same(t.Children, rewritten.Children);
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