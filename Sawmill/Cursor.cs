using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sawmill
{
    public sealed class Cursor<T>
    {
        private readonly IRewriter<T> _rewriter;

        private readonly Stack<Step<T>> _path;

        private Stack<T> _prevSiblings;
        
        private T _focus;
        public T Focus
        {
            get
            {
                return _focus;
            }
            set
            {
                _changed = _changed || !ReferenceEquals(_focus, value);
                _focus = value;
            }
        }
        
        private Stack<T> _nextSiblings;
        
        private bool _changed;

        internal Cursor(
            IRewriter<T> rewriter,
            Stack<Step<T>> path,
            Stack<T> prevSiblings,
            T focus,
            Stack<T> nextSiblings
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

        public void Up()
        {
            if (!TryUp())
            {
                throw new InvalidOperationException("Can't go up from the top of a cursor");
            }
        }
        public bool TryUp()
        {
            if (!_path.Any())
            {
                return false;
            }
            var parent = _path.Pop();
            _focus = _changed ? SetChildren(parent.Focus) : parent.Focus;
            _prevSiblings = parent.PrevSiblings;
            _nextSiblings = parent.NextSiblings;
            _changed = parent.Changed;
            return true;
        }

        public void Down()
        {
            if (!TryDown())
            {
                throw new InvalidOperationException("Can't go down from here, focus has no children");
            }
        }
        public bool TryDown()
        {
            var children = _rewriter.GetChildren(Focus);
            if (!children.Any())
            {
                return false;
            }
            _path.Push(new Step<T>(_prevSiblings, Focus, _nextSiblings, _changed));
            _prevSiblings = new Stack<T>();
            _nextSiblings = new Stack<T>(children.Reverse());
            _focus = _nextSiblings.Pop();
            _changed = false;
            return true;
        }

        public void Left()
        {
            if (!TryLeft())
            {
                throw new InvalidOperationException("Can't go left from here, already at the leftmost sibling");
            }
        }
        public bool TryLeft()
        {
            if (!_prevSiblings.Any())
            {
                return false;
            }
            _nextSiblings.Push(Focus);
            _focus = _prevSiblings.Pop();
            return true;
        }

        public void Right()
        {
            if (!TryRight())
            {
                throw new InvalidOperationException("Can't go left from here, already at the leftmost sibling");
            }
        }
        public bool TryRight()
        {
            if (!_nextSiblings.Any())
            {
                return false;
            }
            _prevSiblings.Push(Focus);
            _focus = _nextSiblings.Pop();
            return true;
        }

        public void Leftmost()
        {
            var success = true;
            while (success)
            {
                success = TryLeft();
            }
        }

        public void Rightmost()
        {
            var success = true;
            while (success)
            {
                success = TryRight();
            }
        }

        public void Top()
        {
            var success = true;
            while (success)
            {
                success = TryUp();
            }
        }

        private T SetChildren(T value)
        {
            var children = _rewriter.GetChildren(value);
            T result;
            switch (children.NumberOfChildren)
            {
                case NumberOfChildren.None:
                    result = value;
                    break;
                case NumberOfChildren.One:
                    result = _rewriter.SetChildren(Children.One(Focus), value);
                    break;
                case NumberOfChildren.Two:
                    T first;
                    T second;
                    if (!_prevSiblings.Any())
                    {
                        first = Focus;
                        second = _nextSiblings.Peek();
                    }
                    else  // !_nextSiblings.Any()
                    {
                        first = _prevSiblings.Peek();
                        second = Focus;
                    }
                    result = _rewriter.SetChildren(Children.Two(first, second), value);
                    break;
                case NumberOfChildren.Many:
                    _nextSiblings.Push(Focus);
                    while (_prevSiblings.Any())
                    {
                        _nextSiblings.Push(_prevSiblings.Pop());
                    }

                    var newChildren = EnumerableBuilder<T>.RebuildFrom(children.Many, _nextSiblings.GetEnumerator());
                    
                    result = _rewriter.SetChildren(Children.Many(newChildren?.result ?? _nextSiblings.ToImmutableList()), value);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown {nameof(NumberOfChildren)}. Please report this as a bug!");
            }
            _nextSiblings = null;
            _prevSiblings = null;
            _focus = default(T);
            return result;
        }
    }
}