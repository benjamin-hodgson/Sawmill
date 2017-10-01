using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sawmill
{
    /// <summary>
    /// An experimental implementation of <see cref="IRewriter{T}"/> using reflection.
    /// </summary>
    public sealed class AutoRewriter<T> : IRewriter<T>
    {
        private static readonly Type _t = typeof(T);
        private static readonly Type[] _tArray = new[] { _t };

        private static readonly Type _childrenT = typeof(Children<T>);
        private static readonly PropertyInfo _childrenTNumberOfChildren =
            _childrenT.GetProperty("NumberOfChildren");
        private static readonly PropertyInfo _childrenTFirst =
            _childrenT.GetProperty("First");
        private static readonly PropertyInfo _childrenTSecond =
            _childrenT.GetProperty("Second");
        private static readonly PropertyInfo _childrenTMany =
            _childrenT.GetProperty("Many");

        private static readonly Type _children = typeof(Children);
        private static readonly MethodInfo _childrenOne =
            _children
                .GetMethod("One", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(_t);
        private static readonly MethodInfo _childrenTwo =
            _children
                .GetMethod("Two", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(_t);
        private static readonly MethodInfo _childrenMany =
            _children
                .GetMethod("Many", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(_t);
        
        private static readonly Type _listT = typeof(List<T>);
        private static readonly MethodInfo _listTAdd
            = _listT.GetMethod("Add", new[]{ _t });

        private static readonly Type _iEnumerator = typeof(IEnumerator);
        private static readonly MethodInfo _iEnumeratorMoveNext =
            _iEnumerator
                .GetMethod("MoveNext");

        private static readonly Type _iEnumeratorT = typeof(IEnumerator<T>);
        private static readonly PropertyInfo _iEnumeratorTCurrent =
            _iEnumeratorT
                .GetProperty("Current", _t);
        
        private static readonly Type _iEnumerable1 = typeof(IEnumerable<>);
        private static readonly Type _iEnumerableT = typeof(IEnumerable<T>);
        private static readonly MethodInfo _iEnumerableGetEnumerator =
            typeof(IEnumerable<T>)
                .GetMethods()
                .Single(m => m.Name == "GetEnumerator" && m.ReturnType.Equals(_iEnumeratorT));
        private static readonly MethodInfo _enumerableCount =
            typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == "Count" && m.GetParameters().Length == 1)
                .MakeGenericMethod(_t);

        private static Type _object = typeof(object);
        private static readonly MethodInfo _objectToString = _object.GetMethod("ToString");
        
        private static readonly MethodInfo _stringConcat2 =
            typeof(string)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == "Concat" && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[]{ _object, _object }));

        private static readonly Type _int = typeof(int);

        private static readonly ConstructorInfo _newInvalidOperationException =
            typeof(InvalidOperationException)
                .GetConstructor(new[]{ typeof(string) });

        private static readonly Type _autoRewriterT = typeof(AutoRewriter<T>);
        private static readonly IReadOnlyDictionary<Type, MethodInfo> _enumerableRebuilders
            = new Dictionary<Type, MethodInfo>
            {
                { typeof(IEnumerable<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(IList<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(IReadOnlyList<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(ICollection<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(IReadOnlyCollection<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },

                { typeof(ImmutableArray<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(ImmutableList<T>), _autoRewriterT.GetMethod("RebuildImmutableList", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(List<T>), _autoRewriterT.GetMethod("RebuildList", BindingFlags.Static | BindingFlags.NonPublic) },
                { typeof(T[]), _autoRewriterT.GetMethod("RebuildArray", BindingFlags.Static | BindingFlags.NonPublic) },
            };

        private readonly ConcurrentDictionary<Type, Func<T, Children<T>>> _getters
            = new ConcurrentDictionary<Type, Func<T, Children<T>>>();
        private readonly ConcurrentDictionary<Type, Func<T, Children<T>, T>> _setters
            = new ConcurrentDictionary<Type, Func<T, Children<T>, T>>();

        private AutoRewriter() { }

        /// <inheritdoc/>
        public Children<T> GetChildren(T value)
            => _getters.GetOrAdd(value.GetType(), t => MkGetter(t))(value);

        /// <inheritdoc/>
        public T SetChildren(Children<T> newChildren, T oldValue)
            => _setters.GetOrAdd(oldValue.GetType(), t => MkSetter(t))(oldValue, newChildren);

        /// <inheritdoc/>
        public T RewriteChildren(Func<T, T> transformer, T oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="AutoRewriter{T}"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="AutoRewriter{T}"/>.</returns>
        public static AutoRewriter<T> Instance { get; } = new AutoRewriter<T>();


        private Func<T, Children<T>> MkGetter(Type nodeType)
        {
            var ctorParams = GetBestConstructor(nodeType)
                ?.GetParameters()
                ?? Enumerable.Empty<ParameterInfo>();  // if no public constructor, assume node has no children

            if (!ctorParams.Select(p => p.ParameterType).Any(ImplementsIEnumerableT))
            {
                return MkGetterWithIndividualChildren(nodeType, ctorParams);
            }
            return MkGetterWithEnumerables(nodeType, ctorParams);
        }

        private Func<T, Children<T>> MkGetterWithIndividualChildren(Type nodeType, IEnumerable<ParameterInfo> ctorParams)
        {
            var propNames = ctorParams
                .Where(p => p.ParameterType.Equals(_t))
                .Select(p => ParamNameToPropName(p.Name))
                .ToList();
            var numberOfChildren = propNames.Count;

            if (numberOfChildren == 0)
            {
                // no children
                return x => Children.None<T>();
            }

            var nodeLocal = Expression.Parameter(nodeType, "node");
            Expression children;
            if (numberOfChildren == 1)
            {
                // Children.One<T>(node.Child)
                var memberAccess = Expression.Property(nodeLocal, nodeType.GetProperty(propNames.Single()));
                children = Expression.Call(_childrenOne, memberAccess);
            }
            else if (numberOfChildren == 2)
            {
                // Children.Two<T>(node.Child1, node.Child2);
                var memberAccess1 = Expression.Property(nodeLocal, nodeType.GetProperty(propNames.First()));
                var memberAccess2 = Expression.Property(nodeLocal, nodeType.GetProperty(propNames.ElementAt(1)));
                children = Expression.Call(_childrenTwo, memberAccess1, memberAccess2);
            }
            else
            {
                // Children.Many<T>(new[] { node.Child1, node.Child2, node.Child3 });
                var props = propNames.Select(nodeType.GetProperty);
                var memberAccesses = props.Select(p => Expression.Property(nodeLocal, p));
                children = Expression.Call(_childrenMany, Expression.NewArrayInit(_t, memberAccesses));
            }
            
            // param =>
            // {
            //     NodeType node = (NodeType)param;
            //     return /* ... */;
            // }
            var param = Expression.Parameter(_t, "param");
            var body = Expression.Block(
                new[]{ nodeLocal },
                new Expression[]
                {
                    Expression.Assign(nodeLocal, Expression.Convert(param, nodeType)),
                    children
                }
            );

            var lam = Expression.Lambda<Func<T, Children<T>>>(body, $"GetChildren_{_t.Name}_{nodeType.Name}", new[] { param });
            return lam.Compile();
        }

        private Func<T, Children<T>> MkGetterWithEnumerables(Type nodeType, IEnumerable<ParameterInfo> ctorParams)
        {
            if (!ctorParams.Any(p => p.ParameterType.Equals(_t)) && ctorParams.Count(p => ImplementsIEnumerableT(p.ParameterType)) == 1)
            {
                // the ctor only contains a single IEnumerable<T>, we can just return it directly.

                var ctorParam = ctorParams.Single(p => ImplementsIEnumerableT(p.ParameterType));
                var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name));

                // param => Children.Many(((NodeType)param).Children)
                var param1 = Expression.Parameter(_t, "param");
                var body1 = Expression.Call(
                    _childrenMany,
                    Expression.Convert(Expression.Property(Expression.Convert(param1, nodeType), property), _iEnumerableT)
                );
                var lam1 = Expression.Lambda<Func<T, Children<T>>>(body1, param1);
                return lam1.Compile();
            }

            // param =>
            // {
            //     NodeType node = (NodeType) param;
            //     List<T> result = new List<T>();
            //     IEnumerator<T> enumerator;
            //
            //     // ...
            //
            //     return Children.Many(result);
            // }

            var param = Expression.Parameter(_t, "param");
            var nodeLocal = Expression.Parameter(nodeType, "node");
            var resultLocal = Expression.Parameter(_listT, "result");
            var enumeratorLocal = Expression.Parameter(_iEnumeratorT, "enumerator");
            var stmts = new List<Expression>
            {
                Expression.Assign(nodeLocal, Expression.Convert(param, nodeType)),
                Expression.Assign(resultLocal, Expression.New(_listT))
            };

            foreach (var ctorParam in ctorParams)
            {
                if (ctorParam.ParameterType.Equals(_t))
                {
                    // result.Add(node.Child1);
                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name));
                    var stmt = Expression.Call(resultLocal, _listTAdd, Expression.Property(nodeLocal, property));
                    stmts.Add(stmt);
                }
                else if (ImplementsIEnumerableT(ctorParam.ParameterType))
                {
                    // enumerator = node.Children.GetEnumerator();
                    // while(true)
                    // {
                    //     if (enumerator.MoveNext())
                    //     {
                    //         result.Add(enumerator.Current);
                    //     }
                    //     else
                    //     {
                    //         break;
                    //     }
                    // }

                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name));
                    var enumerable = Expression.Property(nodeLocal, property);
                    stmts.Add(Expression.Assign(enumeratorLocal, Expression.Call(enumerable, _iEnumerableGetEnumerator)));
                    
                    var breakLbl = Expression.Label();
                    var loopBody = Expression.IfThenElse(
                        Expression.Call(enumeratorLocal, _iEnumeratorMoveNext),
                        Expression.Call(resultLocal, _listTAdd, Expression.Property(enumeratorLocal, _iEnumeratorTCurrent)),
                        Expression.Goto(breakLbl)
                    );
                    stmts.Add(Expression.Loop(loopBody, breakLbl));
                }
                else
                {
                    // the property isn't a T or an IEnumerable<T>, skip it
                }
            }

            stmts.Add(Expression.Call(_childrenMany, resultLocal));

            var body = Expression.Block(new[]{ nodeLocal, resultLocal, enumeratorLocal }, stmts);
            var lam = Expression.Lambda<Func<T, Children<T>>>(body, $"GetChildren_{_t.Name}_{nodeType.Name}", new[] { param });
            return lam.Compile();
        }

        private Func<T, Children<T>, T> MkSetter(Type nodeType)
        {
            var ctor = GetBestConstructor(nodeType);
            var ctorParams = ctor?.GetParameters();
            var numberOfDirectChildren =
                ctorParams
                    ?.Count(p => p.ParameterType.Equals(_t))
                    ?? 0;

            if (ctor == null || ctorParams == null)
            {
                // if no public constructor, assume node has no children, so no rebuilding required
                return NoChildrenSetter();
            }

            var nodeLocal = Expression.Parameter(nodeType, "node");
            var retLocal = Expression.Parameter(nodeType, "ret");
            var childrenParam = Expression.Parameter(_childrenT, "children");

            Expression body;
            if (numberOfDirectChildren == 0)
            {
                return NoChildrenSetter();
            }
            else if (numberOfDirectChildren == 1)
            {
                // if (children.NumberOfChildren == NumberOfChildren.One)
                // {
                //     ret = new NodeType(node.Foo, children.First, node.Bar);
                // }
                // else
                // {
                //     throw new InvalidOperationException(/* ... */);
                // }
                var assertion = Expression.Equal(
                    Expression.Property(childrenParam, _childrenTNumberOfChildren),
                    Expression.Constant(NumberOfChildren.One)
                );

                var childrenFirst = Expression.Property(childrenParam, _childrenTFirst);
                body = Expression.IfThenElse(
                    assertion,
                    Expression.Assign(retLocal, CallNodeCtor(ctor, nodeLocal, new[]{ childrenFirst }, nodeType)),
                    ThrowNewInvalidOperationException(childrenParam, 1)
                );
            }
            else if (numberOfDirectChildren == 2)
            {
                // if (children.NumberOfChildren == NumberOfChildren.Two)
                // {
                //     ret = new NodeType(node.Foo, children.First, node.Bar, children.Second, node.Baz);
                // }
                // else
                // {
                //     throw new InvalidOperationException(/* ... */);
                // }
                var assertion = Expression.Equal(
                    Expression.Property(childrenParam, _childrenTNumberOfChildren),
                    Expression.Constant(NumberOfChildren.Two)
                );

                var childrenFirst = Expression.Property(childrenParam, _childrenTFirst);
                var childrenSecond = Expression.Property(childrenParam, _childrenTSecond);
                body = Expression.IfThenElse(
                    assertion,
                    Expression.Assign(retLocal, CallNodeCtor(ctor, nodeLocal, new[]{ childrenFirst, childrenSecond }, nodeType)),
                    ThrowNewInvalidOperationException(childrenParam, 1)
                );
            }
            else
            {
                // {
                //     IEnumerator<T> enumerator;
                //     T child0;
                //     T child1;
                //     // etc
                //
                //     enumerator = children.Many.GetEnumerator();
                //  
                //     if (enumerator.MoveNext())
                //     {
                //         child0 = enumerator.Current;
                //     }
                //     else
                //     {
                //         throw new InvalidOperationException(/* ... */);
                //     }
                //     // etc
                //
                //     if (enumerator.MoveNext())
                //     {
                //         throw new InvalidOperationException(/* ... */);
                //     }
                //
                //     ret = new NodeType(node.Foo, child0, child1, node.Bar, child2);
                // }
                var enumeratorLocal = Expression.Parameter(_iEnumeratorT, "enumerator");
                var childrenLocals = Enumerable
                    .Repeat(_t, numberOfDirectChildren)
                    .Select((type, i) => Expression.Parameter(type, $"child{i}"))
                    .ToList();

                var childrenManyExpr = Expression.Property(childrenParam, _childrenTMany);


                var stmts = new List<Expression>(numberOfDirectChildren + 3)
                {
                    Expression.Assign(enumeratorLocal, Expression.Call(childrenManyExpr, _iEnumerableGetEnumerator))
                };
                stmts.AddRange(
                    childrenLocals.Select(l =>
                        Expression.IfThenElse(
                            Expression.Call(enumeratorLocal, _iEnumeratorMoveNext),
                            Expression.Assign(l, Expression.Property(enumeratorLocal, _iEnumeratorTCurrent)),
                            ThrowNewInvalidOperationException(childrenParam, numberOfDirectChildren)
                        )
                    )
                );
                stmts.Add(Expression.IfThen(Expression.Call(enumeratorLocal, _iEnumeratorMoveNext), ThrowNewInvalidOperationException(childrenParam, numberOfDirectChildren)));
                stmts.Add(Expression.Assign(retLocal, CallNodeCtor(ctor, nodeLocal, childrenLocals.Cast<Expression>().ToList(), nodeType)));

                body = Expression.Block(new[]{ enumeratorLocal }.Concat(childrenLocals), stmts);
            }

            var nodeParam = Expression.Parameter(_t, "param");
            var block = Expression.Block(
                new[]{ nodeLocal, retLocal },
                new[]
                {
                    Expression.Assign(nodeLocal, Expression.Convert(nodeParam, nodeType)),
                    body,
                    retLocal
                }
            );

            var lam = Expression.Lambda<Func<T, Children<T>, T>>(block, $"SetChildren_{_t.Name}_{nodeType.Name}", new[] { nodeParam, childrenParam });
            return lam.Compile();
        }

        private static string ParamNameToPropName(string paramName)
            => char.ToUpper(paramName[0]) + paramName.Substring(1);

        private static ConstructorInfo GetBestConstructor(Type nodeType)
            => nodeType
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

        private static Func<T, Children<T>, T> NoChildrenSetter()
            => (node, children) =>
            {
                if (children.NumberOfChildren != NumberOfChildren.None)
                {
                    throw new InvalidOperationException($"Expected no children but got {children.Count()}");
                }
                return node;
            };

        private static Expression AccessNodeProperty(Expression expression, Type nodeType, ParameterInfo ctorParam)
            => Expression.Property(expression, nodeType.GetProperty(ParamNameToPropName(ctorParam.Name)));

        private static Expression ThrowNewInvalidOperationException(Expression childrenParam, int expected)
        {
            var count = Expression.Call(_enumerableCount, Expression.Convert(childrenParam, _iEnumerableT));
            var msg = Expression.Call(_stringConcat2, Expression.Constant($"Expected {expected} children but got "), Expression.Call(count, _objectToString));
            var exc = Expression.New(_newInvalidOperationException, msg);
            return Expression.Throw(exc);
        }

        private static Expression CallNodeCtor(ConstructorInfo ctor, ParameterExpression nodeParam, IList<Expression> childrenExprs, Type nodeType)
            => Expression.New(ctor, NodeCtorArgs(nodeParam, ctor.GetParameters(), childrenExprs, nodeType));

        private static IEnumerable<Expression> NodeCtorArgs(ParameterExpression nodeParam, IEnumerable<ParameterInfo> ctorParams, IList<Expression> childrenExprs, Type nodeType)
        {
            var numberOfTs = 0;
            var args = new List<Expression>(ctorParams.Count());
            foreach (var param in ctorParams)
            {
                if (param.ParameterType.Equals(_t))
                {
                    args.Add(childrenExprs[numberOfTs]);
                    numberOfTs++;
                }
                else
                {
                    args.Add(AccessNodeProperty(nodeParam, nodeType, param));
                }
            }
            return args;
        }

        /// <summary>
        /// does type implement <see cref="IEnumerable{T}"/>[T] (not <see cref="IEnumerable{U}"/>[U] for some subtype U of T)?
        /// </summary>
        private static bool ImplementsIEnumerableT(Type type)
            => _iEnumerableT.IsAssignableFrom(type) && GetIEnumerableArgument(type).Equals(_t);

        private static Type GetIEnumerableArgument(Type enumerableType)
            => GetSelfAndBaseTypes(enumerableType)
                .First(t => t.GetGenericTypeDefinition().Equals(_iEnumerable1))
                .GetGenericArguments()
                .Single();

        private static IEnumerable<Type> GetSelfAndBaseTypes(Type type)
        {
            yield return type;
            foreach (var i in type.GetInterfaces())
            {
                foreach (var bi in GetSelfAndBaseTypes(i))
                {
                    yield return i;
                }
            }
            if (type.BaseType != null)
            {
                foreach (var b in GetSelfAndBaseTypes(type.BaseType))
                {
                    yield return b;
                }
            }
        }

        private static ImmutableArray<T> RebuildImmutableArray(IEnumerable<T> oldValues, IEnumerator<T> newValues)
        {
            var builder = oldValues is ICollection<T> c
                ? ImmutableArray.CreateBuilder<T>(c.Count)
                : ImmutableArray.CreateBuilder<T>();
            foreach (var _ in oldValues)
            {
                var hasNext = newValues.MoveNext();
                if (!hasNext)
                {
                    throw new InvalidOperationException("Didn't get enough children");
                }
                builder.Add(newValues.Current);
            }
            return builder.ToImmutable();
        }

        private static ImmutableList<T> RebuildImmutableList(IEnumerable<T> oldValues, IEnumerator<T> newValues)
        {
            var builder = ImmutableList.CreateBuilder<T>();
            foreach (var _ in oldValues)
            {
                var hasNext = newValues.MoveNext();
                if (!hasNext)
                {
                    throw new InvalidOperationException("Didn't get enough children");
                }
                builder.Add(newValues.Current);
            }
            return builder.ToImmutable();
        }

        private static List<T> RebuildList(IEnumerable<T> oldValues, IEnumerator<T> newValues)
        {
            var list = oldValues is ICollection<T> c
                ? new List<T>(c.Count)
                : new List<T>();
            foreach (var _ in oldValues)
            {
                var hasNext = newValues.MoveNext();
                if (!hasNext)
                {
                    throw new InvalidOperationException("Didn't get enough children");
                }
                list.Add(newValues.Current);
            }
            return list;
        }

        private static T[] RebuildArray(IEnumerable<T> oldValues, IEnumerator<T> newValues)
        {
            var array = new T[oldValues.Count()];
            for (var i = 0; i <= array.Length; i++)
            {
                var hasNext = newValues.MoveNext();
                if (!hasNext)
                {
                    throw new InvalidOperationException("Didn't get enough children");
                }
                array[i] = newValues.Current;
            }
            return array;
        }
    }
}