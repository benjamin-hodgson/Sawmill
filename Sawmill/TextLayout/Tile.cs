using System;
using System.Collections.Generic;
using System.Linq;

namespace Sawmill.TextLayout
{
    internal class Tile
    {
        public int Width { get; }
        public int Height { get; }
        public IEnumerable<string> Rows { get; }

        public Tile(int width, IEnumerable<string> rows)
        {
            Width = width;
            Height = rows.Count();
            Rows = rows;
        }

        public static Tile Empty { get; } = new Tile(0, new string[]{});

        public static Tile Content(string content)
            => new Tile(content.Length, new[]{ content });

        public override string ToString()
            => string.Join(Environment.NewLine, Rows);

        public Tile Above(Tile other, HAlignment alignment = HAlignment.Left)
        {
            Alignment a;
            switch (alignment)
            {
                case HAlignment.Left:
                    a = Alignment.Left;
                    break;
                case HAlignment.Centre:
                    a = Alignment.Centre;
                    break;
                case HAlignment.Right:
                    a = Alignment.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment));
            }

            var newWidth = Math.Max(this.Width, other.Width);
            var newThis = this.Resize(newWidth, this.Height, a);
            var newOther = other.Resize(newWidth, other.Height, a);
            return new Tile(Math.Max(this.Width, other.Width), newThis.Rows.Concat(newOther.Rows));
        }

        public Tile Beside(Tile other, VAlignment alignment = VAlignment.Top)
        {
            Alignment a;
            switch (alignment)
            {
                case VAlignment.Top:
                    a = Alignment.Top;
                    break;
                case VAlignment.Centre:
                    a = Alignment.Centre;
                    break;
                case VAlignment.Bottom:
                    a = Alignment.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment));
            }

            var newHeight = Math.Max(this.Height, other.Height);
            var newThis = this.Resize(this.Width, newHeight, a);
            var newOther = other.Resize(other.Width, newHeight, a);
            return new Tile(this.Width + other.Width, newThis.Rows.Zip(newOther.Rows, string.Concat));
        }

        public Tile Resize(int newWidth, int newHeight, Alignment alignment)
        {
            if (newWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newWidth), $"{nameof(newWidth)} cannot be negative.");
            }
            if (newHeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newHeight), $"{nameof(newHeight)} cannot be negative.");
            }
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return this.ResizeWLeft(newWidth).ResizeHTop(newHeight);
                case Alignment.Top:
                    return this.ResizeWCentre(newWidth).ResizeHTop(newHeight);
                case Alignment.TopRight:
                    return this.ResizeWRight(newWidth).ResizeHTop(newHeight);
                case Alignment.Left:
                    return this.ResizeWLeft(newWidth).ResizeHCentre(newHeight);
                case Alignment.Centre:
                    return this.ResizeWCentre(newWidth).ResizeHCentre(newHeight);
                case Alignment.Right:
                    return this.ResizeWRight(newWidth).ResizeHCentre(newHeight);
                case Alignment.BottomLeft:
                    return this.ResizeWLeft(newWidth).ResizeHBottom(newHeight);
                case Alignment.Bottom:
                    return this.ResizeWCentre(newWidth).ResizeHBottom(newHeight);
                case Alignment.BottomRight:
                    return this.ResizeWRight(newWidth).ResizeHBottom(newHeight);
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment));
            }
        }

        private Tile ResizeWLeft(int newWidth)
        {
            if (newWidth == Width)
            {
                return this;
            }
            IEnumerable<string> rows;
            if (newWidth > Width)
            {
                rows = Rows.Select(r => r + new string(' ', newWidth - Width));
            }
            else // newWidth < Width
            {
                rows = Rows.Select(r => r.Substring(0, newWidth));
            }
            return new Tile(newWidth, rows);
        }
        private Tile ResizeWRight(int newWidth)
        {
            if (newWidth == Width)
            {
                return this;
            }

            IEnumerable<string> rows;
            if (newWidth > Width)
            {
                rows = Rows.Select(r => new string(' ', newWidth - Width) + r);
            }
            else  // newWidth < Width
            {
                rows = Rows.Select(r => r.Substring(Width - newWidth, newWidth));
            }
            return new Tile(newWidth, rows);
        }
        private Tile ResizeWCentre(int newWidth)
        {
            if (newWidth == Width)
            {
                return this;
            }

            IEnumerable<string> rows;
            if (newWidth > Width)
            {
                var left = (newWidth - Width) / 2;
                var right = (newWidth - Width) - left;
                rows = Rows.Select(r => new string(' ', left) + r + new string(' ', right));
            }
            else  // newWidth < Width
            {
                var left = (Width - newWidth) / 2;
                rows = Rows.Select(r => r.Substring(left, newWidth));
            }
            return new Tile(newWidth, rows);
        }
        private Tile ResizeHTop(int newHeight)
        {
            if (newHeight == Height)
            {
                return this;
            }

            IEnumerable<string> rows;
            if (newHeight > Height)
            {
                rows = Rows.Concat(Enumerable.Repeat(new string(' ', Width), newHeight - Height));
            }
            else  // newHeight < Height
            {
                rows = Rows.Take(newHeight);
            }
            return new Tile(Width, rows);
        }
        private Tile ResizeHBottom(int newHeight)
        {
            if (newHeight == Height)
            {
                return this;
            }
            IEnumerable<string> rows;
            if (newHeight > Height)
            {
                rows = Enumerable.Repeat(new string(' ', Width), newHeight - Height).Concat(Rows);
            }
            else  // newHeight < Height
            {
                rows = Rows.Skip(Height - newHeight);
            }
            return new Tile(Width, rows);
        }
        private Tile ResizeHCentre(int newHeight)
        {
            if (newHeight == Height)
            {
                return this;
            }
            IEnumerable<string> rows;
            if (newHeight > Height)
            {
                var top = (newHeight - Height) / 2;
                var bottom = newHeight - top;
                var blank = new string(' ', Width);
                rows = Enumerable.Repeat(blank, top).Concat(Rows).Concat(Enumerable.Repeat(blank, bottom));
            }
            else  // newHeight < Height
            {
                rows = Rows.Skip((newHeight - Height) / 2).Take(newHeight);
            }
            return new Tile(Width, rows);
        }
    }
}