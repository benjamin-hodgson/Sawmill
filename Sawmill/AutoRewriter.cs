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
    /// 
    /// <see cref="AutoRewriter{T}"/> looks for the subtype's constructor, and gets/sets
    /// the <typeparamref name="T"/>-children in the order that they appear in the constructor.
    /// </summary>
    public class AutoRewriter<T> : IRewriter<T>
    {
        private static readonly Type _t = typeof(T);

        private static readonly Type _spanT = typeof(Span<T>);
        private static readonly PropertyInfo _spanT_Indexer = _spanT.GetProperty("Item")!;
        private static readonly Type _readOnlySpanT = typeof(ReadOnlySpan<T>);
        private static readonly MethodInfo _readOnlySpanT_Slice = _readOnlySpanT
            .GetMethods()
            .Single(m => m.Name == "Slice" && m.GetParameters().Length == 1);

        private static readonly Type _iEnumerator = typeof(IEnumerator);
        private static readonly MethodInfo _iEnumerator_MoveNext =
            _iEnumerator.GetMethod("MoveNext")!;

        private static readonly Type _iEnumeratorT = typeof(IEnumerator<T>);
        private static readonly PropertyInfo _iEnumeratorT_Current =
            _iEnumeratorT.GetProperty("Current", _t)!;

        private static readonly Type _iEnumerable1 = typeof(IEnumerable<>);
        private static readonly Type _iEnumerableT = typeof(IEnumerable<T>);
        private static readonly MethodInfo _iEnumerable_GetEnumerator =
            typeof(IEnumerable<T>)
                .GetMethods()
                .Single(m => m.Name == "GetEnumerator" && m.ReturnType.Equals(_iEnumeratorT));
        private static readonly MethodInfo _enumerable_Count =
            typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == "Count" && m.GetParameters().Length == 1)
                .MakeGenericMethod(_t);

        private static readonly Type _int = typeof(int);

        private static readonly Type _autoRewriterT = typeof(AutoRewriter<T>);
        private static readonly IReadOnlyDictionary<Type, MethodInfo> _enumerableRebuilders
            = new Dictionary<Type, MethodInfo>
            {
                { typeof(IEnumerable<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(IList<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(IReadOnlyList<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(ICollection<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(IReadOnlyCollection<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },

                { typeof(ImmutableArray<T>), _autoRewriterT.GetMethod("RebuildImmutableArray", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(ImmutableList<T>), _autoRewriterT.GetMethod("RebuildImmutableList", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(List<T>), _autoRewriterT.GetMethod("RebuildList", BindingFlags.Static | BindingFlags.NonPublic)! },
                { typeof(T[]), _autoRewriterT.GetMethod("RebuildArray", BindingFlags.Static | BindingFlags.NonPublic)! },
            };
        private static readonly MethodInfo _getSpanElement =
            _autoRewriterT.GetMethod("GetSpanElement", BindingFlags.Static | BindingFlags.NonPublic)!;
        private static readonly MethodInfo _getReadOnlySpanElement =
            _autoRewriterT.GetMethod("GetReadOnlySpanElement", BindingFlags.Static | BindingFlags.NonPublic)!;
        private static readonly MethodInfo _assignSpanElement =
            _autoRewriterT.GetMethod("AssignSpanElement", BindingFlags.Static | BindingFlags.NonPublic)!;

        private readonly ConcurrentDictionary<Type, Func<T, int>> _counters
            = new ConcurrentDictionary<Type, Func<T, int>>();
        private readonly ConcurrentDictionary<Type, SpanAction<T, T>> _getters
            = new ConcurrentDictionary<Type, SpanAction<T, T>>();
        private readonly ConcurrentDictionary<Type, ReadOnlySpanFunc<T, T, T>> _setters
            = new ConcurrentDictionary<Type, ReadOnlySpanFunc<T, T, T>>();

        /// <summary>
        /// Create a new instance of <see cref="AutoRewriter{T}"/>
        /// </summary>
        protected AutoRewriter() { }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return _counters.GetOrAdd(value.GetType(), t => MkCounter(t))(value);
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<T> children, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _getters.GetOrAdd(value.GetType(), t => MkGetter(t))(children, value);
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public T SetChildren(ReadOnlySpan<T> newChildren, T oldValue)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            return _setters.GetOrAdd(oldValue.GetType(), t => MkSetter(t))(newChildren, oldValue);
        }

        /// <summary>
        /// Gets the single global instance of <see cref="AutoRewriter{T}"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="AutoRewriter{T}"/>.</returns>
        public static AutoRewriter<T> Instance { get; } = new AutoRewriter<T>();


        private Func<T, int> MkCounter(Type nodeType)
        {
            var ctorParams = GetBestConstructor(nodeType)
                ?.GetParameters()
                ?? Enumerable.Empty<ParameterInfo>();  // if no public constructor, assume node has no children

            // (children, param) =>
            // {
            //     NodeType node = (NodeType)param;
            //     int count = 0;
            //     IEnumerator<T> enumerator;
            //
            //     // ...
            // 
            // }

            var nodeParam = Expression.Parameter(_t, "param");
            var nodeLocal = Expression.Parameter(nodeType, "node");
            var countLocal = Expression.Parameter(_int, "count");
            var stmts = new List<Expression>
            {
                Expression.Assign(nodeLocal, Expression.Convert(nodeParam, nodeType)),
            };

            foreach (var ctorParam in ctorParams)
            {
                if (ctorParam.ParameterType.Equals(_t))
                {
                    // i++;
                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name!));
                    stmts.Add(Expression.Assign(countLocal, Expression.Increment(countLocal)));
                }
                else if (ImplementsIEnumerableT(ctorParam.ParameterType))
                {
                    // i += Enumerable.Count(node.Children);

                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name!));
                    var enumerable = Expression.Property(nodeLocal, property);
                    stmts.Add(Expression.AddAssign(countLocal, Expression.Call(_enumerable_Count, enumerable)));
                }
                else
                {
                    // the property isn't a T or an IEnumerable<T>, skip it
                }
            }

            stmts.Add(countLocal);

            var body = Expression.Block(new[]{ nodeLocal, countLocal }, stmts);
            var lam = Expression.Lambda<Func<T, int>>(body, $"CountChildren_{_t.Name}_{nodeType.Name}", new[] { nodeParam });
            return lam.Compile();
        }

        private SpanAction<T, T> MkGetter(Type nodeType)
        {
            var ctorParams = GetBestConstructor(nodeType)
                ?.GetParameters()
                ?? Enumerable.Empty<ParameterInfo>();  // if no public constructor, assume node has no children

            // (children, param) =>
            // {
            //     NodeType node = (NodeType)param;
            //     int i = 0;
            //     IEnumerator<T> enumerator;
            //
            //     // ...
            // 
            // }

            var childrenParam = Expression.Parameter(_spanT, "children");
            var nodeParam = Expression.Parameter(_t, "param");
            var nodeLocal = Expression.Parameter(nodeType, "node");
            var indexLocal = Expression.Parameter(_int, "i");
            var enumeratorLocal = Expression.Parameter(_iEnumeratorT, "enumerator");
            var stmts = new List<Expression>
            {
                Expression.Assign(nodeLocal, Expression.Convert(nodeParam, nodeType)),
            };

            foreach (var ctorParam in ctorParams)
            {
                if (ctorParam.ParameterType.Equals(_t))
                {
                    // children[i] = node.Child;
                    // i++;
                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name!));
                    stmts.Add(Expression.Call(_assignSpanElement, childrenParam, indexLocal, Expression.Property(nodeLocal, property)));
                    stmts.Add(Expression.Assign(indexLocal, Expression.Increment(indexLocal)));
                }
                else if (ImplementsIEnumerableT(ctorParam.ParameterType))
                {
                    // enumerator = node.Children.GetEnumerator();
                    // while(true)
                    // {
                    //     if (enumerator.MoveNext())
                    //     {
                    //         children[i] = enumerator.Current;
                    //         i++;
                    //     }
                    //     else
                    //     {
                    //         break;
                    //     }
                    // }

                    var property = nodeType.GetProperty(ParamNameToPropName(ctorParam.Name!));
                    var enumerable = Expression.Property(nodeLocal, property);
                    stmts.Add(Expression.Assign(enumeratorLocal, Expression.Call(enumerable, _iEnumerable_GetEnumerator)));
                    
                    var breakLbl = Expression.Label();
                    var loopBody = Expression.IfThenElse(
                        Expression.Call(enumeratorLocal, _iEnumerator_MoveNext),
                        Expression.Block(
                            Expression.Call(_assignSpanElement, childrenParam, indexLocal, Expression.Property(enumeratorLocal, _iEnumeratorT_Current)),
                            Expression.Assign(indexLocal, Expression.Increment(indexLocal))
                        ),
                        Expression.Goto(breakLbl)
                    );
                    stmts.Add(Expression.Loop(loopBody, breakLbl));
                }
                else
                {
                    // the property isn't a T or an IEnumerable<T>, skip it
                }
            }

            var body = Expression.Block(new[]{ nodeLocal, indexLocal, enumeratorLocal }, stmts);
            var lam = Expression.Lambda<SpanAction<T, T>>(body, $"GetChildren_{_t.Name}_{nodeType.Name}", new[] { childrenParam, nodeParam });
            return lam.Compile();
        }

        private ReadOnlySpanFunc<T, T, T> MkSetter(Type nodeType)
        {
            var ctor = GetBestConstructor(nodeType);
            var ctorParams = ctor?.GetParameters();

            if (ctorParams == null)
            {
                // if no public constructor, assume node has no children, so no rebuilding required
                return (children, x) => x;
            }

            var numberOfDirectChildren = ctorParams.Count(p => p.ParameterType.Equals(_t));
            var enumerablesOfChildren = ctorParams
                .Where(p => ImplementsIEnumerableT(p.ParameterType))
                .Select(p => p.ParameterType)
                .ToList();

            var childrenParam = Expression.Parameter(_readOnlySpanT, "children");
            var nodeParam = Expression.Parameter(_t, "param");
            var nodeLocal = Expression.Parameter(nodeType, "node");
            var retLocal = Expression.Parameter(nodeType, "ret");


            // (children, param) =>
            // {
            //     NodeType param = (NodeType)param;
            //     T ret;
            // 
            //     T child0;
            //     T[] child1;
            //     // etc
            //
            //  
            //     child0 = children[0];
            //     children = children.Slice(1);
            //     
            //     child1 = AutoRewriter.RebuildArray(node.Children1, children);
            //     children = children.Slice(Enumerable.Count(child1));
            //     // etc
            //
            //     ret = new NodeType(node.Foo, child0, child1, node.Bar, child2);
            //     return ret;
            // }
            var childrenInfos = ctorParams
                .Where(p => p.ParameterType.Equals(_t) || ImplementsIEnumerableT(p.ParameterType))
                .Select((param, i) => (local: Expression.Parameter(param.ParameterType, $"child{i}"), param))
                .ToList();
            var childrenLocals = childrenInfos.Select(x => x.local).ToList();

            var stmts = new List<Expression>
            {
                Expression.Assign(nodeLocal, Expression.Convert(nodeParam, nodeType)),
            };

            stmts.AddRange(
                childrenInfos.SelectMany(x =>
                    x.param.ParameterType.Equals(_t)
                        ? new Expression[]
                        {
                            Expression.Assign(x.local, Expression.Call(_getReadOnlySpanElement, childrenParam, Expression.Constant(0))),
                            Expression.Assign(childrenParam, Expression.Call(childrenParam, _readOnlySpanT_Slice, Expression.Constant(1)))
                        }
                        : new Expression[]
                        {
                            Expression.Assign(
                                x.local,
                                Expression.Call(
                                    _enumerableRebuilders[x.param.ParameterType],
                                    Expression.Property(nodeLocal, nodeType.GetProperty(ParamNameToPropName(x.param.Name!))),
                                    childrenParam
                                )
                            ),
                            Expression.Assign(childrenParam, Expression.Call(childrenParam, _readOnlySpanT_Slice, Expression.Call(_enumerable_Count, x.local)))
                        }
                )
            );
            stmts.Add(Expression.Assign(retLocal, CallNodeCtor(ctor!, nodeLocal, childrenLocals.Cast<Expression>().ToList(), nodeType)));
            stmts.Add(retLocal);

            var block = Expression.Block(
                new[]{ nodeLocal, retLocal }.Concat(childrenLocals),
                stmts
            );

            var lam = Expression.Lambda<ReadOnlySpanFunc<T, T, T>>(block, $"SetChildren_{_t.Name}_{nodeType.Name}", new[] { childrenParam, nodeParam });
            return lam.Compile();
        }

        private static string ParamNameToPropName(string paramName)
            => char.ToUpper(paramName[0]) + paramName.Substring(1);

        private static ConstructorInfo GetBestConstructor(Type nodeType)
            => nodeType
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

        private static Expression AccessNodeProperty(Expression expression, Type nodeType, ParameterInfo ctorParam)
            => Expression.Property(expression, nodeType.GetProperty(ParamNameToPropName(ctorParam.Name!)));

        private static Expression CallNodeCtor(ConstructorInfo ctor, ParameterExpression nodeParam, IList<Expression> childrenExprs, Type nodeType)
            => Expression.New(ctor, NodeCtorArgs(nodeParam, ctor.GetParameters(), childrenExprs, nodeType));

        private static IEnumerable<Expression> NodeCtorArgs(ParameterExpression nodeParam, IEnumerable<ParameterInfo> ctorParams, IList<Expression> childrenExprs, Type nodeType)
        {
            var numberOfTs = 0;
            var args = new List<Expression>(ctorParams.Count());
            foreach (var param in ctorParams)
            {
                if (param.ParameterType.Equals(_t) || ImplementsIEnumerableT(param.ParameterType))
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


        private static T GetSpanElement(Span<T> span, int index) => span[index];
        private static void AssignSpanElement(Span<T> span, int index, T value)
        {
            span[index] = value;
        }
        private static T GetReadOnlySpanElement(ReadOnlySpan<T> span, int index) => span[index];


        private static ImmutableArray<T> RebuildImmutableArray(IEnumerable<T> oldValues, ReadOnlySpan<T> newValues)
        {
            var builder = oldValues is ICollection<T> c
                ? ImmutableArray.CreateBuilder<T>(c.Count)
                : ImmutableArray.CreateBuilder<T>();
            var i = 0;
            foreach (var _ in oldValues)
            {
                builder.Add(newValues[i]);
                i++;
            }
            return builder.ToImmutableAndClear();
        }

        private static ImmutableList<T> RebuildImmutableList(IEnumerable<T> oldValues, ReadOnlySpan<T> newValues)
        {
            var builder = ImmutableList.CreateBuilder<T>();
            var i = 0;
            foreach (var _ in oldValues)
            {
                builder.Add(newValues[i]);
                i++;
            }
            return builder.ToImmutable();
        }

        private static List<T> RebuildList(IEnumerable<T> oldValues, ReadOnlySpan<T> newValues)
        {
            var list = oldValues is ICollection<T> c
                ? new List<T>(c.Count)
                : new List<T>();
            var i = 0;
            foreach (var _ in oldValues)
            {
                list.Add(newValues[i]);
                i++;
            }
            return list;
        }

        private static T[] RebuildArray(IEnumerable<T> oldValues, ReadOnlySpan<T> newValues)
        {
            var array = new T[oldValues.Count()];
            for (var i = 0; i <= array.Length; i++)
            {
                array[i] = newValues[i];
            }
            return array;
        }
    }
}
