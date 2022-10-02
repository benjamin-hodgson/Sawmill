using System;
using System.Buffers;

namespace Sawmill;

internal struct ChunkStack<T>
{
    private static readonly bool _needsClear =
        System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<T>();

    private Region _topRegion;
    private Region[]? _regions;
    private int _regionCount;

    public bool IsEmpty => _topRegion.IsUnused;

    public Span<T> Allocate(int size)
    {
        if (size == 0)
        {
            return new Span<T>();
        }
        if (_topRegion.IsDefault || !_topRegion.HasSpace(size))
        {
            AllocateRegion(size);
        }
        return _topRegion.Allocate(size);
    }

    public Memory<T> AllocateMemory(int size)
    {
        if (size == 0)
        {
            return new Memory<T>();
        }
        if (_topRegion.IsDefault || !_topRegion.HasSpace(size))
        {
            AllocateRegion(size);
        }
        return _topRegion.AllocateMemory(size);
    }

    public void Free(Memory<T> memory)
    {
        Free(memory.Span);
    }
    public void Free(Span<T> span)
    {
        if (span.Length == 0)
        {
            return;
        }
        if (!_topRegion.IsDefault && _topRegion.IsUnused)
        {
            FreeTopRegion();
        }
        _topRegion.Free(span);
    }

    public T Pop()
    {
        if (!_topRegion.IsDefault && _topRegion.IsUnused)
        {
            FreeTopRegion();
        }
        return _topRegion.Pop();
    }

    public void Dispose()
    {
        if (_regions != null)
        {
            for (var i = 0; i < _regionCount; i++)
            {
                _regions[i].Dispose();
                _regions[i] = default;
            }
            _regions = null;
        }
    }

    private void AllocateRegion(int size)
    {
        if (_topRegion.IsDefault)
        {
            _topRegion = new Region(Math.Max(512, size));
            return;
        }

        if (_regions == null)
        {
            _regions = new Region[8];
        }

        if (_regions.Length == _regionCount)
        {
            var newRegions = new Region[_regionCount * 2];
            Array.Copy(_regions, newRegions, _regionCount);
            _regions = newRegions;
        }

        _regions[_regionCount] = _topRegion;
        _regionCount++;
        _topRegion = new Region(Math.Max(512, size));
    }

    private void FreeTopRegion()
    {
        _topRegion.Dispose();
        if (_regionCount > 0)
        {
            _regionCount--;
            _topRegion = _regions![_regionCount];
            _regions[_regionCount] = default;
        }
        else
        {
            _topRegion = default;
        }
    }

    private struct Region
    {
        private T[]? _array;
        private int _used;

        public bool IsDefault => _array == null;
        public bool IsUnused => _used == 0;

        public Region(int size)
        {
            _array = ArrayPool<T>.Shared.Rent(size);
            _used = 0;
        }

        public bool HasSpace(int size)
            => _array!.Length - _used >= size;

        public Span<T> Allocate(int size)
        {
            var memory = _array.AsSpan().Slice(_used, size);
            _used += size;
            return memory;
        }

        public Memory<T> AllocateMemory(int size)
        {
            var memory = _array.AsMemory().Slice(_used, size);
            _used += size;
            return memory;
        }

        public void Free(Span<T> span)
        {
            if (_used < span.Length)
            {
                throw new InvalidOperationException("Chunk was freed in the wrong order. Please report this as a bug in Sawmill!");
            }
            _used -= span.Length;
        }

        public T Pop()
        {
            if (_used == 0)
            {
                throw new InvalidOperationException("Chunk was freed in the wrong order. Please report this as a bug in Sawmill!");
            }
            _used--;
            return _array![_used];
        }

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(_array!, _needsClear);
            _array = null;
        }
    }
}
