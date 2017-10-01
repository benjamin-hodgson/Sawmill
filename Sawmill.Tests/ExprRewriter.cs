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
                    return Children.Many(new[]{ t.Condition, t.ThenBranch, t.ElseBranch });
                case List l:
                    return Children.Many(l.Exprs);
                case IfThenElse i:
                    var children = new List<Expr> { i.Condition };
                    children.AddRange(i.IfTrueStmts);
                    children.AddRange(i.IfFalseStmts);
                    return Children.Many(children);
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
                    return new List(newChildren.ToImmutableArray());
                case IfThenElse i:
                    return new IfThenElse(
                        newChildren.ElementAt(0),
                        newChildren.Skip(1).Take(i.IfTrueStmts.Length).ToImmutableArray(),
                        newChildren.Skip(1 + i.IfTrueStmts.Length).Take(i.IfFalseStmts.Length).ToImmutableArray()
                    );
            }
            throw new ArgumentOutOfRangeException(nameof(oldValue));
        }

        public Expr RewriteChildren(Func<Expr, Expr> transformer, Expr oldValue)
        {
            switch (oldValue)
            {
                case Lit l:
                    return l;
                case Neg n:
                    return new Neg(transformer(n.Operand));
                case Add a:
                    return new Add(transformer(a.Left), transformer(a.Right));
                case Ternary t:
                    return new Ternary(transformer(t.Condition), transformer(t.ThenBranch), transformer(t.ElseBranch));
                case List l:
                    return new List(l.Exprs.Select(transformer).ToImmutableArray());
                case IfThenElse i:
                    return new IfThenElse(
                        transformer(i.Condition),
                        i.IfTrueStmts.Select(transformer).ToImmutableArray(),
                        i.IfFalseStmts.Select(transformer).ToImmutableArray()
                    );
            }
            throw new ArgumentOutOfRangeException(nameof(oldValue));
        }
    }
}