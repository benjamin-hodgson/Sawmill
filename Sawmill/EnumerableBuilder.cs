using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sawmill
{
    internal interface IEnumerableBuilder<in T, out TEnumerable> where TEnumerable : IEnumerable<T>
    {
        void Add(T item);
        TEnumerable Build();
    }
    internal static class EnumerableBuilder<T>
    {
        private static readonly Type _iEnumerableT = typeof(IEnumerable<T>);
        private static readonly Type _tArray = typeof(T[]);
        private static readonly Type _listT = typeof(List<T>);
        private static readonly Type _immutableListT = typeof(ImmutableList<T>);
        private static readonly Type _immutableArrayT = typeof(ImmutableArray<T>);


        internal static IEnumerableBuilder<T, TEnumerable> Create<TEnumerable>() where TEnumerable : IEnumerable<T>
        {
            var tEnumerable = typeof(TEnumerable);

            if (tEnumerable.Equals(_iEnumerableT))
            {
                return (IEnumerableBuilder<T, TEnumerable>)((object)new ImmutableArrayBuilder());
            }
            if (tEnumerable.Equals(_tArray))
            {
                return (IEnumerableBuilder<T, TEnumerable>)((object)new ArrayBuilder());
            }
            if (tEnumerable.Equals(_listT))
            {
                return (IEnumerableBuilder<T, TEnumerable>)((object)new ListBuilder());
            }
            if (tEnumerable.Equals(_immutableListT))
            {
                return (IEnumerableBuilder<T, TEnumerable>)((object)new ImmutableListBuilder());
            }
            if (tEnumerable.Equals(_immutableArrayT))
            {
                return (IEnumerableBuilder<T, TEnumerable>)((object)new ImmutableArrayBuilder());
            }
            
            throw new Exception();
        }

        private sealed class ListBuilder : IEnumerableBuilder<T, List<T>>
        {
            private readonly List<T> _list = new List<T>();

            public void Add(T item) => _list.Add(item);

            public List<T> Build() => _list;
        }
        private sealed class ArrayBuilder : IEnumerableBuilder<T, T[]>
        {
            private readonly List<T> _list = new List<T>();

            public void Add(T item) => _list.Add(item);

            public T[] Build() => _list.ToArray();
        }
        private sealed class ImmutableArrayBuilder : IEnumerableBuilder<T, ImmutableArray<T>>
        {
            private readonly ImmutableArray<T>.Builder _builder = ImmutableArray.CreateBuilder<T>();

            public void Add(T item) => _builder.Add(item);

            public ImmutableArray<T> Build()
            {
                if (_builder.Capacity == _builder.Count)
                {
                    return _builder.MoveToImmutable();
                }
                return _builder.ToImmutable();
            }
        }
        private sealed class ImmutableListBuilder : IEnumerableBuilder<T, ImmutableList<T>>
        {
            private readonly ImmutableList<T>.Builder _builder = ImmutableList.CreateBuilder<T>();

            public void Add(T item) => _builder.Add(item);

            public ImmutableList<T> Build() => _builder.ToImmutable();
        }
    }
}