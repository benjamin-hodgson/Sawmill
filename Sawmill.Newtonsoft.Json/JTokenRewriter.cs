using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json;

/// <summary>
/// An implementation of <see cref="IRewriter{T}"/> for <see cref="JToken"/>s.
/// </summary>
public class JTokenRewriter : IRewriter<JToken>
{
    /// <summary>
    /// Create a new instance of <see cref="JTokenRewriter"/>
    /// </summary>
    protected JTokenRewriter() { }

    /// <summary>
    /// <seealso cref="IRewriter{T}.CountChildren(T)"/>
    /// </summary>
    public int CountChildren(JToken value)
        => value is JContainer c
            ? c.Children().Count()
            : 0;

    /// <summary>
    /// <seealso cref="IRewriter{T}.GetChildren(Span{T}, T)"/>
    /// </summary>
    public void GetChildren(Span<JToken> childrenReceiver, JToken value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (value is JContainer)
        {
            var i = 0;
            foreach (var child in value.Children())
            {
                childrenReceiver[i] = child;
                i++;
            }
        }
    }

    /// <summary>
    /// <seealso cref="IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
    /// </summary>
    public JToken SetChildren(ReadOnlySpan<JToken> newChildren, JToken value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (newChildren.Length == 0)
        {
            return value;
        }
        var c = (JContainer)value.DeepClone();
        c.ReplaceAll(newChildren.ToArray());
        return c;
    }

    /// <summary>
    /// Gets the single global instance of <see cref="JTokenRewriter"/>.
    /// </summary>
    /// <returns>The single global instance of <see cref="JTokenRewriter"/>.</returns>
    public static JTokenRewriter Instance { get; } = new JTokenRewriter();
}
