using System.Collections.Immutable;

namespace Sawmill.Tests;

#pragma warning disable SA1402  // File may only contain a single type

internal abstract record Expr : IRewritable<Expr>
{
    public abstract int CountChildren();

    public abstract void GetChildren(Span<Expr> children);

    public abstract Expr SetChildren(ReadOnlySpan<Expr> newChildren);
}

internal sealed record Lit(int Value) : Expr
{
    public override int CountChildren() => 0;

    public override void GetChildren(Span<Expr> children)
    {
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => this;
}

internal sealed record Neg(Expr Operand) : Expr
{
    public override int CountChildren() => 1;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Operand;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new Neg(newChildren[0]);
}

internal sealed record Add(Expr Left, Expr Right) : Expr
{
    public override int CountChildren() => 2;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Left;
        children[1] = Right;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new Add(newChildren[0], newChildren[1]);
}

internal sealed record Ternary(Expr Condition, Expr ThenBranch, Expr ElseBranch) : Expr
{
    public override int CountChildren() => 3;

    public override void GetChildren(Span<Expr> children)
    {
        children[0] = Condition;
        children[1] = ThenBranch;
        children[2] = ElseBranch;
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren)
        => new Ternary(newChildren[0], newChildren[1], newChildren[2]);
}

internal sealed record List(ImmutableList<Expr> Exprs) : Expr
{
    public override int CountChildren() => Exprs.Count;

    public override void GetChildren(Span<Expr> children)
    {
        for (var i = 0; i < Exprs.Count; i++)
        {
            children[i] = Exprs[i];
        }
    }

    public override Expr SetChildren(ReadOnlySpan<Expr> newChildren) => new List(newChildren.ToArray().ToImmutableList());

    public bool Equals(List? list)
        => list != null && list.Exprs.SequenceEqual(Exprs);

    public override int GetHashCode()
    {
        var x = default(HashCode);
        foreach (var expr in Exprs)
        {
            x.Add(expr);
        }

        return x.ToHashCode();
    }
}

internal sealed record IfThenElse(
    Expr Condition,
    ImmutableList<Expr> IfTrueStmts,
    ImmutableList<Expr> IfFalseStmts
) : Expr
{
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

    public bool Equals(IfThenElse? ite)
        => ite != null
            && ite.Condition == Condition
            && ite.IfTrueStmts.SequenceEqual(IfTrueStmts)
            && ite.IfFalseStmts.SequenceEqual(IfFalseStmts);

    public override int GetHashCode()
    {
        var x = default(HashCode);
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
#pragma warning restore SA1402
