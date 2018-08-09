using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sawmill
{
    /// <summary>
    /// Factory methods for <see cref="Children{T}"/>.
    /// </summary>
    public static class Children
    {
        /// <summary>
        /// Creates a <see cref="Children{T}"/> with no elements.
        /// </summary>
        /// <returns>A <see cref="Children{T}"/> with no elements.</returns>
        public static Children<T> None<T>()
            => new Children<T>();

        /// <summary>
        /// Creates a <see cref="Children{T}"/> with a single element.
        /// </summary>
        /// <returns>A <see cref="Children{T}"/> with a single element.</returns>
        public static Children<T> One<T>(T child)
            => new Children<T>(NumberOfChildren.One, child, default(T), null);

        /// <summary>
        /// Creates a <see cref="Children{T}"/> with two elements.
        /// </summary>
        /// <returns>A <see cref="Children{T}"/> with two elements.</returns>
        public static Children<T> Two<T>(T first, T second)
            => new Children<T>(NumberOfChildren.Two, first, second, null);

        /// <summary>
        /// Creates a <see cref="Children{T}"/> with any number of elements.
        /// </summary>
        /// <returns>A <see cref="Children{T}"/> with any number of elements.</returns>
        public static Children<T> Many<T>(ImmutableList<T> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }
            return new Children<T>(NumberOfChildren.Many, default(T), default(T), children);
        }
    }

    /// <summary>
    /// An enumerable type representing the children of a node of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// Why not just use <see cref="IEnumerable{T}"/>? <see cref="Children{T}"/> is a value type,
    /// whereas <see cref="IEnumerable{T}"/> is a reference type.
    /// In cases where a node has a small, fixed number of children (fewer than three),
    /// it's much more efficient to pass those children around on the stack,
    /// rather than storing an enumerable on the heap which will quickly become garbage.
    /// </remarks>
    /// <typeparam name="T">The type of the rewritable object.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct Children<T> : IImmutableList<T>
    {
        /// <summary>
        /// The number of children the instance contains.
        /// </summary>
        /// <returns>The number of children the instance contains.</returns>
        public NumberOfChildren NumberOfChildren { get; }

        private readonly T _first;
        /// <summary>
        /// Gets the first element, if <see cref="Children{T}.NumberOfChildren"/> is
        /// <see cref="NumberOfChildren.One"/> or <see cref="NumberOfChildren.Two"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="Children{T}.NumberOfChildren"/> is not
        /// <see cref="NumberOfChildren.One"/> or <see cref="NumberOfChildren.Two"/>
        /// </exception>
        /// <returns>
        /// The first element, if <see cref="Children{T}.NumberOfChildren"/> is
        /// <see cref="NumberOfChildren.One"/> or <see cref="NumberOfChildren.Two"/>.
        /// </returns>
        public T First
        {
            get
            {
                if (NumberOfChildren != NumberOfChildren.One && NumberOfChildren != NumberOfChildren.Two)
                {
                    throw new InvalidOperationException("Wrong number of children for First");
                }
                return _first;
            }
        }
        
        private readonly T _second;
        /// <summary>
        /// Gets the second element, if <see cref="Children{T}.NumberOfChildren"/> is <see cref="NumberOfChildren.Two"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="Children{T}.NumberOfChildren"/> is not <see cref="NumberOfChildren.Two"/>
        /// </exception>
        /// <returns>
        /// The second element, if <see cref="Children{T}.NumberOfChildren"/> is <see cref="NumberOfChildren.Two"/>.
        /// </returns>
        public T Second
        {
            get
            {
                if (NumberOfChildren != NumberOfChildren.Two)
                {
                    throw new InvalidOperationException("Wrong number of children for Second");
                }
                return _second;
            }
        }

        private readonly ImmutableList<T> _many;
        /// <summary>
        /// Gets an enumerable containing the elements, if <see cref="Children{T}.NumberOfChildren"/> is <see cref="NumberOfChildren.Many"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="Children{T}.NumberOfChildren"/> is not <see cref="NumberOfChildren.Many"/>
        /// </exception>
        /// <returns>
        /// An enumerable containing the elements, if if <see cref="Children{T}.NumberOfChildren"/> is <see cref="NumberOfChildren.Many"/>.
        /// </returns>
        public ImmutableList<T> Many
        {
            get
            {
                if (NumberOfChildren != NumberOfChildren.Many)
                {
                    throw new InvalidOperationException("Wrong number of children for Many");
                }
                return _many;
            }
        }

        /// <summary>
        /// Gets the number of values in the <see cref="Children{T}"/>.
        /// </summary>
        /// <value>The number of values in the <see cref="Children{T}"/>.</value>
        public int Count
        {
            get
            {
                switch (NumberOfChildren)
                {
                    case NumberOfChildren.None:
                        return 0;
                    case NumberOfChildren.One:
                        return 1;
                    case NumberOfChildren.Two:
                        return 2;
                    case NumberOfChildren.Many:
                        return _many.Count;
                }
                throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
            }
        }

        int IReadOnlyCollection<T>.Count => Count;


        /// <summary>
        /// Returns the element of the <see cref="Children{T}"/> at index <paramref name="index"/>.
        /// </summary>
        /// <value>The element of the <see cref="Children{T}"/> at index <paramref name="index"/>.</value>
        public T this[int index]
        {
            get
            {
                switch (NumberOfChildren)
                {
                    case NumberOfChildren.One when index == 0:
                    case NumberOfChildren.Two when index == 0:
                        return _first;
                    case NumberOfChildren.Two when index == 1:
                        return _second;
                    case NumberOfChildren.Many:
                        return _many[index];
                }
                throw new IndexOutOfRangeException();
            }
        }
        T IReadOnlyList<T>.this[int index] => this[index];

        internal Children(NumberOfChildren numberOfChildren, T first, T second, ImmutableList<T> many)
        {
            NumberOfChildren = numberOfChildren;
            _first = first;
            _second = second;
            _many = many;
        }

        /// <summary>
        /// Returns a new <see cref="Children{T}"/> containing the result of applying a transformation function to the elements of the current instance.
        /// </summary>
        /// <param name="func">A transformation function to apply to the elements of the current instance</param>
        /// <exception name="ArgumentNullException"><paramref name="func"/> is null.</exception>
        /// <returns>A new <see cref="Children{T}"/> containing the result of applying a transformation function to the elements of the current instance.</returns>
        public Children<U> Select<U>(Func<T, U> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            switch (NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return Children.None<U>();
                case NumberOfChildren.One:
                    return Children.One(func(_first));
                case NumberOfChildren.Two:
                    return Children.Two(func(_first), func(_second));
                case NumberOfChildren.Many:
                    return Children.Many(_many.ConvertAll(func));
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }

        /// <summary>
        /// Returns an <see cref="ImmutableList{T}"/> containing the elements of this <see cref="Children{T}"/>.
        /// </summary>
        /// <returns>An <see cref="ImmutableList{T}"/> containing the elements of this <see cref="Children{T}"/>.</returns>
        public ImmutableList<T> ToImmutableList()
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return ImmutableList<T>.Empty;
                case NumberOfChildren.One:
                    return ImmutableList.Create(_first);
                case NumberOfChildren.Two:
                    return ImmutableList.Create(_first, _second);
                case NumberOfChildren.Many:
                    return _many;
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }

        /// <summary>
        /// Equivalent to <c>Children.One(item)</c>
        /// </summary>
        /// <param name="item">The child</param>
        public static implicit operator Children<T>(T item)
            => Children.One(item);

        /// <summary>
        /// Equivalent to <c>Children.Two(children.Item1, children.Item2)</c>
        /// </summary>
        /// <param name="children">The children</param>
        public static implicit operator Children<T>((T, T) children)
            => Children.Two(children.Item1, children.Item2);

        /// <summary>
        /// Equivalent to <c>Children.Many(list)</c>.
        /// </summary>
        /// <param name="list">The children</param>
        public static implicit operator Children<T>(ImmutableList<T> list)
            => Children.Many(list);

        /// <summary>
        /// Finds the first occurence of <paramref name="item"/> in the <paramref name="count"/> items starting from <paramref name="index"/> using the specified <see cref="IEqualityComparer{T}"/>.
        /// Returns the index of the item, if found; otherwise, -1.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="index">The index at which to start the search</param>
        /// <param name="count">The number of items from <paramref name="index"/> to search</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <returns>The index of the item, if found; otherwise, -1.</returns>
        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when index == 0:
                    return count > 0 && equalityComparer.Equals(item, _first) ? 0 : -1;
                case NumberOfChildren.Two when index == 0:
                    return count > 0 && equalityComparer.Equals(item, _first) ? 0
                        : count > 1 && equalityComparer.Equals(item, _second) ? 1
                        : -1;
                case NumberOfChildren.Two when index == 1:
                    return count > 0 && equalityComparer.Equals(item, _first) ? 1 : -1;
                case NumberOfChildren.Many:
                    return _many.IndexOf(item, index, count, equalityComparer);
            }
            return -1;
        }

        /// <summary>
        /// Finds the last occurence of <paramref name="item"/> in the <paramref name="count"/> items ending at <paramref name="index"/> using the specified <see cref="IEqualityComparer{T}"/>.
        /// Returns the index of the item, if found; otherwise, -1.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="index">The index at which to start the search</param>
        /// <param name="count">The number of items before <paramref name="index"/> to search</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <returns>The index of the item, if found; otherwise, -1.</returns>
        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when index == 0:
                    return count > 0 && equalityComparer.Equals(item, _first) ? 0 : -1;
                case NumberOfChildren.Two when index == 0:
                    return count > 1 && equalityComparer.Equals(item, _second) ? 1
                        : count > 0 && equalityComparer.Equals(item, _first) ? 0
                        : -1;
                case NumberOfChildren.Two when index == 1:
                    return count > 0 && equalityComparer.Equals(item, _first) ? 1 : -1;
                case NumberOfChildren.Many:
                    return _many.LastIndexOf(item, index, count, equalityComparer);
            }
            return -1;
        }

        /// <summary>
        /// Returns an empty <see cref="Children{T}"/>.
        /// </summary>
        /// <returns>An empty <see cref="Children{T}"/>.</returns>
        public Children<T> Clear() => Children.None<T>();
        IImmutableList<T> IImmutableList<T>.Clear() => Clear();

        /// <summary>
        /// Adds <paramref name="value"/> to the end of this <see cref="Children{T}"/>.
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="value"/> added at the end.</returns>
        public Children<T> Add(T value)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return Children.One(value);
                case NumberOfChildren.One:
                    return Children.Two(_first, value);
                case NumberOfChildren.Two:
                    return Children.Many(ImmutableList.Create(_first, _second, value));
                case NumberOfChildren.Many:
                    return Children.Many(_many.Add(value));
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }
        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        /// <summary>
        /// Adds <paramref name="items"/> to the end of this <see cref="Children{T}"/>.
        /// </summary>
        /// <param name="items">The items to add</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="items"/> added at the end.</returns>
        public Children<T> AddRange(IEnumerable<T> items)
            => Children.Many(this.ToImmutableList().AddRange(items));
        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        /// <summary>
        /// Inserts <paramref name="element"/> into this <see cref="Children{T}"/> at <paramref name="index"/>, moving later items rightward.
        /// </summary>
        /// <param name="index">The index at which to insert <paramref name="element"/></param>
        /// <param name="element">The element to insert</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="element"/> inserted at <paramref name="index"/>.</returns>
        public Children<T> Insert(int index, T element)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.None when index == 0:
                    return Children.One(element);
                case NumberOfChildren.One when index == 0:
                    return Children.Two(element, _first);
                case NumberOfChildren.One when index == 1:
                    return Children.Two(_first, element);
                case NumberOfChildren.Two:
                    return Children.Many(ImmutableList.Create(_first, _second).Insert(index, element));
                case NumberOfChildren.Many:
                    return Children.Many(_many.Insert(index, element));
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

        /// <summary>
        /// Inserts <paramref name="items"/> into this <see cref="Children{T}"/> at <paramref name="index"/>, moving later items rightward.
        /// </summary>
        /// <param name="index">The index at which to insert <paramref name="items"/></param>
        /// <param name="items">The items to insert</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="items"/> inserted at <paramref name="index"/>.</returns>
        public Children<T> InsertRange(int index, IEnumerable<T> items)
            => Children.Many(this.ToImmutableList().InsertRange(index, items));
        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

        /// <summary>
        /// Removes the first occurence of <paramref name="value"/> from this <see cref="Children{T}"/>.
        /// </summary>
        /// <param name="value">The value to remove</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with the first occurence of <paramref name="value"/> removed.</returns>
        public Children<T> Remove(T value) => Remove(value, EqualityComparer<T>.Default);
        /// <summary>
        /// Removes the first occurence of <paramref name="value"/> from this <see cref="Children{T}"/>, using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="value">The value to remove</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with the first occurence of <paramref name="value"/> removed.</returns>
        public Children<T> Remove(T value, IEqualityComparer<T> equalityComparer)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when equalityComparer.Equals(value, _first):
                    return Children.None<T>();
                case NumberOfChildren.Two when equalityComparer.Equals(value, _first):
                    return Children.One(_second);
                case NumberOfChildren.Two when equalityComparer.Equals(value, _second):
                    return Children.One(_first);
                case NumberOfChildren.Many:
                    return Children.Many(_many.Remove(value, equalityComparer));
            }
            return this;
        }
        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) => Remove(value, equalityComparer);

        /// <summary>
        /// Removes all the items for which <paramref name="match"/> returns true.
        /// </summary>
        /// <param name="match">A predicate to apply to each item in the <see cref="Children{T}"/></param>
        /// <returns>A copy of the <see cref="Children{T}"/> with all the matching items removed.</returns>
        public Children<T> RemoveAll(Predicate<T> match)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when match(_first):
                case NumberOfChildren.Two when match(_first) && match(_second):
                    return Children.None<T>();
                case NumberOfChildren.Two when match(_first):
                    return Children.One(_second);
                case NumberOfChildren.Two when match(_second):
                    return Children.One(_first);
                case NumberOfChildren.Many:
                    return Children.Many(_many.RemoveAll(match));
            }
            return this;
        }
        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        /// <summary>
        /// Remove the first occurence of each item in <paramref name="items"/>
        /// </summary>
        /// <param name="items">The items to remove</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="items"/> removed.</returns>
        public Children<T> RemoveRange(IEnumerable<T> items)
            => RemoveRange(items, EqualityComparer<T>.Default);
        /// <summary>
        /// Remove the first occurence of each item in <paramref name="items"/>, using the specified using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">The items to remove</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <returns>A copy of this <see cref="Children{T}"/> with <paramref name="items"/> removed.</returns>
        public Children<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            var result = this;
            foreach (var item in items)
            {
                result = result.Remove(item, equalityComparer);
            }
            return result;
        }
        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
            => RemoveRange(items, equalityComparer);

        /// <summary>
        /// Removes the <paramref name="count"/> items starting at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The starting index</param>
        /// <param name="count">The number of items to remove</param>
        /// <returns>A copy of the <see cref="Children{T}"/> with the <paramref name="count"/> items starting at <paramref name="index"/> removed.</returns>
        public Children<T> RemoveRange(int index, int count)
            => Children.Many(this.ToImmutableList().RemoveRange(index, count));
        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
            => RemoveRange(index, count);

        /// <summary>
        /// Removes the item at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        /// <returns>A copy of the <see cref="Children{T}"/> with the item at <paramref name="index"/> removed.</returns>
        public Children<T> RemoveAt(int index)
            => Children.Many(this.ToImmutableList().RemoveAt(index));
        IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
            => RemoveAt(index);

        /// <summary>
        /// Replaces the item at <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index of the item to replace with <paramref name="value"/></param>
        /// <param name="value">The value with which to replace the item at <paramref name="index"/></param>
        /// <returns>A copy of the <see cref="Children{T}"/> with the item at <paramref name="index"/> replaced with <paramref name="value"/>.</returns>
        public Children<T> SetItem(int index, T value)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when index == 0:
                    return Children.One(value);
                case NumberOfChildren.Two when index == 0:
                    return Children.Two(value, _second);
                case NumberOfChildren.Two when index == 1:
                    return Children.Two(_first, value);
                case NumberOfChildren.Many:
                    return Children.Many(_many.SetItem(index, value));
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
            => SetItem(index, value);

        /// <summary>
        /// Replaced the first occurence of <paramref name="oldValue"/> with <paramref name="newValue"/>
        /// </summary>
        /// <param name="oldValue">The value to search for</param>
        /// <param name="newValue">The value with which to replace <paramref name="oldValue"/></param>
        /// <returns>A copy of the <see cref="Children{T}"/> with the first occurence of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        public Children<T> Replace(T oldValue, T newValue)
            => Replace(oldValue, newValue, EqualityComparer<T>.Default);
        /// <summary>
        /// Replaced the first occurence of <paramref name="oldValue"/> with <paramref name="newValue"/>, using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="oldValue">The value to search for</param>
        /// <param name="newValue">The value with which to replace <paramref name="oldValue"/></param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <returns>A copy of the <see cref="Children{T}"/> with the first occurence of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        public Children<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.One when equalityComparer.Equals(oldValue, _first):
                    return Children.One(newValue);
                case NumberOfChildren.Two when equalityComparer.Equals(oldValue, _first):
                    return Children.Two(newValue, _second);
                case NumberOfChildren.Two when equalityComparer.Equals(oldValue, _second):
                    return Children.Two(_first, newValue);
                case NumberOfChildren.Many:
                    return Children.Many(_many.Replace(oldValue, newValue, equalityComparer));
            }
            return this;
        }
        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
            => Replace(oldValue, newValue, equalityComparer);


        /// <summary>
        /// Returns an implementation of <see cref="IEnumerator{T}"/> which yields the elements of the current instance.
        /// </summary>
        /// <returns>An implementation of <see cref="IEnumerator{T}"/> which yields the elements of the current instance.</returns>
        public Enumerator GetEnumerator()
            => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            // if we're gonna end up boxing an enumerator,
            // might as well go for a simple implementation
            if (NumberOfChildren == NumberOfChildren.Many)
            {
                return ((IEnumerable<T>)_many).GetEnumerator();
            }
            return _Enumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
            => EnumerableExtensions.GetEnumerator<Children<T>, T>(this);
        private IEnumerator<T> _Enumerator()
        {
            switch (NumberOfChildren)
            {
                case NumberOfChildren.None:
                    break;
                case NumberOfChildren.One:
                    yield return _first;
                    break;
                case NumberOfChildren.Two:
                    yield return _first;
                    yield return _second;
                    break;
            }
        }

        /// <summary>
        /// An implementation of <see cref="IEnumerator{T}"/> which yields the elements of the current instance.
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator
        {
            private readonly Children<T> _children;
            private NumberOfChildren _position;
            private readonly IEnumerator<T> _manyEnumerator;

            internal Enumerator(Children<T> children)
            {
                if (children.NumberOfChildren == NumberOfChildren.Many)
                {
                    _children = default;
                    _position = NumberOfChildren.None;
                    _manyEnumerator = children._many.GetEnumerator();
                }
                else
                {
                    _children = children;
                    _position = NumberOfChildren.None;
                    _manyEnumerator = null;
                }
            }

            /// <inheritdoc/>
            public T Current
            {
                get
                {
                    if (_manyEnumerator != null)
                    {
                        return _manyEnumerator.Current;
                    }
                    if (_position <= NumberOfChildren.None || _position > _children.NumberOfChildren)
                    {
                        // we're before the beginning or after the end
                        return default(T);
                    }
                    switch (_position)
                    {
                        case NumberOfChildren.One:
                            return _children.First;
                        case NumberOfChildren.Two:
                            return _children.Second;
                    }
                    throw new InvalidOperationException("Should be unreachable. Please report this as a bug!");
                }
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (_manyEnumerator != null)
                {
                    return _manyEnumerator.MoveNext();
                }
                _position++;
                return _position <= _children.NumberOfChildren;
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                _manyEnumerator?.Dispose();
            }
        }
    }
}
