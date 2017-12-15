using System;
using System.IO;

namespace Sawmill.Codegen
{
    class Program
    {
        static void Main(string[] args)
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
using System.Collections.Immutable;

namespace {ns}
{{
    /// <summary>
    /// Extension methods for <see cref=""{typeName}""/>s.
    /// </summary>
    public static class {className}
    {{
        /// <summary>
        /// <seealso cref=""IRewriter{{T}}.GetChildren(T)""/>
        /// </summary>
        public static Children<{typeName}> GetChildren(this {typeName} value)
            => {rewriterExpr}.GetChildren(value);

        /// <summary>
        /// <seealso cref=""IRewriter{{T}}.SetChildren(Children{{T}}, T)""/>
        /// </summary>
        public static {typeName} SetChildren(this {typeName} value, Children<{typeName}> newChildren)
            => {rewriterExpr}.SetChildren(newChildren, value);

        /// <summary>
        /// <seealso cref=""Rewriter.DescendantsAndSelf{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> DescendantsAndSelf(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelf(value);
        
        /// <summary>
        /// <seealso cref=""Rewriter.DescendantsAndSelfLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> DescendantsAndSelfLazy(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelfLazy(value);
            
        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendants{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> SelfAndDescendants(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendants(value);
        
        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> SelfAndDescendantsLazy(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsLazy(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsBreadthFirst{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> SelfAndDescendantsBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsBreadthFirst(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsBreadthFirstLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<{typeName}> SelfAndDescendantsBreadthFirstLazy(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref=""Rewriter.ChildrenInContext{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static Children<({typeName} item, Func<{typeName}, {typeName}> replace)> ChildrenInContext(this {typeName} value)
            => {rewriterExpr}.ChildrenInContext(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsInContext{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContext(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContext(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsInContextLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContextLazy(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContextLazy(value);

        /// <summary>
        /// <seealso cref=""Rewriter.DescendantsAndSelfInContext{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> DescendantsAndSelfInContext(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelfInContext(value);

        /// <summary>
        /// <seealso cref=""Rewriter.DescendantsAndSelfInContextLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> DescendantsAndSelfInContextLazy(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelfInContextLazy(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsInContextBreadthFirst{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContextBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContextBreadthFirst(value);

        /// <summary>
        /// <seealso cref=""Rewriter.SelfAndDescendantsInContextBreadthFirstLazy{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContextBreadthFirstLazy(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContextBreadthFirstLazy(value);

        /// <summary>
        /// <seealso cref=""Rewriter.Cursor{{T}}(IRewriter{{T}}, T)""/>
        /// </summary>
        public static Cursor<{typeName}> Cursor(this {typeName} value)
            => {rewriterExpr}.Cursor(value);

        /// <summary>
        /// <seealso cref=""Rewriter.Fold{{T, U}}(IRewriter{{T}}, Func{{T, Children{{U}}, U}}, T)""/>
        /// </summary>
        public static T Fold<T>(this {typeName} value, Func<{typeName}, Children<T>, T> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            return {rewriterExpr}.Fold(func, value);
        }}

        /// <summary>
        /// <seealso cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IEnumerable{{U}}, U}}, T[])""/>
        /// </summary>
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

        /// <summary>
        /// <seealso cref=""Rewriter.ZipFold{{T, U}}(IRewriter{{T}}, Func{{T[], IEnumerable{{U}}, U}}, T[])""/>
        /// </summary>
        public static U ZipFold<U>(this {typeName} value1, {typeName} value2, Func<{typeName}, {typeName}, IEnumerable<U>, U> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold<{typeName}, U>((xs, cs) => func(xs[0], xs[1], cs), new[] {{ value1, value2 }});
        }}

        /// <summary>
        /// <seealso cref=""Rewriter.Rewrite{{T}}(IRewriter{{T}}, Func{{T, T}}, T)""/>
        /// </summary>
        public static {typeName} Rewrite(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.Rewrite(transformer, value);
        }}
        
        /// <summary>
        /// <seealso cref=""IRewriter{{T}}.RewriteChildren(Func{{T, T}}, T)""/>
        /// </summary>
        public static {typeName} RewriteChildren(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteChildren(transformer, value);
        }}
        
        /// <summary>
        /// <seealso cref=""Rewriter.RewriteIter{{T}}(IRewriter{{T}}, Func{{T, IterResult{{T}}}}, T)""/>
        /// </summary>
        public static {typeName} RewriteIter(this {typeName} value, Func<{typeName}, IterResult<{typeName}>> transformer)
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
}
