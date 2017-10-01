using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    /// <summary>
    /// Tools for building rewriters.
    /// </summary>
    /// <typeparam name="T">The rewritable tree type</typeparam>
    public class RewriterBuilder<T>
    {
        private readonly ImmutableDictionary<Type, IRewriter<T>> _rewriters;

        private RewriterBuilder(ImmutableDictionary<Type, IRewriter<T>> rewriters)
        {
            _rewriters = rewriters;
        }

        /// <summary>
        /// Handle a single subclass of <typeparamref name="T"/>
        /// </summary>
        public static RewriterBuilder<T> Case<TSub>(
            Func<RewriterBuilderCase<object, T, TSub>, IRewriter<T>> builderAction
        ) where TSub : T
            => new RewriterBuilder<T>(ImmutableDictionary.Create<Type, IRewriter<T>>())
                .And<TSub>(builderAction);
        
        /// <summary>
        /// Handle a single subclass of <typeparamref name="T"/>
        /// </summary>
        public RewriterBuilder<T> And<TSub>(
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
            return new RewriterBuilder<T>(_rewriters.Add(typeof(TSub), completedBuilder));
        }

        /// <summary>
        /// Build a rewriter
        /// </summary>
        public IRewriter<T> Build()
            => new BuilderRewriter(_rewriters.ToDictionary(kv => kv.Key, kv => kv.Value));

        private class BuilderRewriter : IRewriter<T>
        {
            private readonly Dictionary<Type, IRewriter<T>> _rewriters;

            public BuilderRewriter(Dictionary<Type, IRewriter<T>> rewriters)
            {
                _rewriters = rewriters;
            }

            public Children<T> GetChildren(T value)
                => _rewriters[value.GetType()].GetChildren(value);

            public T RewriteChildren(Func<T, T> transformer, T oldValue)
                => _rewriters[oldValue.GetType()].RewriteChildren(transformer, oldValue);

            public T SetChildren(Children<T> newChildren, T oldValue)
                => _rewriters[oldValue.GetType()].SetChildren(newChildren, oldValue);
        }
    }

    internal delegate Children<TBase> GetChildrenDelegate<TBase, TSub>(
        TSub value,
        Children<TBase> acc
    );

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
                        var thisChild = child(value);
                        switch (acc.NumberOfChildren)
                        {
                            case NumberOfChildren.None:
                                return Sawmill.Children.One(thisChild);
                            case NumberOfChildren.One:
                                return Sawmill.Children.Two(acc.First, thisChild);
                            case NumberOfChildren.Two:
                                return Sawmill.Children.Many(new List<TBase> { acc.First, acc.Second, thisChild });
                            case NumberOfChildren.Many:
                                var many = (List<TBase>)acc.Many;
                                many.Add(thisChild);
                                return Sawmill.Children.Many(many);
                        }
                        throw new Exception("unreachable");
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
        /// Select an enumerable of children, of type <typeparamref name="TEnumerable"/>.
        /// </summary>
        public RewriterBuilderCase<(TArgs, TEnumerable), TBase, TSub> Children<TEnumerable>(Func<TSub, TEnumerable> children) where TEnumerable : IEnumerable<TBase>
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            return new RewriterBuilderCase<(TArgs, TEnumerable), TBase, TSub>(
                GetChildrenDelegates.Add(
                    (value, acc) =>
                    {
                        var many = (List<TBase>)acc.Many;
                        many.AddRange(children(value));
                        return Sawmill.Children.Many(many);
                    }
                ),
                (oldValue, newChildrenEnumerator) =>
                {
                    var (changed, rest) = GetSetChildrenCtorArgs(oldValue, newChildrenEnumerator);
                    var oldChildren = children(oldValue);

                    var newChildren = EnumerableBuilder<TBase>.Create<TEnumerable>();
                    foreach (var oldChild in oldChildren)
                    {
                        newChildrenEnumerator.MoveNext();
                        var newChild = newChildrenEnumerator.Current;

                        changed = changed || !ReferenceEquals(oldChild, newChild);
                        newChildren.Add(newChild);
                    }

                    
                    return (changed, (rest, newChildren.Build()));
                },
                (oldValue, func) =>
                {
                    var (changed, rest) = GetRewriteChildrenCtorArgs(oldValue, func);
                    var oldChildren = children(oldValue);

                    var newChildren = EnumerableBuilder<TBase>.Create<TEnumerable>();
                    foreach (var oldChild in oldChildren)
                    {
                        var newChild = func(oldChild);

                        changed = changed || !ReferenceEquals(oldChild, newChild);
                        newChildren.Add(newChild);
                    }

                    
                    return (changed, (rest, newChildren.Build()));
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
            var children = _childrenCount.HasValue && _childrenCount.Value <= 2
                ? Children.None<TBase>()
                : Children.Many(new List<TBase>());
            foreach (var action in _getChildrenDelegates)
            {
                children = action((TSub)value, children);
            }
            return children;
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
            return new CompletedRewriterBuilderCase<object, TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<(object, U1), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<((object, U1), U2), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<(((object, U1), U2), U3), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<((((object, U1), U2), U3), U4), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<(((((object, U1), U2), U3), U4), U5), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<((((((object, U1), U2), U3), U4), U5), U6), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<(((((((object, U1), U2), U3), U4), U5), U6), U7), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }

        /// <summary>
        /// Rebuild the subtype with the supplied function.
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
            return new CompletedRewriterBuilderCase<((((((((object, U1), U2), U3), U4), U5), U6), U7), U8), TBase, TSub>(
                builder.GetChildrenDelegates.ToArray(),
                builder.GetSetChildrenCtorArgs,
                args => constructor(
                    args.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item1.Item2,
                    args.Item1.Item1.Item2,
                    args.Item1.Item2,
                    args.Item2
                ),
                builder.ChildrenCount
            );
        }
    }
}