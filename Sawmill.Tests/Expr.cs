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
        public ImmutableList<Expr> Exprs { get; }

        public List(ImmutableList<Expr> exprs)
        {
            Exprs = exprs;
        }
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
    }
}