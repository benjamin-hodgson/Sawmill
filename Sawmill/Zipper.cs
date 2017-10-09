using System;
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

        internal Zipper(
            IRewriter<T> rewriter,
            ImmutableStack<Step<T>> path,
            ImmutableStack<Scarred<T>> prevSiblings,
            Scarred<T> focus,
            ImmutableStack<Scarred<T>> nextSiblings
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
        }

        public Zipper<T> SetFocus(T newFocus)
            => new Zipper<T>(
                _rewriter,
                _path,
                _prevSiblings,
                Scarred.Create(_rewriter, newFocus),
                _nextSiblings
            );

        public T Zip() => _focus.Heal();
        
        public Zipper<T> Top()
        {
            var (focus, prevSiblings, nextSiblings) = _path.Aggregate(
                (focus: _focus, prevSiblings: _prevSiblings, nextSiblings: _nextSiblings),
                (t, parent) => (
                    focus: new Scarred<T>(
                        _rewriter,
                        parent.Focus.OldValue,
                        true,
                        parent.Focus.NumberOfChildren,
                        t.prevSiblings,
                        t.focus,
                        t.nextSiblings
                    ),
                    parent.PrevSiblings,
                    parent.NextSiblings
                )
            );
            return new Zipper<T>(
                _rewriter,
                ImmutableStack.Create<Step<T>>(),
                prevSiblings,
                focus,
                nextSiblings
            );
        }
        public T ZipTop() => Top().Zip();

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
                    parent.Focus.OldValue,
                    true,  // it must have children, because we came up
                    parent.Focus.NumberOfChildren,
                    _prevSiblings,
                    _focus,
                    _nextSiblings
                ),
                parent.NextSiblings
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
                _path.Push(new Step<T>(_prevSiblings, _focus, _nextSiblings)),
                _focus.LeftChildren,
                _focus.FocusedChild,
                _focus.RightChildren
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
                _nextSiblings.Push(_focus)
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
                nextSiblings
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
                nextSiblings
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
                nextSiblings
            );
        }
    }
}