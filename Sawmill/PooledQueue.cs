using System;
using System.Buffers;

namespace Sawmill
{
    internal struct PooledQueue<T>
    {
        private static readonly bool _needsClear =
            System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<T>();

        private T[] _array;
        private int _start;
        private int _end;

        public int Count => _end - _start;

        /// <summary>
        /// Make sure to discard the Span before the next AllocateRight call
        /// </summary>
        public Span<T> AllocateRight(int count)
        {
            GrowIfNecessary(count);

            var span = _array.AsSpan().Slice(_end, count);
            _end += count;
            return span;
        }

        public T PopLeft()
        {
            if (_start >= _end)
            {
                throw new InvalidOperationException("Tried to pop from empty queue. Please report this as a bug in Sawmill!");
            }
            var result = _array[_start];
            _start++;
            return result;
        }

        public void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array, _needsClear);
                _start = _end = 0;
                _array = null!;
            }
        }

        private void GrowIfNecessary(int count)
        {
            if (_array == null)
            {
                _array = ArrayPool<T>.Shared.Rent(Math.Max(512, count));
            }

            if (count > _array.Length - _end)
            {
                if (count > _array.Length - Count)
                {
                    var newArray = ArrayPool<T>.Shared.Rent(Math.Max(Count + count, _array.Length * 2));
                    Array.Copy(_array, _start, newArray, 0, Count);
                    ArrayPool<T>.Shared.Return(_array, _needsClear);
                    _array = newArray;
                }
                else
                {
                    Array.Copy(_array, _start, _array, 0, Count);
                }
                _end = Count;
                _start = 0;
            }
        }
    }
}
