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
            var list = new List(ImmutableArray.CreateRange(new Expr[] { a, b, c }));

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
            var ite = new IfThenElse(a, ImmutableArray.CreateRange(new Expr[] { b, c }), ImmutableArray.CreateRange(new Expr[] { d, e }));

            Assert.Equal(new[]{ a, b, c, d ,e }, _rewriter.GetChildren(ite));
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
            var newTernary = _rewriter.SetChildren(Children.Many(new Expr[]{ newC, newT, newF }), ternary);

            Assert.Equal(new[]{ c, t, f }, _rewriter.GetChildren(ternary));
        }

        // [Fact]
        // public void TestSetChildren_EnumerableChildren()
        // {
        //     var a = new Lit(0);
        //     var b = new Lit(1);
        //     var c = new Lit(2);
        //     var list = new List(ImmutableArray.CreateRange(new Expr[] { a, b, c }));

        //     Assert.Equal(new[]{ a, b, c }, _rewriter.GetChildren(list));
        // }

        // [Fact]
        // public void TestSetChildren_EnumerableChildren_Multiple()
        // {
        //     var a = new Lit(0);
        //     var b = new Lit(1);
        //     var c = new Lit(2);
        //     var d = new Lit(3);
        //     var e = new Lit(4);
        //     var ite = new IfThenElse(a, ImmutableArray.CreateRange(new Expr[] { b, c }), ImmutableArray.CreateRange(new Expr[] { d, e }));

        //     Assert.Equal(new[]{ a, b, c, d ,e }, _rewriter.GetChildren(ite));
        // }
    }
}
