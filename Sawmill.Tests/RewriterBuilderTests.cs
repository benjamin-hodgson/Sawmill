using Xunit;

namespace Sawmill.Tests
{
    public class RewriterBuilderTests
    {
        private static readonly IRewriter<Expr> _rewriter =
            RewriterBuilder<Expr>
                .Case<Lit>(
                    c => c
                        .Field(l => l.Value)
                        .ConstructWith(x => new Lit(x))
                )
                .And<Neg>(
                    c => c
                        .Child(n => n.Operand)
                        .ConstructWith(o => new Neg(o))
                )
                .And<Add>(
                    c => c
                        .Child(a => a.Left)
                        .Child(a => a.Right)
                        .ConstructWith((l, r) => new Add(l, r))
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

            Assert.Equal(new[]{ newNeg, newLit }, _rewriter.SelfAndDescendants(newNeg));
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

            Assert.Equal(new[]{ newAdd, newL, newR }, _rewriter.SelfAndDescendants(newAdd));
        }
    }
}