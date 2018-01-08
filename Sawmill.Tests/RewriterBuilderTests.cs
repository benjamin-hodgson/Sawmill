using System.Collections.Immutable;
using Xunit;

namespace Sawmill.Tests
{
    public class RewriterBuilderTests
    {
        private static readonly IRewriter<Expr> _rewriter =
            RewriterBuilder.For<Expr>()
                .Case<Lit>(
                    c => c
                        .Field(l => l.Value)
                        .ConstructWith(x => new Lit(x))
                )
                .Case<Neg>(
                    c => c
                        .Child(n => n.Operand)
                        .ConstructWith(o => new Neg(o))
                )
                .Case<Add>(
                    c => c
                        .Child(a => a.Left)
                        .Child(a => a.Right)
                        .ConstructWith((l, r) => new Add(l, r))
                )
                .Case<Ternary>(
                    c => c
                        .Child(t => t.Condition)
                        .Child(t => t.ThenBranch)
                        .Child(t => t.ElseBranch)
                        .ConstructWith((cond, tru, fls) => new Ternary(cond, tru, fls))
                )
                .Case<List>(
                    c => c
                        .Children(l => l.Exprs)
                        .ConstructWith(es => new List(es))
                )
                .Build();

        [Fact]
        public void TestGetChildren_NoChildren()
        {
            var lit = new Lit(3);

            Assert.Empty(_rewriter.GetChildren(lit));
        }

        [Fact]
        public void TestGetChildren_OneChild()
        {
            var lit = new Lit(3);
            var neg = new Neg(lit);

            Assert.Single(_rewriter.GetChildren(neg), lit);
        }

        [Fact]
        public void TestGetChildren_TwoChildren()
        {
            var l = new Lit(1);
            var r = new Lit(2);
            var add = new Add(l, r);

            Assert.Equal(new[]{ l, r }, _rewriter.GetChildren(add));
        }

        [Fact]
        public void TestGetChildren_ThreeChildren()
        {
            var c = new Lit(0);
            var l = new Lit(1);
            var r = new Lit(2);
            var tern = new Ternary(c, l, r);

            Assert.Equal(new[]{ c, l, r }, _rewriter.GetChildren(tern));
        }

        [Fact]
        public void TestGetChildren_ManyChildren()
        {
            var zero = new Lit(0);
            var one = new Lit(1);
            var two = new Lit(2);
            var list = new List(ImmutableList<Expr>.Empty.Add(zero).Add(one).Add(two));

            Assert.Equal(new[]{ zero, one, two }, _rewriter.GetChildren(list));
        }
        
        [Fact]
        public void TestSetChildren_NoChildren()
        {
            var lit = new Lit(3);

            Assert.Same(lit, _rewriter.SetChildren(Children.None<Expr>(), lit));
        }

        [Fact]
        public void TestSetChildren_OneChild()
        {
            var lit = new Lit(3);
            var neg = new Neg(lit);

            var newLit = new Lit(4);
            var newNeg = _rewriter.SetChildren(Children.One<Expr>(newLit), neg);

            Assert.Equal(new[]{ newLit }, _rewriter.GetChildren(newNeg));
        }

        [Fact]
        public void TestSetChildren_TwoChildren()
        {
            var l = new Lit(1);
            var r = new Lit(2);
            var add = new Add(l, r);

            var newL = new Lit(3);
            var newR = new Lit(4);
            var newAdd = _rewriter.SetChildren(Children.Two<Expr>(newL, newR), add);

            Assert.Equal(new[]{ newL, newR }, _rewriter.GetChildren(newAdd));
        }

        [Fact]
        public void TestSetChildren_ThreeChildren()
        {
            var c = new Lit(0);
            var l = new Lit(1);
            var r = new Lit(2);
            var tern = new Ternary(c, l, r);

            var newC = new Lit(3);
            var newL = new Lit(4);
            var newR = new Lit(5);
            var newTern = _rewriter.SetChildren(Children.Many(ImmutableList<Expr>.Empty.Add(newC).Add(newL).Add(newR)), tern);

            Assert.Equal(new[]{ newC, newL, newR }, _rewriter.GetChildren(newTern));
        }

        [Fact]
        public void TestSetChildren_ManyChildren()
        {
            var zero = new Lit(0);
            var one = new Lit(1);
            var two = new Lit(2);
            var list = new List(ImmutableList<Expr>.Empty.Add(zero).Add(one).Add(two));

            var new0 = new Lit(3);
            var new1 = new Lit(4);
            var new2 = new Lit(5);
            var newList = _rewriter.SetChildren(Children.Many(ImmutableList<Expr>.Empty.Add(new0).Add(new1).Add(new2)), list);

            Assert.Equal(new[]{ new0, new1, new2 }, _rewriter.GetChildren(newList));
        }
    }
}