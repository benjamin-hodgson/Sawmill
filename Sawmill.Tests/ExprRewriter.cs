using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill.Tests
{
    class ExprRewriter : IRewriter<Expr>
    {
        public Children<Expr> GetChildren(Expr value)
        {
            switch (value)
            {
                case Lit l:
                    return Children.None<Expr>();
                case Neg n:
                    return Children.One(n.Operand);
                case Add a:
                    return Children.Two(a.Left, a.Right);
                case Ternary t:
                    return Children.Many(ImmutableList<Expr>.Empty.Add(t.Condition).Add(t.ThenBranch).Add(t.ElseBranch));
                case List l:
                    return Children.Many(l.Exprs);
                case IfThenElse i:
                    return Children.Many(
                        ImmutableList<Expr>
                            .Empty
                            .Add(i.Condition)
                            .AddRange(i.IfTrueStmts)
                            .AddRange(i.IfFalseStmts)
                    );
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public Expr SetChildren(Children<Expr> newChildren, Expr oldValue)
        {
            switch (oldValue)
            {
                case Lit l:
                    return l;
                case Neg n:
                    return new Neg(newChildren.First);
                case Add a:
                    return new Add(newChildren.First, newChildren.Second);
                case Ternary t:
                    return new Ternary(newChildren.ElementAt(0), newChildren.ElementAt(1), newChildren.ElementAt(2));
                case List l:
                    return new List(newChildren.Many);
                case IfThenElse i:
                    return new IfThenElse(
                        newChildren.Many[0],
                        newChildren.Many.Skip(1).Take(i.IfTrueStmts.Count).ToImmutableList(),
                        newChildren.Many.Skip(1 + i.IfTrueStmts.Count).Take(i.IfFalseStmts.Count).ToImmutableList()
                    );
            }
            throw new ArgumentOutOfRangeException(nameof(oldValue));
        }

        public Expr RewriteChildren(Func<Expr, Expr> transformer, Expr oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);
    }
}