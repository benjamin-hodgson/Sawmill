using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill.Tests
{
    abstract class Expr : IRewritable<Expr>
    {
        public abstract Children<Expr> GetChildren();
        public abstract Expr SetChildren(Children<Expr> newChildren);
        public Expr RewriteChildren(Func<Expr, Expr> transformer)
            => this.DefaultRewriteChildren(transformer);
    }
    class Lit : Expr
    {
        public int Value { get; }

        public Lit(int value)
        {
            Value = value;
        }

        public override Children<Expr> GetChildren() => Children.None<Expr>();

        public override Expr SetChildren(Children<Expr> newChildren) => this;
    }
    class Neg : Expr
    {
        public Expr Operand { get; }

        public Neg(Expr operand)
        {
            Operand = operand;
        }

        public override Children<Expr> GetChildren() => (Operand);

        public override Expr SetChildren(Children<Expr> newChildren) => new Neg(newChildren.First);
    }
    class Add : Expr
    {
        public Expr Left { get; }
        public Expr Right { get; }

        public Add(Expr left, Expr right)
        {
            Left = left;
            Right = right;
        }

        public override Children<Expr> GetChildren() => (Left, Right);

        public override Expr SetChildren(Children<Expr> newChildren) => new Add(newChildren.First, newChildren.Second);
    }
    class Ternary : Expr
    {
        public Expr Condition { get; }
        public Expr ThenBranch { get; }
        public Expr ElseBranch { get; }

        public Ternary(Expr condition, Expr thenBranch, Expr elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override Children<Expr> GetChildren()
            => new[] { Condition, ThenBranch, ElseBranch }.ToImmutableList();

        public override Expr SetChildren(Children<Expr> newChildren)
            => new Ternary(newChildren.Many[0], newChildren.Many[1], newChildren.Many[2]);
    }
    class List : Expr
    {
        public ImmutableList<Expr> Exprs { get; }

        public List(ImmutableList<Expr> exprs)
        {
            Exprs = exprs;
        }

        public override Children<Expr> GetChildren() => Exprs;

        public override Expr SetChildren(Children<Expr> newChildren) => new List(newChildren.Many);
    }
    class IfThenElse : Expr
    {
        public Expr Condition { get; }
        public ImmutableList<Expr> IfTrueStmts { get; }
        public ImmutableList<Expr> IfFalseStmts { get; }

        public IfThenElse(Expr condition, ImmutableList<Expr> ifTrueStmts, ImmutableList<Expr> ifFalseStmts)
        {
            Condition = condition;
            IfTrueStmts = ifTrueStmts;
            IfFalseStmts = ifFalseStmts;
        }

        public override Children<Expr> GetChildren()
            => ImmutableList
                .Create(Condition)
                .AddRange(IfTrueStmts)
                .AddRange(IfFalseStmts);

        public override Expr SetChildren(Children<Expr> newChildren)
            => new IfThenElse(
                newChildren.Many[0],
                newChildren.Many.GetRange(1, IfTrueStmts.Count),
                newChildren.Many.GetRange(IfTrueStmts.Count + 1, IfFalseStmts.Count)
            );
    }
}
