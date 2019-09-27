using System;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="JToken"/>s.
    /// </summary>
    public sealed class JTokenRewriter : IRewriter<JToken>
    {
        private JTokenRewriter() {}

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.CountChildren(T)"/>
        /// </summary>
        public int CountChildren(JToken value)
            => value is JContainer c
                ? c.Children().Count()
                : 0;

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(Span{T}, T)"/>
        /// </summary>
        public void GetChildren(Span<JToken> children, JToken value)
        {
            if (value is JContainer c)
            {
                var i = 0;
                foreach (var child in value.Children())
                {
                    children[i] = child;
                    i++;
                }
            }
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(ReadOnlySpan{T}, T)"/>
        /// </summary>
        public JToken SetChildren(ReadOnlySpan<JToken> newChildren, JToken oldValue)
        {
            if (newChildren.Length == 0)
            {
                return oldValue;
            }
            var c = (JContainer)oldValue.DeepClone();
            c.ReplaceAll(newChildren.ToArray());
            return c;
        }

        /// <summary>
        /// Gets the single global instance of <see cref="JTokenRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="JTokenRewriter"/>.</returns>
        public static JTokenRewriter Instance { get; } = new JTokenRewriter();
    }
}
