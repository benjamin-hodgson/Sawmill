using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Sawmill.Tests
{
    public class RewriterTests
    {
        [Fact]
        public void TestSelfAndDescendants()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(minusTwo, one);

            Assert.Equal(new Expr[] { expr, minusTwo, two, one }, expr.SelfAndDescendants());
        }

        [Fact]
        public void TestDescendantsAndSelf()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, expr.DescendantsAndSelf());
        }

        [Fact]
        public void TestSelfAndDescendantsBreadthFirst()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(minusOne, minusTwo);

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, expr.SelfAndDescendantsBreadthFirst());
        }

        [Fact]
        public void TestChildrenInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            var childrenInContext = expr.ChildrenInContext();

            Assert.Equal(new Expr[] { one, minusTwo }, childrenInContext.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = childrenInContext[0].replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, newExpr.GetChildren());
        }

        [Fact]
        public void TestSelfAndDescendantsInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            var contexts = expr.SelfAndDescendantsInContext();

            Assert.Equal(new Expr[] { expr, one, minusTwo, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, newExpr.GetChildren());
        }

        [Fact]
        public void TestDescendantsAndSelfInContext()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            var contexts = expr.DescendantsAndSelfInContext();

            Assert.Equal(new Expr[] { one, two, minusTwo, expr }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(0).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, newExpr.GetChildren());
        }

        [Fact]
        public void TestSelfAndDescendantsInContextBreadthFirst()
        {
            var one = new Lit(1);
            var minusOne = new Neg(one);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(minusOne, minusTwo);

            var contexts = expr.SelfAndDescendantsInContextBreadthFirst();

            Assert.Equal(new Expr[] { expr, minusOne, minusTwo, one, two }, contexts.Select(x => x.item));
            
            var three = new Lit(3);
            var newExpr = contexts.ElementAt(1).replace(three);
            Assert.Equal(new Expr[] { three, minusTwo }, newExpr.GetChildren());
        }

        [Fact]
        public void TestDescendantAt()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);
            {
                var result = expr.DescendantAt(new[] { Direction.Down, Direction.Right });

                Assert.Same(minusTwo, result);
            }
            {
                var result = expr.DescendantAt(new Direction[] { });

                Assert.Same(expr, result);
            }
            {
                Assert.Throws<InvalidOperationException>(() => expr.DescendantAt(new[] { Direction.Down, Direction.Left }));
                Assert.Throws<InvalidOperationException>(() => expr.DescendantAt(new[] { Direction.Right }));
                Assert.Throws<InvalidOperationException>(() => expr.DescendantAt(new[] { Direction.Up }));
            }
        }

        [Fact]
        public void TestReplaceDescendantAt()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);
            {
                var result = expr.ReplaceDescendantAt(new[] { Direction.Down, Direction.Right }, one);

                Assert.Equal(2, Eval(result));
            }
            {
                var result = expr.ReplaceDescendantAt(new[] { Direction.Down, Direction.Right }, minusTwo);

                Assert.Same(expr, result);
            }
            {
                Assert.Throws<InvalidOperationException>(() => expr.ReplaceDescendantAt(new[] { Direction.Down, Direction.Left }, one));
                Assert.Throws<InvalidOperationException>(() => expr.ReplaceDescendantAt(new[] { Direction.Right }, one));
                Assert.Throws<InvalidOperationException>(() => expr.ReplaceDescendantAt(new[] { Direction.Up }, one));
            }
        }

        [Fact]
        public void TestRewriteDescendantAt()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);
            {
                var result = expr.RewriteDescendantAt(new[] { Direction.Down }, x => x is Lit l ? new Lit(l.Value + 1) : x);

                Assert.Equal(0, Eval(result));
            }
            {
                var result = expr.RewriteDescendantAt(new[] { Direction.Down }, x => x);

                Assert.Same(expr, result);
            }
            {
                Assert.Throws<InvalidOperationException>(() => expr.RewriteDescendantAt(new[] { Direction.Down, Direction.Left }, x => x));
                Assert.Throws<InvalidOperationException>(() => expr.RewriteDescendantAt(new[] { Direction.Right }, x => x));
                Assert.Throws<InvalidOperationException>(() => expr.RewriteDescendantAt(new[] { Direction.Up }, x => x));
            }
        }

        [Fact]
        public void TestFold()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

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
        public void TestRewriteChildren()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            {
                var rewritten = expr.RewriteChildren(_ => new Lit(3));

                Assert.Equal(6, Eval(rewritten));
            }
            {
                var result = expr.RewriteChildren(x => x);

                Assert.Same(expr, result);
            }
        }

        [Fact]
        public void TestRewriteChildren_NoOp()
        {
            var t = new Tree<int>(
                1,
                new[]
                {
                    new Tree<int>(2, ImmutableList.Create<Tree<int>>()),
                    new Tree<int>(3, ImmutableList.Create<Tree<int>>())
                }.ToImmutableList()
            );

            var rewritten = t.RewriteChildren(x => x);

            Assert.Same(t.Children, rewritten.Children);
        }

        [Fact]
        public void TestRewrite()
        {
            var one = new Lit(1);
            var two = new Lit(2);
            var minusTwo = new Neg(two);
            Expr expr = new Add(one, minusTwo);

            {
                var rewritten = expr.Rewrite(x => x is Lit l ? new Lit(l.Value * 2) : x);

                Assert.Equal(-2, Eval(rewritten));
            }
            {
                var result = expr.Rewrite(x => x);

                Assert.Same(expr, result);
            }
        }

        [Fact]
        public void TestRewriteIter()
        {
            // -((1+2)+3) --> ((-1)+(-2))+(-3)
            Expr expr = new Neg(
                new Add(
                    new Add(
                        new Lit(1),
                        new Lit(2)
                    ),
                    new Lit(3)
                )
            );

            {
                var rewritten = expr.RewriteIter(
                    x => x is Neg n && n.Operand is Add a
                        ? new Add(new Neg(a.Left), new Neg(a.Right))
                        : x
                );

                Assert.Equal(-6, Eval(rewritten));
                // find the -1
                Assert.Equal(1, ((Lit)((Neg)((Add)((Add)rewritten).Left).Left).Operand).Value);
            }
            {
                var result = expr.RewriteIter(x => x);

                Assert.Same(expr, result);
            }
        }

        private int Eval(Expr expr)
            => expr.Fold<Expr, int>(
                (next, x) =>
                {
                    switch (x)
                    {
                        case Lit l:
                            return l.Value;
                        case Neg n:
                            return -next[0];
                        case Add a:
                            return next[0] + next[1];
                    }
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
            );
        
        private bool Equal(Expr left, Expr right)
            => left.ZipFold<Expr, bool>(
                right,
                (x, y, children) =>
                {
                    switch (x)
                    {
                        case Lit l1 when y is Lit l2:
                            return l1.Value == l2.Value;
                        case Neg n1 when y is Neg n2:
                            return children.First();
                        case Add a1 when y is Add a2:
                            return children.All(n => n);
                        default:
                            return false;
                    }
                }
            );
    }
}
