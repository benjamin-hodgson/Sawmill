using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    public sealed class Zipper<T>
    {
        private readonly IRewriter<T> _rewriter;

        private readonly ImmutableStack<Step<T>> _path;

        private readonly ImmutableStack<Scarred<T>> _prevSiblings;
        private readonly Scarred<T> _focus;
        private readonly ImmutableStack<Scarred<T>> _nextSiblings;

        private readonly bool _focusOrSiblingsChanged;

        internal Zipper(
            IRewriter<T> rewriter,
            ImmutableStack<Step<T>> path,
            ImmutableStack<Scarred<T>> prevSiblings,
            Scarred<T> focus,
            ImmutableStack<Scarred<T>> nextSiblings,
            bool focusOrSiblingsChanged
        )
        {
            if (rewriter == null)
            {
                throw new ArgumentNullException(nameof(rewriter));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (prevSiblings == null)
            {
                throw new ArgumentNullException(nameof(prevSiblings));
            }
            if (focus == null)
            {
                throw new ArgumentNullException(nameof(focus));
            }
            if (nextSiblings == null)
            {
                throw new ArgumentNullException(nameof(nextSiblings));
            }

            _rewriter = rewriter;
            _path = path;
            _prevSiblings = prevSiblings;
            _focus = focus;
            _nextSiblings = nextSiblings;
            _focusOrSiblingsChanged = focusOrSiblingsChanged;
        }

        public Zipper<T> SetFocus(T newFocus)
            => ReferenceEquals(newFocus, _focus.Value)
                ? this
                : new Zipper<T>(
                    _rewriter,
                    _path,
                    _prevSiblings,
                    Scarred.Create(_rewriter, newFocus),
                    _nextSiblings,
                    true
                );

        public T Focus => _focus.Value;

        public Zipper<T> Up()
        {
            if (_path.IsEmpty)
            {
                return null;
            }

            return new Zipper<T>(
                _rewriter,
                _path.Pop(out var parent),
                parent.PrevSiblings,
                new Scarred<T>(
                    _rewriter,
                    _focusOrSiblingsChanged
                        ? SetChildren(parent.Focus.Value, _prevSiblings, _focus.Value, _nextSiblings)
                        : parent.Focus.Value,
                    true,  // it must have children, because we came up
                    parent.Focus.NumberOfChildren,
                    _prevSiblings,
                    _focus,
                    _nextSiblings
                ),
                parent.NextSiblings,
                _focusOrSiblingsChanged || parent.FocusOrSiblingsChanged
            );
        }
        public Zipper<T> TryUp() => Up() ?? this;

        public Zipper<T> Down()
        {
            if (!_focus.HasChildren)
            {
                return null;
            }

            return new Zipper<T>(
                _rewriter,
                _path.Push(new Step<T>(_prevSiblings, _focus, _nextSiblings, _focusOrSiblingsChanged)),
                _focus.LeftChildren,
                _focus.FocusedChild,
                _focus.RightChildren,
                false
            );
        }
        public Zipper<T> TryDown() => Down() ?? this;

        public Zipper<T> Left()
        {
            if (_prevSiblings.IsEmpty)
            {
                return null;
            }

            return new Zipper<T>(
                _rewriter,
                _path,
                _prevSiblings.Pop(out var newFocus),
                newFocus,
                _nextSiblings.Push(_focus),
                _focusOrSiblingsChanged
            );
        }
        public Zipper<T> TryLeft() => Left() ?? this;

        public Zipper<T> Right()
        {
            if (_nextSiblings.IsEmpty)
            {
                return null;
            }
            
            var nextSiblings = _nextSiblings.Pop(out var newFocus);
            return new Zipper<T>(
                _rewriter,
                _path,
                _prevSiblings.Push(_focus),
                newFocus,
                nextSiblings,
                _focusOrSiblingsChanged
            );
        }
        public Zipper<T> TryRight() => Right() ?? this;

        public Zipper<T> Leftmost()
        {
            if (_prevSiblings.IsEmpty)
            {
                return this;
            }

            var prevSiblings = _prevSiblings;
            var focus = _focus;
            var nextSiblings = _nextSiblings;

            while (!prevSiblings.IsEmpty)
            {
                nextSiblings = nextSiblings.Push(focus);
                prevSiblings = prevSiblings.Pop(out focus);
            }

            return new Zipper<T>(
                _rewriter,
                _path,
                prevSiblings,
                focus,
                nextSiblings,
                _focusOrSiblingsChanged
            );
        }

        public Zipper<T> Rightmost()
        {
            if (_nextSiblings.IsEmpty)
            {
                return this;
            }
            
            var nextSiblings = _nextSiblings;
            var focus = _focus;
            var prevSiblings = _prevSiblings;

            while (!nextSiblings.IsEmpty)
            {
                prevSiblings = prevSiblings.Push(focus);
                nextSiblings = nextSiblings.Pop(out focus);
            }

            return new Zipper<T>(
                _rewriter,
                _path,
                prevSiblings,
                focus,
                nextSiblings,
                _focusOrSiblingsChanged
            );
        }

        public Zipper<T> Top()
        {
            var focus = _focus;
            var prevSiblings = _prevSiblings;
            var nextSiblings = _nextSiblings;
            var changed = _focusOrSiblingsChanged;

            var path = _path;

            while (!path.IsEmpty)
            {
                path = path.Pop(out var parent);
                focus = new Scarred<T>(
                    _rewriter,
                    changed
                        ? SetChildren(parent.Focus.Value, prevSiblings, focus.Value, nextSiblings)
                        : parent.Focus.Value,
                    true,  // it must have children, because we came up
                    parent.Focus.NumberOfChildren,
                    prevSiblings,
                    focus,
                    nextSiblings
                );
                prevSiblings = parent.PrevSiblings;
                nextSiblings = parent.NextSiblings;
                changed = changed || parent.FocusOrSiblingsChanged;
            }

            return new Zipper<T>(_rewriter, path, prevSiblings, focus, nextSiblings, changed);
        }

        private T SetChildren(T value, ImmutableStack<Scarred<T>> leftChildren, T focusedChild, ImmutableStack<Scarred<T>> rightChildren)
        {
            var children = _rewriter.GetChildren(value);
            switch (children.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    return value;
                case NumberOfChildren.One:
                    return _rewriter.SetChildren(Children.One(focusedChild), value);
                case NumberOfChildren.Two:
                    T first;
                    T second;
                    if (leftChildren.IsEmpty)
                    {
                        first = focusedChild;
                        second = rightChildren.Peek().Value;
                    }
                    else  // rightChildren.IsEmpty
                    {
                        first = leftChildren.Peek().Value;
                        second = focusedChild;
                    }
                    return _rewriter.SetChildren(Children.Two(first, second), value);
                case NumberOfChildren.Many:
                    var childrenList = leftChildren.Select(s => s.Value).Reverse().ToList();
                    childrenList.Add(focusedChild);
                    childrenList.AddRange(rightChildren.Select(s => s.Value));

                    var newChildren = EnumerableBuilder<T>.RebuildFrom(children.Many, childrenList.GetEnumerator());
                    
                    return _rewriter.SetChildren(Children.Many(newChildren?.result ?? childrenList), value);
            }
            throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
        }
    }
}