using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
    public readonly struct Children<T> : IEnumerable<T>
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