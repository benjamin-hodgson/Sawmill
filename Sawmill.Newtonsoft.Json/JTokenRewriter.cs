using System;
using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json
{
    /// <summary>
    /// An implementation of <see cref="IRewriter{T}"/> for <see cref="JToken"/>s.
    /// </summary>
    public sealed class JTokenRewriter : IRewriter<JToken>
    {
        private JTokenRewriter() {}

        /// <inheritdoc/>
        public Children<JToken> GetChildren(JToken value)
            => value is JContainer c
                ? Children.Many(c.Children())
                : Children.None<JToken>();

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public JToken RewriteChildren(Func<JToken, JToken> transformer, JToken oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);

        /// <summary>
        /// Gets the single global instance of <see cref="JTokenRewriter"/>.
        /// </summary>
        /// <returns>The single global instance of <see cref="JTokenRewriter"/>.</returns>
        public static JTokenRewriter Instance { get; } = new JTokenRewriter();
    }
}
