using System;
using System.Collections.Generic;
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
        public void TestDescendantsAndSelf()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, _rewriter.DescendantsAndSelf(expr));
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
        public void TestFold()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            Assert.Equal(-1, Eval(expr));
        }

        [Fact]
        public void TestZipFold()
        {
            {
                var one = new Lit(1);
                var two = new Lit(2);
                var minusTwo = new Neg(two);
                var expr = new Add(one, minusTwo);

                Assert.True(Equal(expr, expr));
            }
            {
                var one = new Lit(1);
                var two = new Lit(2);
                var minusTwo = new Neg(two);
                var left = new Add(one, minusTwo);
                var right = new Add(two, minusTwo);

                Assert.False(Equal(left, right));
            }
        }

        [Fact]
        public void TestDefaultRewriteChildren()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            var expr = new Add(one, minusTwo);

            {
                var rewritten = _rewriter.DefaultRewriteChildren(_ => new Lit(3), expr);

                Assert.Equal(6, Eval(rewritten));
            }
            {
                var result = _rewriter.DefaultRewriteChildren(x => x, expr);

                Assert.Same(expr, result);
            }
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

            {
                var rewritten = _rewriter.Rewrite(x => x is Lit l ? new Lit(l.Value * 2) : x, expr);

                Assert.Equal(-2, Eval(rewritten));
            }
            {
                var result = _rewriter.Rewrite(x => x, expr);

                Assert.Same(expr, result);
            }
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

            {
                var rewritten = _rewriter.RewriteIter(
                    x => x is Neg n && n.Operand is Add a
                        ? new Add(new Neg(a.Left), new Neg(a.Right))
                        : x,
                    expr
                );

                Assert.Equal(-6, Eval(rewritten));
                // find the -1
                Assert.Equal(1, ((Lit)((Neg)((Add)((Add)rewritten).Left).Left).Operand).Value);
            }
            {
                var result = _rewriter.RewriteIter(x => x, expr);

                Assert.Same(expr, result);
            }
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
        
        private bool Equal(Expr left, Expr right)
            => _rewriter.ZipFold<Expr, bool>(
                (xs, children) =>
                {
                    switch (xs[0])
                    {
                        case Lit l1 when xs[1] is Lit l2:
                            return l1.Value == l2.Value;
                        case Neg n1 when xs[1] is Neg n2:
                            return children.First();
                        case Add a1 when xs[1] is Add a2:
                            return children.All(x => x);
                        default:
                            return false;
                    }
                },
                left,
                right
            );
    }
}