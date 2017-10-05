using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sawmill
{
    internal interface IEnumerableBuilder<in T, out TEnumerable> where TEnumerable : IEnumerable<T>
    {
        void Add(T item);
        TEnumerable Build();
    }
    internal static class EnumerableBuilder<T>
    {
        private static readonly ImmutableArray<Type> _canHandleTypes = ImmutableArray.CreateRange(
            new[]
            {
                typeof(T[]),
                typeof(List<T>),
                typeof(ImmutableList<T>),
                typeof(ImmutableArray<T>)
            }
        );

        private static Delegate _castImmutableArray;
        private static readonly object _castImmutableArrayLock = new object();

        internal static bool CanHandle<TEnumerable>() where TEnumerable : IEnumerable<T>
            => CanHandle(typeof(TEnumerable));
        internal static bool CanHandle(Type enumerable)
            => _canHandleTypes.Contains(enumerable);

        internal static (bool changed, TEnumerable result)? Map<TEnumerable>(TEnumerable enumerable, Func<T, T> func) where TEnumerable : IEnumerable<T>
        {
            var changed = false;

            if (enumerable is T[])
            {
                var oldArray = (T[])(object)enumerable;
                var newArray = new T[oldArray.Length];

                for(var i = 0; i < oldArray.Length; i++)
                {
                    var oldItem = oldArray[i];
                    var newItem = func(oldItem);

                    changed = changed || !ReferenceEquals(oldItem, newItem);
                    newArray[i] = newItem;
                }

                return (changed, (TEnumerable)(object)newArray);
            }
            if (enumerable is List<T>)
            {
                var oldList = (List<T>)(object)enumerable;
                var newList = new List<T>(oldList.Count);

                foreach (var oldItem in oldList)
                {
                    var newItem = func(oldItem);

                    changed = changed || !ReferenceEquals(oldItem, newItem);
                    newList.Add(newItem);
                }

                return (changed, (TEnumerable)(object)newList);
            }
            if (enumerable is ImmutableList<T>)
            {
                var builder = ((ImmutableList<T>)(object)enumerable).ToBuilder();

                for (var i = 0; i < builder.Count; i++)
                {
                    var oldItem = builder[i];
                    var newItem = func(oldItem);

                    if (!ReferenceEquals(oldItem, newItem))
                    {
                        changed = true;
                        builder[i] = newItem;
                    }
                }
            
                return (changed, (TEnumerable)(object)builder.ToImmutable());
            }
            if (enumerable is ImmutableArray<T>)
            {
                if (_castImmutableArray == null)
                {
                    lock (_castImmutableArrayLock)
                    {
                        if (_castImmutableArray == null)
                        {
                            var param = Expression.Parameter(typeof(TEnumerable));
                            _castImmutableArray = Expression.Lambda<Func<TEnumerable, ImmutableArray<T>>>(param, param).Compile();
                        }
                    }
                }

                var builder = ((Func<TEnumerable, ImmutableArray<T>>)_castImmutableArray)(enumerable).ToBuilder();

                for (var i = 0; i < builder.Count; i++)
                {
                    var oldItem = builder[i];
                    var newItem = func(oldItem);

                    if (!ReferenceEquals(oldItem, newItem))
                    {
                        changed = true;
                        builder[i] = newItem;
                    }
                }
            
                return (changed, (TEnumerable)(object)builder.MoveToImmutable());
            }

            return null;
        }

        internal static (bool changed, TEnumerable result)? RebuildFrom<TEnumerable>(TEnumerable enumerable, IEnumerator<T> enumerator)
            where TEnumerable : IEnumerable<T>
        {
            var changed = false;

            if (enumerable is T[])
            {
                var oldArray = (T[])(object)enumerable;
                var newArray = new T[oldArray.Length];

                for(var i = 0; i < oldArray.Length; i++)
                {
                    var oldItem = oldArray[i];
                    var newItem = enumerator.Draw1();

                    changed = changed || !ReferenceEquals(oldItem, newItem);
                    newArray[i] = newItem;
                }
                
                return (changed, (TEnumerable)(object)newArray);
            }
            if (enumerable is List<T>)
            {
                var oldList = (List<T>)(object)enumerable;
                var newList = new List<T>(oldList.Count);

                foreach (var oldItem in oldList)
                {
                    var newItem = enumerator.Draw1();

                    changed = changed || !ReferenceEquals(oldItem, newItem);
                    newList.Add(newItem);
                }

                return (changed, (TEnumerable)(object)newList);
            }
            if (enumerable is ImmutableList<T>)
            {
                var builder = ((ImmutableList<T>)(object)enumerable).ToBuilder();

                for (var i = 0; i < builder.Count; i++)
                {
                    var oldItem = builder[i];
                    var newItem = enumerator.Draw1();

                    if (!ReferenceEquals(oldItem, newItem))
                    {
                        changed = true;
                        builder[i] = newItem;
                    }
                }
            
                return (changed, (TEnumerable)(object)builder.ToImmutable());
            }
            if (enumerable is ImmutableArray<T>)
            {
                if (_castImmutableArray == null)
                {
                    lock (_castImmutableArrayLock)
                    {
                        if (_castImmutableArray == null)
                        {
                            var param = Expression.Parameter(typeof(TEnumerable));
                            _castImmutableArray = Expression.Lambda<Func<TEnumerable, ImmutableArray<T>>>(param, param).Compile();
                        }
                    }
                }

                var builder = ((Func<TEnumerable, ImmutableArray<T>>)_castImmutableArray)(enumerable).ToBuilder();

                for (var i = 0; i < builder.Count; i++)
                {
                    var oldItem = builder[i];
                    var newItem = enumerator.Draw1();

                    if (!ReferenceEquals(oldItem, newItem))
                    {
                        changed = true;
                        builder[i] = newItem;
                    }
                }
            
                return (changed, (TEnumerable)(object)builder.MoveToImmutable());
            }

            return null;
        }
    }
}