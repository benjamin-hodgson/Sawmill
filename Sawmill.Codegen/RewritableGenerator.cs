namespace Sawmill.Codegen;

internal static class RewritableGenerator
{
    public static void Go()
    {
        GenerateFile(
            "Sawmill/Expressions/ExpressionExtensions.Generated.cs",
            "Sawmill.Expressions",
            "ExpressionExtensions",
            "System.Linq.Expressions.Expression",
            "ExpressionRewriter.Instance"
        );
        GenerateFile(
            "Sawmill/Xml/XElementExtensions.Generated.cs",
            "Sawmill.Xml",
            "XElementExtensions",
            "System.Xml.Linq.XElement",
            "XElementRewriter.Instance"
        );
        GenerateFile(
            "Sawmill/Xml/XmlNodeExtensions.Generated.cs",
            "Sawmill.Xml",
            "XmlNodeExtensions",
            "System.Xml.XmlNode",
            "XmlNodeRewriter.Instance"
        );
        GenerateFile(
            "Sawmill.Newtonsoft.Json/JTokenExtensions.Generated.cs",
            "Sawmill.Newtonsoft.Json",
            "JTokenExtensions",
            "global::Newtonsoft.Json.Linq.JToken",
            "JTokenRewriter.Instance"
        );
        GenerateFile(
            "Sawmill.HtmlAgilityPack/HtmlNodeExtensions.Generated.cs",
            "Sawmill.HtmlAgilityPack",
            "HtmlNodeExtensions",
            "global::HtmlAgilityPack.HtmlNode",
            "HtmlNodeRewriter.Instance"
        );
        GenerateFile(
            "Sawmill.Microsoft.CodeAnalysis.CSharp/CSharpSyntaxNodeExtensions.Generated.cs",
            "Sawmill.Microsoft.CodeAnalysis.CSharp",
            "CSharpSyntaxNodeExtensions",
            "global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode",
            "SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode>.Instance"
        );
        GenerateFile(
            "Sawmill.Microsoft.CodeAnalysis.VisualBasic/VisualBasicSyntaxNodeExtensions.Generated.cs",
            "Sawmill.Microsoft.CodeAnalysis.VisualBasic",
            "VisualBasicSyntaxNodeExtensions",
            "global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode",
            "SyntaxNodeRewriter<global::Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxNode>.Instance"
        );
    }

    private static void GenerateFile(
        string filename,
        string ns,
        string className,
        string typeName,
        string rewriterExpr
    )
    {
        var result = $@"#region GeneratedCode
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace {ns}
{{
    /// <summary>
    /// Extension methods for <see cref=""{typeName}""/>s.
    /// </summary>
    public static class {className}
    {{
        /// <inheritdoc cref=""IRewriter{{T}}.CountChildren"" />
        public static int CountChildren(this {typeName} value)
            => {rewriterExpr}.CountChildren(value);

        /// <inheritdoc cref=""Rewriter.GetChildren{{T}}(IRewriter{{T}}, T)"" />
        public static {typeName}[] GetChildren(this {typeName} value)
            => {rewriterExpr}.GetChildren(value);

        /// <inheritdoc cref=""IRewriter{{T}}.GetChildren"" />"" />
        public static void GetChildren(this {typeName} value, Span<{typeName}> childrenReceiver)
            => {rewriterExpr}.GetChildren(childrenReceiver, value);

        /// <inheritdoc cref=""IRewriter{{T}}.SetChildren"" />"" />
        public static {typeName} SetChildren(this {typeName} value, ReadOnlySpan<{typeName}> newChildren)
            => {rewriterExpr}.SetChildren(newChildren, value);

        /// <inheritdoc cref=""Rewriter.DescendantsAndSelf{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<{typeName}> DescendantsAndSelf(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelf(value);

        /// <inheritdoc cref=""Rewriter.SelfAndDescendants{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<{typeName}> SelfAndDescendants(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendants(value);

        /// <inheritdoc cref=""Rewriter.SelfAndDescendantsBreadthFirst{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<{typeName}> SelfAndDescendantsBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsBreadthFirst(value);

        /// <inheritdoc cref=""Rewriter.ChildrenInContext{{T}}(IRewriter{{T}}, T)"" />
        public static ({typeName} item, Func<{typeName}, {typeName}> replace)[] ChildrenInContext(this {typeName} value)
            => {rewriterExpr}.ChildrenInContext(value);

        /// <inheritdoc cref=""Rewriter.SelfAndDescendantsInContext{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContext(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContext(value);

        /// <inheritdoc cref=""Rewriter.DescendantsAndSelfInContext{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> DescendantsAndSelfInContext(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelfInContext(value);

        /// <inheritdoc cref=""Rewriter.SelfAndDescendantsInContextBreadthFirst{{T}}(IRewriter{{T}}, T)"" />
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContextBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContextBreadthFirst(value);

        /// <inheritdoc cref=""Rewriter.DescendantAt{{T}}(IRewriter{{T}}, IEnumerable{{Direction}}, T)"" />
        public static {typeName} DescendantAt(this {typeName} value, IEnumerable<Direction> path)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}

            return {rewriterExpr}.DescendantAt(path, value);
        }}

        /// <inheritdoc cref=""Rewriter.ReplaceDescendantAt{{T}}(IRewriter{{T}}, IEnumerable{{Direction}}, T, T)"" />
        public static {typeName} ReplaceDescendantAt<T>(this {typeName} value, IEnumerable<Direction> path, {typeName} newDescendant)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}

            return {rewriterExpr}.ReplaceDescendantAt(path, newDescendant, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteDescendantAt{{T}}(IRewriter{{T}}, IEnumerable{{Direction}}, Func{{T, T}}, T)"" />
        public static {typeName} RewriteDescendantAt<T>(this {typeName} value, IEnumerable<Direction> path, Func<{typeName}, {typeName}> transformer)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}

            return {rewriterExpr}.RewriteDescendantAt(path, transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteDescendantAt{{T}}(IRewriter{{T}}, IEnumerable{{Direction}}, Func{{T, ValueTask{{T}}}}, T)"" />
        public static ValueTask<{typeName}> RewriteDescendantAt<T>(this {typeName} value, IEnumerable<Direction> path, Func<{typeName}, ValueTask<{typeName}>> transformer)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}

            return {rewriterExpr}.RewriteDescendantAt(path, transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.Cursor{{T}}(IRewriter{{T}}, T)"" />
        public static Cursor<{typeName}> Cursor(this {typeName} value)
            => {rewriterExpr}.Cursor(value);

        /// <inheritdoc cref=""Rewritable.Fold{{T, U}}(T, SpanFunc{{U, T, U}})"" />
        public static T Fold<T>(this {typeName} value, SpanFunc<T, {typeName}, T> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            return {rewriterExpr}.Fold(func, value);
        }}

        /// <inheritdoc cref=""Rewritable.Fold{{T, U}}(T, Func{{Memory{{U}}, T, ValueTask{{U}}}})"" />
        public static ValueTask<T> Fold<T>(this {typeName} value, Func<Memory<T>, {typeName}, ValueTask<T>> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            return {rewriterExpr}.Fold(func, value);
        }}

        /// <inheritdoc cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IEnumerable{{U}}, U}}, T[])"" />
        public static U ZipFold<U>(this {typeName}[] values, Func<{typeName}[], IEnumerable<U>, U> func)
        {{
            if (values == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold(func, values);
        }}

        /// <inheritdoc cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IAsyncEnumerable{{U}}, ValueTask{{U}}}}, T[])"" />
        public static ValueTask<U> ZipFold<U>(this {typeName}[] values, Func<{typeName}[], IAsyncEnumerable<U>, ValueTask<U>> func)
        {{
            if (values == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold(func, values);
        }}

        /// <inheritdoc cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IEnumerable{{U}}, U}}, T[])"" />
        public static U ZipFold<U>(this {typeName} value1, {typeName} value2, Func<{typeName}, {typeName}, IEnumerable<U>, U> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold<{typeName}, U>((xs, cs) => func(xs[0], xs[1], cs), new[] {{ value1, value2 }});
        }}

        /// <inheritdoc cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IAsyncEnumerable{{U}}, ValueTask{{U}}}}, T[])"" />
        public static ValueTask<U> ZipFold<U>(this {typeName} value1, {typeName} value2, Func<{typeName}, {typeName}, IAsyncEnumerable<U>, ValueTask<U>> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold<{typeName}, U>((xs, cs) => func(xs[0], xs[1], cs), new[] {{ value1, value2 }});
        }}

        /// <inheritdoc cref=""Rewriter.Rewrite{{T}}(IRewriter{{T}}, Func{{T, T}}, T)"" />
        public static {typeName} Rewrite(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.Rewrite(transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.Rewrite{{T}}(IRewriter{{T}}, Func{{T, ValueTask{{T}}}}, T)"" />
        public static ValueTask<{typeName}> Rewrite(this {typeName} value, Func<{typeName}, ValueTask<{typeName}>> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.Rewrite(transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteChildren{{T}}(IRewriter{{T}}, Func{{T, T}}, T)"" />
        public static {typeName} RewriteChildren(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteChildren(transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteChildren{{T}}(IRewriter{{T}}, Func{{T, ValueTask{{T}}}}, T)"" />
        public static ValueTask<{typeName}> RewriteChildren(this {typeName} value, Func<{typeName}, ValueTask<{typeName}>> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteChildren(transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteIter{{T}}(IRewriter{{T}}, Func{{T, T}}, T)"" />
        public static {typeName} RewriteIter(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteIter(transformer, value);
        }}

        /// <inheritdoc cref=""Rewriter.RewriteIter{{T}}(IRewriter{{T}}, Func{{T, ValueTask{{T}}}}, T)"" />
        public static ValueTask<{typeName}> RewriteIter(this {typeName} value, Func<{typeName}, ValueTask<{typeName}>> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteIter(transformer, value);
        }}
    }}
}}
#endregion
";
        File.WriteAllText(filename, result);
    }
}
