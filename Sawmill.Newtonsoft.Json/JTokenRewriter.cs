using System;
using System.Collections.Immutable;
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
        /// <seealso cref="Sawmill.IRewriter{T}.GetChildren(T)"/>
        /// </summary>
        public Children<JToken> GetChildren(JToken value)
            => value is JContainer c
                ? Children.Many(c.Children().ToImmutableList())
                : Children.None<JToken>();

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.SetChildren(Children{T}, T)"/>
        /// </summary>
        public JToken SetChildren(Children<JToken> newChildren, JToken oldValue)
        {
            if (newChildren.NumberOfChildren == NumberOfChildren.None)
            {
                return oldValue;
            }
            var c = (JContainer)oldValue.DeepClone();
            c.ReplaceAll(newChildren.Many);
            return c;
        }

        /// <summary>
        /// <seealso cref="Sawmill.IRewriter{T}.RewriteChildren(Func{T, T}, T)"/>
        /// </summary>
        public JToken RewriteChildren(Func<JToken, JToken> transformer, JToken oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="JTokenRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="JTokenRewriter"/>.</returns>
        public static JTokenRewriter Instance { get; } = new JTokenRewriter();
    }
}
