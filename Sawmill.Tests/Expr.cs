using System.Collections.Immutable;

namespace Sawmill.Tests;

internal abstract class Expr : IRewritable<Expr>
{
    public abstract int CountChildren();
    public abstract void GetChildren(Span<Expr> children);
    public abstract Expr SetChildren(ReadOnlySpan<Expr> newChildren);
}

internal class Lit : Expr
{
    public int Value { get; }

    public Lit(int value)
    {
        Value = value;
    }

    public override int CountChildren() => 0;

    public override void GetChildren(Span<Expr> children) { }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => this;

    public override bool Equals(object? obj)
        => obj is Lit l
        && l.Value == this.Value;

    public override int GetHashCode()
        => HashCode.Combine(Value);
}

internal class Neg : Expr
{
    public Expr Operand { get; }

    public Neg(Expr operand)
    {
        Operand = operand;
    }

    public override int CountChildren() => 1;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Operand;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new Neg(newChildren[0]);

    public override bool Equals(object? obj)
        => obj is Neg n
        && n.Operand == this.Operand;

    public override int GetHashCode()
        => HashCode.Combine(Operand);
}

internal class Add : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public Add(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override int CountChildren() => 2;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Left;
        children[1] = Right;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new Add(newChildren[0], newChildren[1]);

    public override bool Equals(object? obj)
        => obj is Add a
        && a.Left == this.Left
        && a.Right == this.Right;

    public override int GetHashCode()
        => HashCode.Combine(Left, Right);
}

internal class Ternary : Expr
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

    public override int CountChildren() => 3;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Condition;
        children[1] = ThenBranch;
        children[2] = ElseBranch;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren)
        => new Ternary(newChildren[0], newChildren[1], newChildren[2]);

    public override bool Equals(object? obj)
        => obj is Ternary t
        && t.Condition == this.Condition
        && t.ThenBranch == this.ThenBranch
        && t.ElseBranch == this.ElseBranch;

    public override int GetHashCode()
        => HashCode.Combine(Condition, ThenBranch, ElseBranch);
}

internal class List : Expr
{
    public ImmutableList<Expr> Exprs { get; }

    public List(ImmutableList<Expr> exprs)
    {
        Exprs = exprs;
    }

    public override int CountChildren() => Exprs.Count;

    public override void GetChildren(Span<Expr> children)
    {
        for (var i = 0; i < Exprs.Count; i++)
        {
            children[i] = Exprs[i];
        }
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new List(newChildren.ToArray().ToImmutableList());

    public override bool Equals(object? obj)
        => obj is List l
        && l.Exprs.SequenceEqual(this.Exprs);

    public override int GetHashCode()
    {
        var x = new HashCode();
        foreach (var expr in Exprs)
        {
            x.Add(expr);
        }
        return x.ToHashCode();
    }
}

internal class IfThenElse : Expr
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

    public override int CountChildren() => 1 + IfTrueStmts.Count + IfFalseStmts.Count;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Condition;
        var t = children[1..];
        for (var i = 0; i < IfTrueStmts.Count; i++)
        {
            t[i] = IfTrueStmts[i];
        }
        var f = t[IfTrueStmts.Count..];
        for (var i = 0; i < IfFalseStmts.Count; i++)
        {
            f[i] = IfFalseStmts[i];
        }
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren)
        => new IfThenElse(
            newChildren[0],
            newChildren.Slice(1, IfTrueStmts.Count).ToArray().ToImmutableList(),
            newChildren.Slice(IfTrueStmts.Count + 1, IfFalseStmts.Count).ToArray().ToImmutableList()
        );

    public override bool Equals(object? obj)
        => obj is IfThenElse i
        && i.Condition == this.Condition
        && i.IfTrueStmts.SequenceEqual(this.IfTrueStmts)
        && i.IfFalseStmts.SequenceEqual(this.IfFalseStmts);

    public override int GetHashCode()
    {
        var x = new HashCode();
        x.Add(Condition);
        foreach (var expr in IfTrueStmts)
        {
            x.Add(expr);
        }
        foreach (var expr in IfFalseStmts)
        {
            x.Add(expr);
        }
        return x.ToHashCode();
    }
}
