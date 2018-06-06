using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sawmill
{
    public static partial class Rewriter
    {
        public static string ToTreeString<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.ToTreeString(value);
            
        public static string ToTreeString<T>(this IRewriter<T> rewriter, T value)
        {
            string Twigs(IEnumerable<TreeString> children)
                => string.Join(" ", children.Select(t => new string(' ', t.Width / 2) + '|' + new string(' ', t.Width / 2)));
            string Branches(IEnumerable<TreeString> children)
            {
                var sb = new StringBuilder();
                if (children.Count() == 1)
                {
                    sb.Append(' ', children.First().Width / 2);
                    sb.Append('+');
                    sb.Append(' ', children.First().Width / 2);
                    return sb.ToString();
                }

                sb.Append(' ', children.First().Width / 2);
                sb.Append('+');
                sb.Append('-', children.First().Width / 2);
                sb.Append('-');

                for (var i = 1; i < children.Count() - 1; i++)
                {
                    sb.Append('-', children.ElementAt(i).Width / 2);
                    sb.Append('+');
                    sb.Append('-', children.ElementAt(i).Width / 2);
                    sb.Append('-');
                }

                sb.Append('-', children.Last().Width / 2);
                sb.Append('+');
                sb.Append(' ', children.Last().Width / 2);

                return sb.ToString();
            }

            IEnumerable<string> ZipRows(IEnumerable<TreeString> xs)
            {
                var enumerators = xs.Select(x => (exhausted: false, enumerator: x.Rows.GetEnumerator())).ToArray();

                while (true)
                {
                    var currents = new string[enumerators.Length];
                    for (var i = 0; i < enumerators.Length; i++)
                    {
                        var (exhausted, e) = enumerators[i];
                        var hasNext = e.MoveNext();
                        if (exhausted || !hasNext)
                        {
                            enumerators[i].exhausted = true;
                            currents[i] = new string(' ', xs.ElementAt(i).Width);
                        }
                        else
                        {
                            currents[i] = e.Current;
                        }
                    }
                    if (enumerators.All(x => x.exhausted))
                    {
                        yield break;
                    }
                    yield return string.Join(" ", currents);
                }
            }

            var ts = rewriter.Fold<T, TreeString>(
                (t, children) =>
                {
                    var tString = t.ToString();
                    var name = tString.Length % 2 == 0 ? tString + ' ' : tString;
                    var totalWidth = Math.Max(children.Sum(c => c.Width) + children.Count() - 1, name.Length);

                    if (!children.Any())
                    {
                        return new TreeString(totalWidth, new[] { name });
                    }

                    var namePadding = totalWidth - name.Length;
                    var nameRow = new string(' ', namePadding / 2) + name + new string(' ', namePadding / 2);

                    var trunk = new string(' ', totalWidth / 2) + "|" + new string(' ', totalWidth / 2);
                    var branches = Branches(children);
                    var twigs = Twigs(children);
                    
                    return new TreeString(totalWidth, new[] { nameRow, trunk, branches, twigs }.Concat(ZipRows(children)));
                },
                value
            );
            return string.Join(Environment.NewLine, ts.Rows);
        }
        
        private class TreeString
        {
            public int Width { get; }
            public IEnumerable<string> Rows { get; }

            public TreeString(int width, IEnumerable<string> rows)
            {
                Width = width;
                Rows = rows;
            }
        }
    }
}