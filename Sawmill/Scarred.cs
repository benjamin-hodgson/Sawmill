using System;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    internal static class Scarred
    {
        public static Scarred<T> Create<T>(IRewriter<T> rewriter, T value)
            => new Scarred<T>(rewriter, value, null, null, null, null, null);
    }
    // A view of a value and its lazily-instantiated, possibly-changed children
    internal sealed class Scarred<T>
    {
        private readonly IRewriter<T> _rewriter;

        public T OldValue { get; }

        private bool? _hasChildren;
        private NumberOfChildren? _numberOfChildren;
        private ImmutableStack<Scarred<T>> _leftChildren;
        private Scarred<T> _focusedChild;
        private ImmutableStack<Scarred<T>> _rightChildren;

        public Scarred(
            IRewriter<T> rewriter,
            T oldValue,
            bool? hasChildren,
            NumberOfChildren? numberOfChildren,
            ImmutableStack<Scarred<T>> leftChildren,
            Scarred<T> focusedChild,
            ImmutableStack<Scarred<T>> rightChildren
        )
        {
            if (_rewriter == null)
            {
                _rewriter = rewriter;
            }

            _rewriter = rewriter;
            OldValue = oldValue;

            _hasChildren = hasChildren;
            _numberOfChildren = numberOfChildren;
            _leftChildren = leftChildren;
            _focusedChild = focusedChild;
            _rightChildren = rightChildren;

            CheckInitialisedOrUninitialised();
        }

        public bool HasChildren
        {
            get
            {
                EnsureChildren();
                return _hasChildren.Value;
            }
        }
        public NumberOfChildren NumberOfChildren
        {
            get
            {
                EnsureChildren();
                return _numberOfChildren.Value;
            }
        }
        public ImmutableStack<Scarred<T>> LeftChildren
        {
            get
            {
                EnsureChildren();
                return _leftChildren;
            }
        }
        public Scarred<T> FocusedChild
        {
            get
            {
                EnsureChildren();
                return _focusedChild;
            }
        }
        public ImmutableStack<Scarred<T>> RightChildren
        {
            get
            {
                EnsureChildren();
                return _rightChildren;
            }
        }

        private bool _hasHealed = false;
        private T _healed;
        public T Heal()
        {
            CheckInitialisedOrUninitialised();
            if (Uninitialised || !_hasChildren.Value)
            {
                return OldValue;
            }
            if (_hasHealed)
            {
                // Heal() has already been called on this instance, so don't traverse/rebuild again
                return _healed;
            }

            // the children have been traversed, so put _value back together
            _healed = HealChildren();
            _hasHealed = true;
            return _healed;
        }

        private T HealChildren()
        {
            switch (_numberOfChildren)
            {
                case NumberOfChildren.None:
                    return OldValue;
                case NumberOfChildren.One:
                    return _rewriter.SetChildren(Children.One(_focusedChild.Heal()), OldValue);
                case NumberOfChildren.Two:
                    Scarred<T> first;
                    Scarred<T> second;
                    if (_leftChildren.IsEmpty)
                    {
                        first = _focusedChild;
                        second = _rightChildren.Peek();
                    }
                    else  // rightChildren.IsEmpty
                    {
                        first = _leftChildren.Peek();
                        second = _focusedChild;
                    }
                    return _rewriter.SetChildren(Children.Two(first.Heal(), second.Heal()), OldValue);
                case NumberOfChildren.Many:
                    // todo: return enumerable of same type
                    var children = _leftChildren.Select(c => c.Heal()).Reverse().ToList();
                    children.Add(_focusedChild.Heal());
                    children.AddRange(_rightChildren.Select(c => c.Heal()));
                    return _rewriter.SetChildren(Children.Many(children), OldValue);
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }

        private void EnsureChildren()
        {
            if (_hasChildren == null)
            {
                CheckUninitialised();

                var children = _rewriter.GetChildren(OldValue);
                _hasChildren = children.Any();
                _numberOfChildren = children.NumberOfChildren;
                _leftChildren = ImmutableStack.Create<Scarred<T>>();

                if (!_hasChildren.Value)
                {
                    _focusedChild = null;
                    _rightChildren = ImmutableStack.Create<Scarred<T>>();
                }
                else
                {
                    var childrenScarred = children.Select(c => Scarred.Create(_rewriter, c));
                    var childrenStack = ImmutableStack.CreateRange(childrenScarred.Reverse());
                    _rightChildren = childrenStack.Pop(out _focusedChild);
                }
            }
            CheckInitialised();
        }

        private bool Initialised
            => _hasChildren != null
                && _numberOfChildren != null
                && _leftChildren != null
                && _rightChildren != null
                && NumberOfChildrenIsCorrect();
        private bool NumberOfChildrenIsCorrect()
        {
            switch (_numberOfChildren)
            {
                case NumberOfChildren.None:
                    // there should be no children
                    return _leftChildren.IsEmpty && _focusedChild == null || _rightChildren.IsEmpty;
                case NumberOfChildren.One:
                    // there should be one child
                    return _leftChildren.IsEmpty && _focusedChild != null || _rightChildren.IsEmpty;
                case NumberOfChildren.Two:
                    // there should be two children
                    if (_focusedChild == null)
                    {
                        return false;
                    }
                    if (_leftChildren.IsEmpty && _rightChildren.Count() != 1)
                    {
                        return false;
                    }
                    else if (_rightChildren.IsEmpty && _leftChildren.Count() != 1)
                    {
                        return false;
                    }
                    return true;
                case NumberOfChildren.Many:
                    return _focusedChild != null
                        || (_leftChildren.IsEmpty && _rightChildren.IsEmpty);
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }
        
        private void CheckInitialised()
        {
            if (!Initialised)
            {
                throw InvalidState();
            }
        }
        
        private bool Uninitialised
            => _hasChildren == null
                && _numberOfChildren == null
                && _leftChildren == null
                && _focusedChild == null
                && _rightChildren == null;
        private void CheckUninitialised()
        {
            if (!Uninitialised)
            {
                throw InvalidState();
            }
        }

        private void CheckInitialisedOrUninitialised()
        {
            if (!Initialised && !Uninitialised)
            {
                throw InvalidState();
            }
        }

        private Exception InvalidState()
        {
            var e = new InvalidOperationException($"Scarred<{typeof(T).Name}> was in an invalid state. Please report this as a bug!");
            e.Data.Add(nameof(_hasChildren), _hasChildren);
            e.Data.Add(nameof(_numberOfChildren), _numberOfChildren);
            e.Data.Add(nameof(_leftChildren), _leftChildren);
            e.Data.Add(nameof(_focusedChild), _focusedChild);
            e.Data.Add(nameof(_rightChildren), _rightChildren);
            return e;
        }
    }
}