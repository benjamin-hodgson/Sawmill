using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sawmill.TextLayout;

namespace Sawmill
{
    public static partial class Rewriter
    {
        public static string ToTreeString<T>(this T value) where T : IRewritable<T>
            => RewritableRewriter<T>.Instance.ToTreeString(value);
            
        public static string ToTreeString<T>(this IRewriter<T> rewriter, T value)
            => rewriter.Fold<T, Tile>(
                (n, children) =>
                {
                    var thisTile = Tile.Content(n.ToString());
                    if (!children.Any())
                    {
                        return thisTile;
                    }

                    var subtree = children.Count == 1
                        ? children.Single()
                        : children
                            .Select(
                                (t, i) =>
                                {
                                    var leftChar = i == 0 ? ' ' : '-';
                                    var rightChar = i == children.Count - 1 ? ' ' : '-';
                                    var leftLen = (t.Width - 1) / 2;
                                    var topLine = new string(leftChar, leftLen) + '+' + new string(rightChar, t.Width - 1 - leftLen);
                                    var tileWithTopLine = Tile.Content(topLine)
                                        .Above(
                                            Tile.Content("|").Above(t, HAlignment.Centre),
                                            HAlignment.Centre
                                        );
                                    return i == children.Count - 1
                                        ? tileWithTopLine
                                        : tileWithTopLine.Beside(Tile.Content("-"));
                                })
                            .Aggregate(Tile.Empty, (t1, t2) => t1.Beside(t2));

                    return thisTile
                        .Above(
                            Tile.Content("|").Above(subtree, HAlignment.Centre),
                            HAlignment.Centre
                        );
                },
                value
            )
            .ToString();
    }
}
