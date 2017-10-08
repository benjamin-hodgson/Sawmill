using System;
using Newtonsoft.Json.Linq;

namespace Sawmill.Newtonsoft.Json
{
    public class JTokenRewriter : IRewriter<JToken>
    {
        public Children<JToken> GetChildren(JToken value)
            => value is JContainer c
                ? Children.Many(c.Children())
                : Children.None<JToken>();

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

        public JToken RewriteChildren(Func<JToken, JToken> transformer, JToken oldValue)
            => this.DefaultRewriteChildren(transformer, oldValue);
    }
}
