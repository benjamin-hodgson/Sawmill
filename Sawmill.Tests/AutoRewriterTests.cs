using System;
using System.Collections.Immutable;
using Xunit;

namespace Sawmill.Tests
{
    public class AutoRewriterTests
    {
        private static readonly IRewriter<Expr> _rewriter = AutoRewriter<Expr>.Instance;

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
            var t = new Lit(1);
            var f = new Lit(2);
            var ternary = new Ternary(c, t, f);

            Assert.Equal(new[]{ c, t, f }, _rewriter.GetChildren(ternary));
        }

        [Fact]
        public void TestGetChildren_EnumerableChildren()
        {
            var a = new Lit(0);
            var b = new Lit(1);
            var c = new Lit(2);
            var list = new List(ImmutableList<Expr>.Empty.Add(a).Add(b).Add(c));

            Assert.Equal(new[]{ a, b, c }, _rewriter.GetChildren(list));
        }

        [Fact]
        public void TestGetChildren_EnumerableChildren_Multiple()
        {
            var a = new Lit(0);
            var b = new Lit(1);
            var c = new Lit(2);
            var d = new Lit(3);
            var e = new Lit(4);
            var ite = new IfThenElse(a, ImmutableList<Expr>.Empty.Add(b).Add(c), ImmutableList<Expr>.Empty.Add(d).Add(e));

            Assert.Equal(new[]{ a, b, c, d ,e }, _rewriter.GetChildren(ite));
        }

        [Fact]
        public void TestSetChildren_NoChildren()
        {
            var lit = new Lit(3);

            Assert.Equal(lit, _rewriter.SetChildren(new Expr[]{}, lit));
        }

        [Fact]
        public void TestSetChildren_OneChild()
        {
            var lit = new Lit(3);
            var neg = new Neg(lit);

            var newLit = new Lit(4);
            var newNeg = _rewriter.SetChildren(new[] { newLit }, neg);

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
            var newAdd = _rewriter.SetChildren(new[] { newL, newR }, add);

            Assert.Equal(new[]{ newL, newR }, _rewriter.GetChildren(newAdd));
        }

        [Fact]
        public void TestSetChildren_ThreeChildren()
        {
            var c = new Lit(0);
            var t = new Lit(1);
            var f = new Lit(2);
            var ternary = new Ternary(c, t, f);

            var newC = new Lit(3);
            var newT = new Lit(4);
            var newF = new Lit(5);
            var newTernary = _rewriter.SetChildren(new[] { newC, newT, newF }, ternary);

            Assert.Equal(new[]{ newC, newT, newF }, _rewriter.GetChildren(newTernary));
        }

        [Fact]
        public void TestSetChildren_EnumerableChildren()
        {
            var a = new Lit(0);
            var b = new Lit(1);
            var c = new Lit(2);
            var list = new List(ImmutableList<Expr>.Empty.Add(a).Add(b).Add(c));

            var newA = new Lit(3);
            var newB = new Lit(4);
            var newC = new Lit(5);
            var newList = _rewriter.SetChildren(new[] { newA, newB, newC }, list);

            Assert.Equal(new[]{ newA, newB, newC }, _rewriter.GetChildren(newList));
        }

        [Fact]
        public void TestSetChildren_EnumerableChildren_Multiple()
        {
            var a = new Lit(0);
            var b = new Lit(1);
            var c = new Lit(2);
            var d = new Lit(3);
            var e = new Lit(4);
            var ite = new IfThenElse(a, ImmutableList<Expr>.Empty.Add(b).Add(c), ImmutableList<Expr>.Empty.Add(d).Add(e));

            var newA = new Lit(5);
            var newB = new Lit(6);
            var newC = new Lit(7);
            var newD = new Lit(8);
            var newE = new Lit(9);
            var newIte = _rewriter.SetChildren(new[] { newA, newB, newC, newD, newE }, ite);

            Assert.Equal(new[]{ newA, newB, newC, newD, newE }, _rewriter.GetChildren(newIte));
        }
    }
}
