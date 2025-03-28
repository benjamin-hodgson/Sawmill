using System;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill;

#pragma warning disable SA1402  // File may only contain a single type

/// <summary>
/// Tools for building rewriters.
/// </summary>
public static class RewriterBuilder
{
    /// <summary>
    /// Create a new <see cref="RewriterBuilder{T}" />.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type.</typeparam>
    /// <returns>A <see cref="RewriterBuilder{T}" />.</returns>
    public static RewriterBuilder<T> For<T>()
        => new(ImmutableList.Create<(Type, IRewriter<T>)>());
}

/// <summary>
/// Tools for building rewriters.
/// </summary>
/// <typeparam name="T">The rewritable tree type.</typeparam>
public class RewriterBuilder<T>
{
    private readonly ImmutableList<(Type, IRewriter<T>)> _rewriters;

    internal RewriterBuilder(ImmutableList<(Type Type, IRewriter<T> Rewriter)> rewriters)
    {
        _rewriters = rewriters;
    }

    /// <summary>
    /// Handle a single subclass of <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="TSub">The concrete subclass type.</typeparam>
    /// <param name="builderAction">The action defining the builder for this subclass.</param>
    /// <returns>A <see cref="RewriterBuilder{T}" />.</returns>
    public RewriterBuilder<T> Case<TSub>(
        Func<RewriterBuilderCase<object, T, TSub>, IRewriter<T>> builderAction
    )
        where TSub : T
    {
        ArgumentNullException.ThrowIfNull(builderAction);

        var builder = new RewriterBuilderCase<object, T, TSub>(
            ImmutableList.Create<Func<TSub, int>>(),
            ImmutableList.Create<GetChildrenDelegate<T, TSub>>(),
            (oldValue, newChildrenEnumerator) => (false, 0, new object())
        );
        var completedBuilder = builderAction(builder);
        return new RewriterBuilder<T>(_rewriters.Add((typeof(TSub), completedBuilder)));
    }

    /// <summary>
    /// Build a rewriter.
    /// </summary>
    /// <returns>A rewriter.</returns>
    public IRewriter<T> Build()
        => new BuilderRewriter(_rewriters.ToArray());

    private class BuilderRewriter : IRewriter<T>
    {
        private readonly (Type, IRewriter<T>)[] _rewriters;

        public BuilderRewriter((Type, IRewriter<T>)[] rewriters)
        {
            _rewriters = rewriters;
        }

        public int CountChildren(T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            foreach (var kv in _rewriters)
            {
                if (kv.Item1.IsAssignableFrom(value.GetType()))
                {
                    return kv.Item2.CountChildren(value);
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"Unknown type {value.GetType()}. Are you missing an And clause from your RewriterBuilder?"
            );
        }

        public void GetChildren(Span<T> children, T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            foreach (var kv in _rewriters)
            {
                if (kv.Item1.IsAssignableFrom(value.GetType()))
                {
                    kv.Item2.GetChildren(children, value);
                    return;
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"Unknown type {value.GetType()}. Are you missing an And clause from your RewriterBuilder?"
            );
        }

        public T SetChildren(ReadOnlySpan<T> newChildren, T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            foreach (var kv in _rewriters)
            {
                if (kv.Item1.IsAssignableFrom(value.GetType()))
                {
                    return kv.Item2.SetChildren(newChildren, value);
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"Unknown type {value.GetType()}. Are you missing an And clause from your RewriterBuilder?"
            );
        }
    }
}

internal delegate int GetChildrenDelegate<TBase, TSub>(Span<TBase> children, int position, TSub value);

internal delegate (bool Changed, int Consumed, TArgs Args) GetCtorArgsDelegate<TBase, TSub, TArgs>(ReadOnlySpan<TBase> newChildren, TSub oldValue);

/// <summary>
/// Tools for building rewriters for a single subclass of a base type.
/// </summary>
/// <typeparam name="TArgs">The constructor arguments.</typeparam>
/// <typeparam name="TBase">The rewritable tree type.</typeparam>
/// <typeparam name="TSub">The sublcass which this case handles.</typeparam>
public sealed class RewriterBuilderCase<TArgs, TBase, TSub>
{
    internal ImmutableList<Func<TSub, int>> CountChildrenDelegates { get; }

    internal ImmutableList<GetChildrenDelegate<TBase, TSub>> GetChildrenDelegates { get; }

    internal GetCtorArgsDelegate<TBase, TSub, TArgs> GetSetChildrenCtorArgs { get; }

    internal RewriterBuilderCase(
        ImmutableList<Func<TSub, int>> countChildrenDelegates,
        ImmutableList<GetChildrenDelegate<TBase, TSub>> getChildrenDelegates,
        GetCtorArgsDelegate<TBase, TSub, TArgs> getSetChildrenCtorArgs
    )
    {
        CountChildrenDelegates = countChildrenDelegates;
        GetChildrenDelegates = getChildrenDelegates;
        GetSetChildrenCtorArgs = getSetChildrenCtorArgs;
    }

    /// <summary>
    /// Select a field from the subclass.
    /// </summary>
    /// <typeparam name="U">The field type.</typeparam>
    /// <param name="field">The field selector.</param>
    /// <returns>A copy of this <see cref="RewriterBuilderCase" /> with an additional constructor argument.</returns>
    public RewriterBuilderCase<(TArgs Args, U Field), TBase, TSub> Field<U>(Func<TSub, U> field)
    {
        ArgumentNullException.ThrowIfNull(field);

        return new RewriterBuilderCase<(TArgs, U), TBase, TSub>(
            CountChildrenDelegates,
            GetChildrenDelegates,
            (newChildren, oldValue) =>
            {
                var (changed, consumed, rest) = GetSetChildrenCtorArgs(newChildren, oldValue);
                return (changed, consumed, (rest, field(oldValue)));
            }
        );
    }

    /// <summary>
    /// Select an immediate child.
    /// </summary>
    /// <param name="child">The child selector.</param>
    /// <returns>A copy of this <see cref="RewriterBuilderCase" /> with an additional constructor argument.</returns>
    public RewriterBuilderCase<(TArgs Args, TBase Child), TBase, TSub> Child(Func<TSub, TBase> child)
    {
        ArgumentNullException.ThrowIfNull(child);

        return new RewriterBuilderCase<(TArgs, TBase), TBase, TSub>(
            CountChildrenDelegates.Add(value => 1),
            GetChildrenDelegates.Add(
                (receiver, index, value) =>
                {
                    receiver[index] = child(value);
                    return 1;
                }
            ),
            (newChildren, oldValue) =>
            {
                var (changed, consumed, rest) = GetSetChildrenCtorArgs(newChildren, oldValue);
                var oldChild = child(oldValue);

                var newChild = newChildren[consumed];
                consumed++;

                return (changed || !ReferenceEquals(oldChild, newChild), consumed, (rest, newChild));
            }
        );
    }

    /// <summary>
    /// Select a list of children.
    /// </summary>
    /// <param name="children">The children selector.</param>
    /// <returns>A copy of this <see cref="RewriterBuilderCase" /> with an additional constructor argument.</returns>
    public RewriterBuilderCase<(TArgs Args, ImmutableList<TBase> Children), TBase, TSub> Children(Func<TSub, ImmutableList<TBase>> children)
    {
        ArgumentNullException.ThrowIfNull(children);

        return new RewriterBuilderCase<(TArgs, ImmutableList<TBase>), TBase, TSub>(
            CountChildrenDelegates.Add(value => children(value).Count),
            GetChildrenDelegates.Add(
                (receiver, index, value) =>
                {
                    var total = 0;
                    foreach (var child in children(value))
                    {
                        receiver[index + total] = child;
                        total++;
                    }

                    return total;
                }
            ),
            (newChildren, oldValue) =>
            {
                var (changed, consumed, rest) = GetSetChildrenCtorArgs(newChildren, oldValue);
                var oldChildren = children(oldValue);

                var builder = oldChildren.ToBuilder();
                for (var i = 0; i < builder.Count; i++)
                {
                    var oldChild = builder[i];

                    if (consumed >= newChildren.Length)
                    {
                        throw new InvalidOperationException("Reached end of enumeration");
                    }

                    var newChild = newChildren[consumed];
                    consumed++;

                    if (!ReferenceEquals(oldChild, newChild))
                    {
                        changed = true;
                        builder[i] = newChild;
                    }
                }

                return (changed, consumed, (rest, builder.ToImmutable()));
            }
        );
    }
}

internal class CompletedRewriterBuilderCase<TArgs, TBase, TSub> : IRewriter<TBase>
    where TSub : TBase
{
    private readonly Func<TSub, int>[] _countChildrenDelegates;
    private readonly GetChildrenDelegate<TBase, TSub>[] _getChildrenDelegates;
    private readonly GetCtorArgsDelegate<TBase, TSub, TArgs> _getSetChildrenCtorArgs;
    private readonly Func<TArgs, TSub> _ctor;

    internal CompletedRewriterBuilderCase(
        Func<TSub, int>[] countChildrenDelegates,
        GetChildrenDelegate<TBase, TSub>[] getChildrenDelegates,
        GetCtorArgsDelegate<TBase, TSub, TArgs> getSetChildrenCtorArgs,
        Func<TArgs, TSub> ctor
    )
    {
        _countChildrenDelegates = countChildrenDelegates;
        _getChildrenDelegates = getChildrenDelegates;
        _getSetChildrenCtorArgs = getSetChildrenCtorArgs;
        _ctor = ctor;
    }

    public int CountChildren(TBase value)
        => _countChildrenDelegates.Select(f => f((TSub)value!)).Sum();

    public void GetChildren(Span<TBase> children, TBase value)
    {
        var index = 0;
        foreach (var action in _getChildrenDelegates)
        {
            index += action(children, index, (TSub)value!);
        }
    }

    public TBase SetChildren(ReadOnlySpan<TBase> newChildren, TBase oldValue)
    {
        var (changed, _, args) = _getSetChildrenCtorArgs(newChildren, (TSub)oldValue!);
        if (changed)
        {
            return _ctor(args);
        }

        return oldValue;
    }
}

/// <summary>
/// Tools for building rewriters for a single subclass of a base type.
/// </summary>
public static class RewriterBuilderCase
{
    /// <summary>
    /// Rebuild the subtype with the supplied function.
    /// </summary>
    /// <typeparam name="T">The arguments of the builder.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<T, TBase, TSub>(
        this RewriterBuilderCase<T, TBase, TSub> builder,
        Func<T, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return new CompletedRewriterBuilderCase<T, TBase, TSub>(
            builder.CountChildrenDelegates.ToArray(),
            builder.GetChildrenDelegates.ToArray(),
            builder.GetSetChildrenCtorArgs,
            constructor
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function.
    /// </summary>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<TBase, TSub>(
        this RewriterBuilderCase<object, TBase, TSub> builder,
        Func<TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor()
        );
    }

#pragma warning disable SA1414  // Tuple types in signatures should have element names
    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, TBase, TSub>(
        this RewriterBuilderCase<(object, U1), TBase, TSub> builder,
        Func<U1, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, TBase, TSub>(
        this RewriterBuilderCase<((object, U1), U2), TBase, TSub> builder,
        Func<U1, U2, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, TBase, TSub>(
        this RewriterBuilderCase<(((object, U1), U2), U3), TBase, TSub> builder,
        Func<U1, U2, U3, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="U4">Constructor argument 4.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, TBase, TSub>(
        this RewriterBuilderCase<((((object, U1), U2), U3), U4), TBase, TSub> builder,
        Func<U1, U2, U3, U4, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="U4">Constructor argument 4.</typeparam>
    /// <typeparam name="U5">Constructor argument 5.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, TBase, TSub>(
        this RewriterBuilderCase<(((((object, U1), U2), U3), U4), U5), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="U4">Constructor argument 4.</typeparam>
    /// <typeparam name="U5">Constructor argument 5.</typeparam>
    /// <typeparam name="U6">Constructor argument 6.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, TBase, TSub>(
        this RewriterBuilderCase<((((((object, U1), U2), U3), U4), U5), U6), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="U4">Constructor argument 4.</typeparam>
    /// <typeparam name="U5">Constructor argument 5.</typeparam>
    /// <typeparam name="U6">Constructor argument 6.</typeparam>
    /// <typeparam name="U7">Constructor argument 7.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, U7, TBase, TSub>(
        this RewriterBuilderCase<(((((((object, U1), U2), U3), U4), U5), U6), U7), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, U7, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    /// <typeparam name="U1">Constructor argument 1.</typeparam>
    /// <typeparam name="U2">Constructor argument 2.</typeparam>
    /// <typeparam name="U3">Constructor argument 3.</typeparam>
    /// <typeparam name="U4">Constructor argument 4.</typeparam>
    /// <typeparam name="U5">Constructor argument 5.</typeparam>
    /// <typeparam name="U6">Constructor argument 6.</typeparam>
    /// <typeparam name="U7">Constructor argument 7.</typeparam>
    /// <typeparam name="U8">Constructor argument 8.</typeparam>
    /// <typeparam name="TBase">The rewritable tree type.</typeparam>
    /// <typeparam name="TSub">The subtype.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="constructor">The constructor function.</param>
    /// <returns>A rewriter.</returns>
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, U7, U8, TBase, TSub>(
        this RewriterBuilderCase<((((((((object, U1), U2), U3), U4), U5), U6), U7), U8), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, U7, U8, TSub> constructor
    )
        where TSub : TBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(constructor);

        return builder.ConstructWith(
            args => constructor(
                args.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item1.Item2,
                args.Item1.Item1.Item2,
                args.Item1.Item2,
                args.Item2
            )
        );
    }
}

#pragma warning restore SA1402
#pragma warning restore SA1414
