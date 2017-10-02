Sawmill
=======

[![Build status](https://ci.appveyor.com/api/projects/status/cc2fnul624198x02?svg=true)](https://ci.appveyor.com/project/benjamin-hodgson/sawmill)

Simple tools for working with immutable trees, based on [_Uniform Boilerplate and List Processing_](http://ndmitchell.com/downloads/paper-uniform_boilerplate_and_list_processing-30_sep_2007.pdf) and developed at Stack Overflow.

Installing
----------

Sawmill is [available on Nuget](https://www.nuget.org/packages/Sawmill/). Dev builds can be found on [our AppVeyor Nuget feed](https://ci.appveyor.com/nuget/sawmill-2d8cq9lwy37i).

Tutorial
--------

Sawmill contains functions which make it easy to work with immutable tree-shaped data such as abstract syntax trees. It factors out the boilerplate associated with recursively traversing a tree, allowing you to write queries and transformations which get straight to the point.

Sawmill is designed to be extremely simple and lightweight (it's built as a set of extension methods for a single simple interface); it works well with modern C# features like lambdas and pattern matching (the days of the clunky old visitor pattern are over!); and it doesn't get in the way when you need to go it alone and write traversals without Sawmill's help.

### Querying a tree

For example, suppose you're working with a simple language of arithmetic expressions featuring literal numbers, variables, addition, and unary subtraction. Here's a function to extract a list of the variables mentioned in a given expression (for example, a compiler writer might want to find the variables captured by a lambda expression):

```csharp
IEnumerable<string> GetVariables(Expr expr)
{
    switch (expr)
    {
        case Lit l:
            return Enumerable.Empty<string>();
        case Var v:
            return new[] { v.Name };
        case Add a:
            return GetVariables(a.Left).Concat(GetVariables(a.Right));
        case Neg n:
            return GetVariables(n.Operand);
    }
    throw new ArgumentOutOfRangeException(nameof(expr));
}
```

The only _interesting_ line of code here is the `Var` case. The rest is just boilerplate to recursively call `GetVariables` on the nodes' children and combine the results. With Sawmill, `GetVariables` is one line of simple, direct code:

```csharp
IEnumerable<string> GetVariables(Expr expr)
    => expr.SelfAndDescendants().OfType<Var>().Select(v => v.Name);
```

`SelfAndDescendants` returns an enumerable containing the current node and all of the nodes in the rest of the tree. The example above uses the standard `OfType` and `Select` LINQ methods to find the names of all the variables mentioned in `expr`.

### Transforming a tree

Tree transformations are simple too. Here's an example of a simple optimisation pass, which removes double-negatives:

```csharp
Expr RemoveDoubleNegation(Expr expr)
    => expr.Rewrite(node =>
        node is Neg n1 && n1.Operand is Neg n2
            ? n2.Operand
            : node
    );
```

`Rewrite` takes a transformation function and rebuilds a tree by applying the function to every node in the tree. Sawmill takes care to avoid rebuilding parts of the tree which the transformation function leaves unchanged, so `Rewrite` will typically be more efficient than a naïve handwritten implementation.

### Putting an expression into normal form

Normalising an expression typically involves repeatedly applying a set of rewrite rules until they can't be applied any more.

For example, to put an arithmetic expression into [_negation normal form_](https://en.wikipedia.org/wiki/Negation_normal_form), so that all of the minus signs appear only next to variables or literal numbers, you distribute `-` over `+` (so `-(3+2)` becomes `(-3)+(-2)`). Since performing this distribution might produce more places where the result of an addition is negated (consider `-((1+2)+3) -> (-(1+2))+(-3)`), you need to do so repeatedly until you can't do it any more.

`RewriteIter` packages up this pattern. It applies a transformation function to every node in the tree from bottom to top, repeating this until the function returns `IterResult.Done<T>()` for every node in the output. (In other words, `rewriter.DescendantsAndSelf(rewriter.RewriteIter(f, x)).Any(n => f(n).Continue) == false`.)

```csharp
Expr ToNegationNormalForm(Expr expr)
    => expr.RewriteIter(node => 
        node is Neg n && n.Operand is Add a
            ? IterResult.Continue<Expr>(new Add(new Neg(a.Left), new Neg(a.Right)))
            : IterResult.Done<Expr>()
    );
```

It'd be pretty tedious to write this operation by hand as a recursive function!

### How it works

To use Sawmill with your own expression types, you implement the `IRewritable` interface. You explain how to read and write the nodes' immediate children, and Sawmill does the boring work of recursively traversing the children's children.

```csharp
abstract class Expr : IRewritable<Expr>
{
    public abstract Children<Expr> GetChildren();
    public abstract Expr SetChildren(Children<Expr> newChildren);
    public Expr RewriteChildren(Func<Expr, Expr> transformer)
        => this.DefaultRewriteChildren(transformer);  // RewriteChildren will usually be implemented using DefaultRewriteChildren, but can be overridden for efficiency
}
// literal numbers are leaf nodes; they have no children
class Lit : Expr
{
    public int Value { get; }

    public override Children<Expr> GetChildren()
        => Children.None<Expr>();
    public override Expr SetChildren(Children<Expr> newChildren)
        => this;
}
// variables also have no children
class Var : Expr
{
    public int Name { get; }

    public override Children<Expr> GetChildren()
        => Children.None<Expr>();
    public override Expr SetChildren(Children<Expr> newChildren)
        => this;
}
class Neg : Expr
{
    public Expr Operand { get; }

    public override Children<Expr> GetChildren()
        => Children.One(this.Operand);
    public override Expr SetChildren(Children<Expr> newChildren)
        => new Neg(newChildren.First);
}
class Add : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public override Children<Expr> GetChildren()
        => Children.Two(this.Left, this.Right);
    public override Expr SetChildren(Children<Expr> newChildren)
        => new Add(newChildren.First, newChildren.Second);
}
```

#### About `RewriteChildren`

`RewriteChildren` is part of the `IRewritable` interface, but most of the time you'll just want to delegate to the `DefaultRewriteChildren` extension method. It's there so that you can override it with a more efficient implementation if necessary. The typical scenario where this makes a significant difference to performance is when your node has a fixed number of children and that number is greater than two.

#### If you can't implement `IRewritable`

There's also an `IRewriter<T>` interface, which is useful if you can't change the tree type to implement `IRewritable`. Sawmill comes bundled with `IRewriter` implementations (and extension methods) for some tree-shaped objects in the BCL, namely `Expression`, `XElement`, and `XmlNode`. In the box you'll also find `RewriterBuilder`, which is a domain-specific language for buiding `IRewriter` implementations, and an experimental reflection-based `AutoRewriter`.
