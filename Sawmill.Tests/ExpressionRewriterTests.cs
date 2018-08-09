using Sawmill.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Sawmill.Tests
{
    public class ExpressionRewriterTests
    {
        [Fact]
        public void TestCall()
        {
            {
                var expr = _callInstanceMethod.Body;

                Assert.Equal(new[]{ expr, ((MethodCallExpression)expr).Object }, ExpressionExtensions.SelfAndDescendants(expr).ToArray());
            }
            {
                var expr = _callStaticMethod.Body;

                Assert.Equal(new[]{ expr }, ExpressionExtensions.SelfAndDescendants(expr).ToArray());
            }
        }

        private static readonly Expression<Action> _callStaticMethod = () => ExpressionRewriterTests.Static();
        public static void Static() {}

        private static readonly Expression<Action> _callInstanceMethod = () => "foo".ToString();
    }
}