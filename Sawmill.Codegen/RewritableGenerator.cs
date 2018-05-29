using System.IO;

namespace Sawmill.Codegen
{
    static class RewritableGenerator
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
using System.Collections.Immutable;

namespace {ns}
{{
    /// <summary>
    /// Extension methods for <see cref=""{typeName}""/>s.
    /// </summary>
    public static class {className}
    {{
        //!pastedoc M:Sawmill.IRewriter`1.GetChildren(`0)
        public static Children<{typeName}> GetChildren(this {typeName} value)
            => {rewriterExpr}.GetChildren(value);

        //!pastedoc M:Sawmill.IRewriter`1.SetChildren(Sawmill.Children{{`0}},`0)
        public static {typeName} SetChildren(this {typeName} value, Children<{typeName}> newChildren)
            => {rewriterExpr}.SetChildren(newChildren, value);

        //!pastedoc M:Sawmill.Rewriter.DescendantsAndSelf``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<{typeName}> DescendantsAndSelf(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelf(value);
        
        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendants``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<{typeName}> SelfAndDescendants(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendants(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsBreadthFirst``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<{typeName}> SelfAndDescendantsBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsBreadthFirst(value);

        //!pastedoc M:Sawmill.Rewriter.ChildrenInContext``1(Sawmill.IRewriter{{``0}},``0)
        public static Children<({typeName} item, Func<{typeName}, {typeName}> replace)> ChildrenInContext(this {typeName} value)
            => {rewriterExpr}.ChildrenInContext(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsInContext``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContext(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContext(value);

        //!pastedoc M:Sawmill.Rewriter.DescendantsAndSelfInContext``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> DescendantsAndSelfInContext(this {typeName} value)
            => {rewriterExpr}.DescendantsAndSelfInContext(value);

        //!pastedoc M:Sawmill.Rewriter.SelfAndDescendantsInContextBreadthFirst``1(Sawmill.IRewriter{{``0}},``0)
        public static IEnumerable<({typeName} item, Func<{typeName}, {typeName}> replace)> SelfAndDescendantsInContextBreadthFirst(this {typeName} value)
            => {rewriterExpr}.SelfAndDescendantsInContextBreadthFirst(value);
        
        //!pastedoc M:Sawmill.Rewriter.DescendantAt``1(Sawmill.IRewriter{{``0}},System.Collections.Generic.IEnumerable{{Sawmill.Direction}},``0)
        public static {typeName} DescendantAt(this {typeName} value, IEnumerable<Direction> path)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}
            
            return {rewriterExpr}.DescendantAt(path, value);
        }}

        //!pastedoc M:Sawmill.Rewriter.ReplaceDescendantAt``1(Sawmill.IRewriter{{``0}},System.Collections.Generic.IEnumerable{{Sawmill.Direction}},``0,``0)
        public static {typeName} ReplaceDescendantAt<T>(this {typeName} value, IEnumerable<Direction> path, {typeName} newDescendant)
        {{
            if (path == null)
            {{
                throw new ArgumentNullException(nameof(path));
            }}
            
            return {rewriterExpr}.ReplaceDescendantAt(path, newDescendant, value);
        }}

        //!pastedoc M:Sawmill.Rewriter.RewriteDescendantAt``1(Sawmill.IRewriter{{``0}},System.Collections.Generic.IEnumerable{{Sawmill.Direction}},System.Func{{``0,``0}},``0)
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

        //!pastedoc M:Sawmill.Rewriter.Cursor``1(Sawmill.IRewriter{{``0}},``0)
        public static Cursor<{typeName}> Cursor(this {typeName} value)
            => {rewriterExpr}.Cursor(value);

        //!pastedoc M:Sawmill.Rewriter.Fold``2(Sawmill.IRewriter{{``0}},System.Func{{``0,Sawmill.Children{{``1}},``1}},``0)
        public static T Fold<T>(this {typeName} value, Func<{typeName}, Children<T>, T> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}
            return {rewriterExpr}.Fold(func, value);
        }}

        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{{``0}},System.Func{{``0[],System.Collections.Generic.IEnumerable{{``1}},``1}},``0[])
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

        #pragma warning disable CS1734, CS1572, CS1573
        //!pastedoc M:Sawmill.Rewriter.ZipFold``2(Sawmill.IRewriter{{``0}},System.Func{{``0[],System.Collections.Generic.IEnumerable{{``1}},``1}},``0[])
        public static U ZipFold<U>(this {typeName} value1, {typeName} value2, Func<{typeName}, {typeName}, IEnumerable<U>, U> func)
        {{
            if (func == null)
            {{
                throw new ArgumentNullException(nameof(func));
            }}

            return {rewriterExpr}.ZipFold<{typeName}, U>((xs, cs) => func(xs[0], xs[1], cs), new[] {{ value1, value2 }});
        }}
        #pragma warning restore CS1734, CS1572, CS1573 

        //!pastedoc M:Sawmill.Rewriter.Rewrite``1(Sawmill.IRewriter{{``0}},System.Func{{``0,``0}},``0)
        public static {typeName} Rewrite(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.Rewrite(transformer, value);
        }}
        
        //!pastedoc M:Sawmill.IRewriter`1.RewriteChildren(System.Func{{`0,`0}},`0)
        public static {typeName} RewriteChildren(this {typeName} value, Func<{typeName}, {typeName}> transformer)
        {{
            if (transformer == null)
            {{
                throw new ArgumentNullException(nameof(transformer));
            }}
            return {rewriterExpr}.RewriteChildren(transformer, value);
        }}
        
        //!pastedoc M:Sawmill.Rewriter.RewriteIter``1(Sawmill.IRewriter{{``0}},System.Func{{``0,``0}},``0)
        public static {typeName} RewriteIter(this {typeName} value, Func<{typeName}, {typeName}> transformer)
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
