using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
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
            => new RewriterBuilder<T>(ImmutableList.Create<(Type, IRewriter<T>)>());
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
                ImmutableList.Create<GetChildrenDelegate<T, TSub>>(),
                (oldValue, newChildrenEnumerator) => (false, new object()),
                (oldValue, func) => (false, new object()),
                0        
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

            public Children<T> GetChildren(T value)
            {
                foreach (var kv in _rewriters)
                {
                    if (kv.Item1.IsAssignableFrom(value.GetType()))
                    {
                        return kv.Item2.GetChildren(value);
                    }
                }
                throw new ArgumentOutOfRangeException(
                    $"Unknown type {value.GetType()}. Are you missing an And clause from your RewriterBuilder?",
                    nameof(value)
                );
            }

            public T RewriteChildren(Func<T, T> transformer, T oldValue)
            {
                foreach (var kv in _rewriters)
                {
                    if (kv.Item1.IsAssignableFrom(oldValue.GetType()))
                    {
                        return kv.Item2.RewriteChildren(transformer, oldValue);
                    }
                }
                throw new ArgumentOutOfRangeException(
                    $"Unknown type {oldValue.GetType()}. Are you missing an And clause from your RewriterBuilder?",
                    nameof(oldValue)
                );
            }

            public T SetChildren(Children<T> newChildren, T oldValue)
            {
                foreach (var kv in _rewriters)
                {
                    if (kv.Item1.IsAssignableFrom(oldValue.GetType()))
                    {
                        return kv.Item2.SetChildren(newChildren, oldValue);
                    }
                }
                throw new ArgumentOutOfRangeException(
                    $"Unknown type {oldValue.GetType()}. Are you missing an And clause from your RewriterBuilder?",
                    nameof(oldValue)
                );
            }
        }
    }

    internal class ChildrenBuilder<T>
    {
        private NumberOfChildren _numberOfChildren = NumberOfChildren.None;
        private T _first;
        private T _second;
        private ImmutableList<T>.Builder _many;

        public void Add(T child)
        {
            switch (_numberOfChildren)
            {
                case NumberOfChildren.None:
                    _first = child;
                    _numberOfChildren = NumberOfChildren.One;
                    break;
                case NumberOfChildren.One:
                    _second = child;
                    _numberOfChildren = NumberOfChildren.Two;
                    break;
                case NumberOfChildren.Two:
                    _many = ImmutableList.CreateBuilder<T>();
                    _many.Add(_first);
                    _many.Add(_second);
                    _many.Add(child);
                    _numberOfChildren = NumberOfChildren.Many;
                    break;
                case NumberOfChildren.Many:
                    _many.Add(child);
                    break;
            }
        }

        public Children<T> Build()
        {
            switch (_numberOfChildren)
            {
                case NumberOfChildren.None:
                    return Children.None<T>();
                case NumberOfChildren.One:
                    return Children.One<T>(_first);
                case NumberOfChildren.Two:
                    return Children.Two<T>(_first, _second);
                case NumberOfChildren.Many:
                    return Children.Many<T>(_many.ToImmutable());
            }
            throw new InvalidOperationException("Unreachable");
        }
    }
    internal delegate void GetChildrenDelegate<TBase, TSub>(TSub value, ChildrenBuilder<TBase> acc);

    /// <summary>
    /// Tools for building rewriters for a single subclass of a base type.
    /// </summary>
    public sealed class RewriterBuilderCase<TArgs, TBase, TSub>
    {
        internal ImmutableList<GetChildrenDelegate<TBase, TSub>> GetChildrenDelegates { get; }
        internal Func<TSub, IEnumerator<TBase>, (bool, TArgs)> GetSetChildrenCtorArgs { get; }
        internal Func<TSub, Func<TBase, TBase>, (bool, TArgs)> GetRewriteChildrenCtorArgs { get; }
        internal int? ChildrenCount { get; }

        internal RewriterBuilderCase(
            ImmutableList<GetChildrenDelegate<TBase, TSub>> getChildrenDelegates,
            Func<TSub, IEnumerator<TBase>, (bool, TArgs)> getSetChildrenCtorArgs,
            Func<TSub, Func<TBase, TBase>, (bool, TArgs)> getRewriteChildrenCtorArgs,
            int? childrenCount
        )
        {
            GetChildrenDelegates = getChildrenDelegates;
            GetSetChildrenCtorArgs = getSetChildrenCtorArgs;
            GetRewriteChildrenCtorArgs = getRewriteChildrenCtorArgs;
            ChildrenCount = childrenCount;
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
                GetChildrenDelegates,
                (oldValue, newChildrenEnumerator) =>
                {
                    var (changed, rest) = GetSetChildrenCtorArgs(oldValue, newChildrenEnumerator);
                    return (changed, (rest, field(oldValue)));
                },
                (oldValue, func) =>
                {
                    var (changed, rest) = GetRewriteChildrenCtorArgs(oldValue, func);
                    return (changed, (rest, field(oldValue)));
                },
                ChildrenCount
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
                GetChildrenDelegates.Add(
                    (value, acc) =>
                    {
                        acc.Add(child(value));
                    }
                ),
                (oldValue, newChildrenEnumerator) =>
                {
                    var (changed, rest) = GetSetChildrenCtorArgs(oldValue, newChildrenEnumerator);
                    var oldChild = child(oldValue);

                    newChildrenEnumerator.MoveNext();
                    var newChild = newChildrenEnumerator.Current;
                    
                    return (changed || !ReferenceEquals(oldChild, newChild), (rest, newChild));
                },
                (oldValue, func) =>
                {
                    var (changed, rest) = GetRewriteChildrenCtorArgs(oldValue, func);
                    var oldChild = child(oldValue);

                    var newChild = func(oldChild);
                    
                    return (changed || !ReferenceEquals(oldChild, newChild), (rest, newChild));
                },
                ChildrenCount.HasValue ? ChildrenCount.Value + 1 : ChildrenCount
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
                GetChildrenDelegates.Add(
                    (value, acc) =>
                    {
                        foreach (var child in children(value))
                        {
                            acc.Add(child);
                        }
                    }
                ),
                (oldValue, newChildrenEnumerator) =>
                {
                    var (changed, rest) = GetSetChildrenCtorArgs(oldValue, newChildrenEnumerator);
                    var oldChildren = children(oldValue);

                    var builder = oldChildren.ToBuilder();
                    for (var i = 0; i < builder.Count; i++)
                    {
                        var oldChild = builder[i];

                        var hasNext = newChildrenEnumerator.MoveNext();
                        if (!hasNext)
                        {
                            throw new InvalidOperationException("Reached end of enumeration");
                        }
                        var newChild = newChildrenEnumerator.Current;

                        if (!ReferenceEquals(oldChild, newChild))
                        {
                            changed = true;
                            builder[i] = newChild;
                        }
                    }
                    return (changed, (rest, builder.ToImmutable()));
                },
                (oldValue, func) =>
                {
                    var (changed, rest) = GetRewriteChildrenCtorArgs(oldValue, func);
                    var oldChildren = children(oldValue);

                    var builder = oldChildren.ToBuilder();
                    for (var i = 0; i < builder.Count; i++)
                    {
                        var oldChild = builder[i];
                        var newChild = func(oldChild);

                        if (!ReferenceEquals(oldChild, newChild))
                        {
                            changed = true;
                            builder[i] = newChild;
                        }
                    }
                    return (changed, (rest, builder.ToImmutable()));
                },
                null  // assume there'll be many children
            );
        }
    }

    internal class CompletedRewriterBuilderCase<TArgs, TBase, TSub> : IRewriter<TBase> where TSub : TBase
    {
        private readonly GetChildrenDelegate<TBase, TSub>[] _getChildrenDelegates;
        private readonly Func<TSub, IEnumerator<TBase>, (bool, TArgs)> _getSetChildrenCtorArgs;
        private readonly Func<TArgs, TSub> _ctor;
        private readonly int? _childrenCount;

        internal CompletedRewriterBuilderCase(
            GetChildrenDelegate<TBase, TSub>[] getChildrenDelegates,
            Func<TSub, IEnumerator<TBase>, (bool, TArgs)> getSetChildrenCtorArgs,
            Func<TArgs, TSub> ctor,
            int? childrenCount
        )
        {
            _getChildrenDelegates = getChildrenDelegates;
            _getSetChildrenCtorArgs = getSetChildrenCtorArgs;
            _ctor = ctor;
            _childrenCount = childrenCount;
        }

        public Children<TBase> GetChildren(TBase value)
        {
            var childrenBuilder = new ChildrenBuilder<TBase>();
            foreach (var action in _getChildrenDelegates)
            {
                action((TSub)value, childrenBuilder);
            }
            return childrenBuilder.Build();
        }

        public TBase RewriteChildren(Func<TBase, TBase> transformer, TBase oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        public TBase SetChildren(Children<TBase> newChildren, TBase oldValue)
        {
            var enumerator = newChildren.AsEnumerable().GetEnumerator();
            var (changed, args) = _getSetChildrenCtorArgs((TSub)oldValue, enumerator);
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
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                constructor,
                builder.ChildrenCount
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
}
