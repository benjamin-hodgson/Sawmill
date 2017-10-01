using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill.Tests
{
    abstract class Expr { }
    class Lit : Expr
    {
        public int Value { get; }

        public Lit(int value)
        {
            Value = value;
        }
    }
    class Neg : Expr
    {
        public Expr Operand { get; }

        public Neg(Expr operand)
        {
            Operand = operand;
        }
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
    }
    class List : Expr
    {
        public ImmutableArray<Expr> Exprs { get; }

        public List(ImmutableArray<Expr> exprs)
        {
            Exprs = exprs;
        }
    }
    class IfThenElse : Expr
    {
        public Expr Condition { get; }
        public ImmutableArray<Expr> IfTrueStmts { get; }
        public ImmutableArray<Expr> IfFalseStmts { get; }

        public IfThenElse(Expr condition, ImmutableArray<Expr> ifTrueStmts, ImmutableArray<Expr> ifFalseStmts)
        {
            Condition = condition;
            IfTrueStmts = ifTrueStmts;
            IfFalseStmts = ifFalseStmts;
        }
    }
}