using System.Linq.Expressions;

using Sawmill.Expressions;

using Xunit;

namespace Sawmill.Tests;

public class ExpressionRewriterTests
{
    [Fact]
    public void TestCall()
    {
        {
            var expr = _callInstanceMethod.Body;

            Assert.Equal(new[] { expr, ((MethodCallExpression)expr).Object }, expr.SelfAndDescendants().ToArray());
        }

        {
            var expr = _callStaticMethod.Body;

            Assert.Equal(new[] { expr }, expr.SelfAndDescendants().ToArray());
        }
    }

    private static readonly Expression<Action> _callStaticMethod = () => ExpressionRewriterTests.Static();

    private static void Static()
    {
    }

    private static readonly Expression<Action> _callInstanceMethod = () => "foo".ToString();
}
