using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Sawmill.Analyzers
{
    /// <inheritdoc/>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RewriterRefactoringProvider)), Shared]
    public class RewriterRefactoringProvider : CodeRefactoringProvider
    {
        /// <inheritdoc/>
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var classDecl = root.FindNode(context.Span) as ClassDeclarationSyntax;
            if (classDecl == null)
            {
                // not looking at a class declaration
                return;
            }


            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken);
            var iRewriter = semanticModel.Compilation.GetTypeByMetadataName("Sawmill.IRewriter`1");
            if (iRewriter == null)
            {
                return;
            }

            var rewriterClass = semanticModel.GetDeclaredSymbol(classDecl);

            var iRewriterImplementation = rewriterClass.Interfaces.FirstOrDefault(ifce => ifce.OriginalDefinition.Equals(iRewriter));
            if (iRewriterImplementation == null)
            {
                // class does not implement IRewriter
                return;
            }

            var iRewriterArgument = iRewriterImplementation.TypeArguments.SingleOrDefault() as INamedTypeSymbol;
            if (iRewriterArgument == null)
            {
                return;
            }

            context.RegisterRefactoring(CodeAction.Create(
                "Generate rewriter extension methods",
                cancellationToken => new ExtensionMethodGenerator(context.Document, rewriterClass, iRewriterArgument).GenerateExtensionMethodsDocument(cancellationToken)
            ));
        }

        class ExtensionMethodGenerator
        {
            private readonly Document _document;
            private readonly INamedTypeSymbol _rewriterSymbol;
            private readonly INamedTypeSymbol _rewritableSymbol;
            private readonly TypeSyntax _rewritable;
            private readonly TypeSyntax _funcTT;
            private readonly TypeSyntax _childrenT;
            private readonly TypeSyntax _iEnumerableT;
            private readonly TypeSyntax _childrenContext;
            private readonly TypeSyntax _iEnumerableContext;
            private readonly TypeSyntax _cursorT;

            public ExtensionMethodGenerator(
                Document document,
                INamedTypeSymbol rewriter,
                INamedTypeSymbol rewritable
            )
            {
                _document = document;
                _rewriterSymbol = rewriter;
                _rewritableSymbol = rewritable;

                _rewritable = IdentifierName(_rewritableSymbol.Name);

                _funcTT = GenericName(
                    Identifier("Func"),
                    TypeArgumentList(SeparatedList(new[] { _rewritable, _rewritable }))
                );
                _childrenT = GenericName(
                    Identifier("Children"),
                    TypeArgumentList(SingletonSeparatedList(_rewritable))
                );
                _iEnumerableT = GenericName(
                    Identifier("IEnumerable"),
                    TypeArgumentList(SingletonSeparatedList(_rewritable))
                );
                TypeSyntax context = TupleType(SeparatedList(
                    new[]
                    {
                        TupleElement(_rewritable, Identifier("item")),
                        TupleElement(_funcTT, Identifier("context"))
                    }
                ));
                _childrenContext = GenericName(
                    Identifier("Children"),
                    TypeArgumentList(SingletonSeparatedList(context))
                );
                _iEnumerableContext = GenericName(
                    Identifier("IEnumerable"),
                    TypeArgumentList(SingletonSeparatedList(context))
                );
                _cursorT = GenericName(
                    Identifier("Cursor"),
                    TypeArgumentList(SingletonSeparatedList(_rewritable))
                );
            }

            public Task<Solution> GenerateExtensionMethodsDocument(CancellationToken cancellationToken)
            {
                var accessibility = _rewritableSymbol.DeclaredAccessibility == Accessibility.Public
                    ? Token(SyntaxKind.PublicKeyword)
                    : Token(SyntaxKind.InternalKeyword);

                var extensionMethodsClass = ClassDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(accessibility)
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    identifier: Identifier($"{_rewritableSymbol.Name}RewriterExtensions"),
                    typeParameterList: null,
                    baseList: null,
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    members: ExtensionMethods()
                );

                var compilationUnit = CompilationUnit(
                    externs: List<ExternAliasDirectiveSyntax>(),
                    usings: List(
                        new[]
                        {
                            UsingDirective(IdentifierName("System")),
                            UsingDirective(QualifiedName(QualifiedName(IdentifierName("System"), IdentifierName("Collections")), IdentifierName("Generic"))),
                            UsingDirective(IdentifierName("Sawmill")),
                            _rewritableSymbol.ContainingNamespace != null
                                ? UsingDirective(NamespaceName(_rewritableSymbol.ContainingNamespace))
                                : null
                        }.Where(x => x != null)
                    ),
                    attributeLists: List<AttributeListSyntax>(),
                    members: List(
                        new[]
                        {
                            _rewriterSymbol.ContainingNamespace != null
                                ? NamespaceDeclaration(
                                    NamespaceName(_rewriterSymbol.ContainingNamespace),
                                    externs: List<ExternAliasDirectiveSyntax>(),
                                    usings: List<UsingDirectiveSyntax>(),
                                    members: List<MemberDeclarationSyntax>(new[] { extensionMethodsClass })
                                )
                                : (MemberDeclarationSyntax)extensionMethodsClass
                        }
                    )
                );

                var newDocument = _document.Project.AddDocument(
                    $"{_rewritableSymbol.Name}RewriterExtensions.cs",
                    compilationUnit.WithAdditionalAnnotations(Formatter.Annotation),
                    _document.Folders
                );
                return Task.FromResult(newDocument.Project.Solution);
            }

            private SyntaxList<MemberDeclarationSyntax> ExtensionMethods()
                => List<MemberDeclarationSyntax>(
                    new[]
                    {
                        QueryMethod("GetChildren", _childrenT),
                        RewriteMethod("SetChildren", _childrenT, "newChildren", nullCheck: false),
                        QueryMethod("SelfAndDescendants", _iEnumerableT),
                        QueryMethod("DescendantsAndSelf", _iEnumerableT),
                        QueryMethod("SelfAndDescendantsBreadthFirst", _iEnumerableT),
                        QueryMethod("ChildrenInContext", _childrenContext),
                        QueryMethod("SelfAndDescendantsInContext", _iEnumerableContext),
                        QueryMethod("DescendantsAndSelfInContext", _iEnumerableContext),
                        QueryMethod("SelfAndDescendantsInContextBreadthFirst", _iEnumerableContext),
                        QueryMethod("Cursor", _cursorT),
                        FoldMethod(),
                        BinaryZipFoldMethod(),
                        NAryZipFoldMethod(),
                        RewriteMethod("Rewrite", _funcTT, "transformer"),
                        RewriteMethod("RewriteChildren", _funcTT, "transformer"),
                        RewriteMethod("RewriteIter", _funcTT, "transformer")
                    }
                );

            private MethodDeclarationSyntax QueryMethod(string methodName, TypeSyntax returnType)
            {
                const string valueParamName = "value";
                var bodyExpression = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_rewriterSymbol.Name),
                            IdentifierName("Instance")
                        ),
                        IdentifierName(methodName)
                    ),
                    ArgumentList(SingletonSeparatedList(Argument(IdentifierName(valueParamName))))
                );
                return MethodDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(Token(SyntaxKind.PublicKeyword))
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    returnType: returnType,
                    explicitInterfaceSpecifier: null,
                    identifier: Identifier(methodName),
                    typeParameterList: null,
                    parameterList: ParameterList(SingletonSeparatedList(Parameter(
                        attributeLists: List<AttributeListSyntax>(),
                        modifiers: SyntaxTokenList.Create(Token(SyntaxKind.ThisKeyword)),
                        type: _rewritable,
                        identifier: Identifier(valueParamName),
                        @default: null
                    ))),
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    body: Block(ReturnStatement(bodyExpression)),
                    expressionBody: null
                );
            }

            private MethodDeclarationSyntax RewriteMethod(string methodName, TypeSyntax argumentType, string paramName, bool nullCheck = true)
            {
                const string valueParamName = "value";
                var bodyExpression = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_rewriterSymbol.Name),
                            IdentifierName("Instance")
                        ),
                        IdentifierName(methodName)
                    ),
                    ArgumentList(SeparatedList(new[] { Argument(IdentifierName(paramName)), Argument(IdentifierName(valueParamName)) }))
                );

                var body = nullCheck
                    ? Block(
                        IfStatement(
                            BinaryExpression(SyntaxKind.EqualsExpression, IdentifierName(paramName), LiteralExpression(SyntaxKind.NullLiteralExpression)),
                            Block(ThrowStatement(ObjectCreationExpression(
                                IdentifierName("ArgumentNullException"),
                                ArgumentList(SingletonSeparatedList(Argument(
                                    InvocationExpression(IdentifierName("nameof"), ArgumentList(SingletonSeparatedList(Argument(IdentifierName(paramName)))))
                                ))),
                                null
                            )))
                        ),
                        ReturnStatement(bodyExpression)
                    )
                    : Block(ReturnStatement(bodyExpression));

                return MethodDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(Token(SyntaxKind.PublicKeyword))
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    returnType: _rewritable,
                    explicitInterfaceSpecifier: null,
                    identifier: Identifier(methodName),
                    typeParameterList: null,
                    parameterList: ParameterList(SeparatedList(
                        new[]
                        {
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: SyntaxTokenList.Create(Token(SyntaxKind.ThisKeyword)),
                                type: _rewritable,
                                identifier: Identifier(valueParamName),
                                @default: null
                            ),
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: new SyntaxTokenList(),
                                type: argumentType,
                                identifier: Identifier(paramName),
                                @default: null
                            )
                        }
                    )),
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    body: body,
                    expressionBody: null
                );
            }

            private MethodDeclarationSyntax FoldMethod()
            {
                const string methodName = "Fold";
                const string valueParamName = "value";
                const string funcParamName = "func";
                var u = IdentifierName("U");
                var childrenU = GenericName(Identifier("Children"), TypeArgumentList(SingletonSeparatedList<TypeSyntax>(u)));

                var bodyExpression = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_rewriterSymbol.Name),
                            IdentifierName("Instance")
                        ),
                        IdentifierName(methodName)
                    ),
                    ArgumentList(SeparatedList(new[] { Argument(IdentifierName(funcParamName)), Argument(IdentifierName(valueParamName)) }))
                );

                var body = Block(
                    IfStatement(
                        BinaryExpression(SyntaxKind.EqualsExpression, IdentifierName(funcParamName), LiteralExpression(SyntaxKind.NullLiteralExpression)),
                        Block(ThrowStatement(ObjectCreationExpression(
                            IdentifierName("ArgumentNullException"),
                            ArgumentList(SingletonSeparatedList(Argument(
                                InvocationExpression(IdentifierName("nameof"), ArgumentList(SingletonSeparatedList(Argument(IdentifierName(funcParamName)))))
                            ))),
                            null
                        )))
                    ),
                    ReturnStatement(bodyExpression)
                );

                return MethodDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(Token(SyntaxKind.PublicKeyword))
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    returnType: u,
                    explicitInterfaceSpecifier: null,
                    identifier: Identifier(methodName),
                    typeParameterList: TypeParameterList(SingletonSeparatedList(TypeParameter("U"))),
                    parameterList: ParameterList(SeparatedList(
                        new[]
                        {
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: SyntaxTokenList.Create(Token(SyntaxKind.ThisKeyword)),
                                type: _rewritable,
                                identifier: Identifier(valueParamName),
                                @default: null
                            ),
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: new SyntaxTokenList(),
                                type: GenericName(Identifier("Func"), TypeArgumentList(SeparatedList(new[] { _rewritable, childrenU, u }))),
                                identifier: Identifier(funcParamName),
                                @default: null
                            )
                        }
                    )),
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    body: body,
                    expressionBody: null
                );
            }

            private MethodDeclarationSyntax BinaryZipFoldMethod()
            {
                const string methodName = "ZipFold";
                const string value1ParamName = "value1";
                const string value2ParamName = "value2";
                const string funcParamName = "func";
                var u = IdentifierName("U");
                var iEnumerableU = GenericName(Identifier("IEnumerable"), TypeArgumentList(SingletonSeparatedList<TypeSyntax>(u)));

                const string xs = "xs";
                const string cs = "cs";
                var wrapperFunc = ParenthesizedLambdaExpression(
                    ParameterList(SeparatedList(new[] { Parameter(Identifier(xs)), Parameter(Identifier(cs)) })),
                    InvocationExpression(
                        IdentifierName(funcParamName),
                        ArgumentList(SeparatedList(
                            new[]
                            {
                                Argument(ElementAccessExpression(
                                    IdentifierName(xs),
                                    BracketedArgumentList(SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(0)))))
                                )),
                                Argument(ElementAccessExpression(
                                    IdentifierName(xs),
                                    BracketedArgumentList(SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1)))))
                                )),
                                Argument(IdentifierName(cs))
                            }
                        ))
                    )
                );
                var bodyExpression = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_rewriterSymbol.Name),
                            IdentifierName("Instance")
                        ),
                        GenericName(Identifier(methodName), TypeArgumentList(SeparatedList(new[] { _rewritable, u })))
                    ),
                    ArgumentList(SeparatedList(new[] { Argument(wrapperFunc), Argument(IdentifierName(value1ParamName)), Argument(IdentifierName(value2ParamName)) }))
                );

                var body = Block(
                    IfStatement(
                        BinaryExpression(SyntaxKind.EqualsExpression, IdentifierName(funcParamName), LiteralExpression(SyntaxKind.NullLiteralExpression)),
                        Block(ThrowStatement(ObjectCreationExpression(
                            IdentifierName("ArgumentNullException"),
                            ArgumentList(SingletonSeparatedList(Argument(
                                InvocationExpression(IdentifierName("nameof"), ArgumentList(SingletonSeparatedList(Argument(IdentifierName(funcParamName)))))
                            ))),
                            null
                        )))
                    ),
                    ReturnStatement(bodyExpression)
                );

                return MethodDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(Token(SyntaxKind.PublicKeyword))
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    returnType: u,
                    explicitInterfaceSpecifier: null,
                    identifier: Identifier(methodName),
                    typeParameterList: TypeParameterList(SingletonSeparatedList(TypeParameter("U"))),
                    parameterList: ParameterList(SeparatedList(
                        new[]
                        {
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: SyntaxTokenList.Create(Token(SyntaxKind.ThisKeyword)),
                                type: _rewritable,
                                identifier: Identifier(value1ParamName),
                                @default: null
                            ),
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: new SyntaxTokenList(),
                                type: _rewritable,
                                identifier: Identifier(value2ParamName),
                                @default: null
                            ),
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: new SyntaxTokenList(),
                                type: GenericName(Identifier("Func"), TypeArgumentList(SeparatedList(new[] { _rewritable, _rewritable, iEnumerableU, u }))),
                                identifier: Identifier(funcParamName),
                                @default: null
                            )
                        }
                    )),
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    body: body,
                    expressionBody: null
                );
            }

            private MethodDeclarationSyntax NAryZipFoldMethod()
            {
                const string methodName = "ZipFold";
                const string valuesParamName = "values";
                const string funcParamName = "func";
                var u = IdentifierName("U");
                var iEnumerableU = GenericName(Identifier("IEnumerable"), TypeArgumentList(SingletonSeparatedList<TypeSyntax>(u)));
                var tArray = ArrayType(_rewritable, SingletonList(ArrayRankSpecifier()));

                var bodyExpression = InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(_rewriterSymbol.Name),
                            IdentifierName("Instance")
                        ),
                        IdentifierName(methodName)
                    ),
                    ArgumentList(SeparatedList(new[] { Argument(IdentifierName(funcParamName)), Argument(IdentifierName(valuesParamName)) }))
                );

                var body = Block(
                    IfStatement(
                        BinaryExpression(SyntaxKind.EqualsExpression, IdentifierName(funcParamName), LiteralExpression(SyntaxKind.NullLiteralExpression)),
                        Block(ThrowStatement(ObjectCreationExpression(
                            IdentifierName("ArgumentNullException"),
                            ArgumentList(SingletonSeparatedList(Argument(
                                InvocationExpression(IdentifierName("nameof"), ArgumentList(SingletonSeparatedList(Argument(IdentifierName(funcParamName)))))
                            ))),
                            null
                        )))
                    ),
                    ReturnStatement(bodyExpression)
                );

                return MethodDeclaration(
                    attributeLists: List<AttributeListSyntax>(),
                    modifiers: SyntaxTokenList
                        .Create(Token(SyntaxKind.PublicKeyword))
                        .Add(Token(SyntaxKind.StaticKeyword)),
                    returnType: u,
                    explicitInterfaceSpecifier: null,
                    identifier: Identifier(methodName),
                    typeParameterList: TypeParameterList(SingletonSeparatedList(TypeParameter("U"))),
                    parameterList: ParameterList(SeparatedList(
                        new[]
                        {
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: SyntaxTokenList.Create(Token(SyntaxKind.ThisKeyword)),
                                type: tArray,
                                identifier: Identifier(valuesParamName),
                                @default: null
                            ),
                            Parameter(
                                attributeLists: List<AttributeListSyntax>(),
                                modifiers: new SyntaxTokenList(),
                                type: GenericName(Identifier("Func"), TypeArgumentList(SeparatedList(new TypeSyntax[] { tArray, iEnumerableU, u }))),
                                identifier: Identifier(funcParamName),
                                @default: null
                            )
                        }
                    )),
                    constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                    body: body,
                    expressionBody: null
                );
            }

            private static NameSyntax NamespaceName(INamespaceSymbol ns)
                => ns.ToDisplayString().Split('.').Aggregate(
                    null as NameSyntax,
                    (n, p) => n == null
                        ? (NameSyntax)IdentifierName(p)
                        : QualifiedName(n, IdentifierName(p))
                );
        }
    }
}