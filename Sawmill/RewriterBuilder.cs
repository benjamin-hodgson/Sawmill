using System.Collections.Immutable;

namespace Sawmill;

/// <summary>
/// Tools for building rewriters.
/// </summary>
public static class RewriterBuilder
{
    /// <summary>
    /// Create a new <see cref="RewriterBuilder{T}"/>.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type</typeparam>
    public static RewriterBuilder<T> For<T>()
        => new(ImmutableList.Create<(Type, IRewriter<T>)>());
}

/// <summary>
/// Tools for building rewriters.
/// </summary>
/// <typeparam name="T">The rewritable tree type</typeparam>
public class RewriterBuilder<T>
{
    private readonly ImmutableList<(Type, IRewriter<T>)> _rewriters;

    internal RewriterBuilder(ImmutableList<(Type, IRewriter<T>)> rewriters)
    {
        _rewriters = rewriters;
    }

    /// <summary>
    /// Handle a single subclass of <typeparamref name="T"/>
    /// </summary>
    public RewriterBuilder<T> Case<TSub>(
        Func<RewriterBuilderCase<object, T, TSub>, IRewriter<T>> builderAction
    ) where TSub : T
    {
        if (builderAction == null)
        {
            throw new ArgumentNullException(nameof(builderAction));
        }
        var builder = new RewriterBuilderCase<object, T, TSub>(
            ImmutableList.Create<Func<TSub, int>>(),
            ImmutableList.Create<GetChildrenDelegate<T, TSub>>(),
            (oldValue, newChildrenEnumerator) => (false, 0, new object())
        );
        var completedBuilder = builderAction(builder);
        return new RewriterBuilder<T>(_rewriters.Add((typeof(TSub), completedBuilder)));
    }

    /// <summary>
    /// Build a rewriter
    /// </summary>
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
internal delegate (bool changed, int consumed, TArgs args) GetCtorArgsDelegate<TBase, TSub, TArgs>(ReadOnlySpan<TBase> newChildren, TSub oldValue);

/// <summary>
/// Tools for building rewriters for a single subclass of a base type.
/// </summary>
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
    /// Select a field from the subclass
    /// </summary>
    public RewriterBuilderCase<(TArgs, U), TBase, TSub> Field<U>(Func<TSub, U> field)
    {
        if (field == null)
        {
            throw new ArgumentNullException(nameof(field));
        }
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
    /// Select an immediate child
    /// </summary>
    public RewriterBuilderCase<(TArgs, TBase), TBase, TSub> Child(Func<TSub, TBase> child)
    {
        if (child == null)
        {
            throw new ArgumentNullException(nameof(child));
        }
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
    public RewriterBuilderCase<(TArgs, ImmutableList<TBase>), TBase, TSub> Children(Func<TSub, ImmutableList<TBase>> children)
    {
        if (children == null)
        {
            throw new ArgumentNullException(nameof(children));
        }

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

internal class CompletedRewriterBuilderCase<TArgs, TBase, TSub> : IRewriter<TBase> where TSub : TBase
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
    public static IRewriter<TBase> ConstructWith<T, TBase, TSub>(
        this RewriterBuilderCase<T, TBase, TSub> builder,
        Func<T, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<TBase, TSub>(
        this RewriterBuilderCase<object, TBase, TSub> builder,
        Func<TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
        return builder.ConstructWith(
            args => constructor()
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    public static IRewriter<TBase> ConstructWith<U1, TBase, TSub>(
        this RewriterBuilderCase<(object, U1), TBase, TSub> builder,
        Func<U1, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
        return builder.ConstructWith(
            args => constructor(
                args.Item2
            )
        );
    }

    /// <summary>
    /// Rebuild the subtype with the supplied function, after flattening the tuple.
    /// </summary>
    public static IRewriter<TBase> ConstructWith<U1, U2, TBase, TSub>(
        this RewriterBuilderCase<((object, U1), U2), TBase, TSub> builder,
        Func<U1, U2, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, TBase, TSub>(
        this RewriterBuilderCase<(((object, U1), U2), U3), TBase, TSub> builder,
        Func<U1, U2, U3, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, TBase, TSub>(
        this RewriterBuilderCase<((((object, U1), U2), U3), U4), TBase, TSub> builder,
        Func<U1, U2, U3, U4, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, TBase, TSub>(
        this RewriterBuilderCase<(((((object, U1), U2), U3), U4), U5), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, TBase, TSub>(
        this RewriterBuilderCase<((((((object, U1), U2), U3), U4), U5), U6), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, U7, TBase, TSub>(
        this RewriterBuilderCase<(((((((object, U1), U2), U3), U4), U5), U6), U7), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, U7, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
    public static IRewriter<TBase> ConstructWith<U1, U2, U3, U4, U5, U6, U7, U8, TBase, TSub>(
        this RewriterBuilderCase<((((((((object, U1), U2), U3), U4), U5), U6), U7), U8), TBase, TSub> builder,
        Func<U1, U2, U3, U4, U5, U6, U7, U8, TSub> constructor
    ) where TSub : TBase
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (constructor == null)
        {
            throw new ArgumentNullException(nameof(constructor));
        }
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
