using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Sawmill.Xml;

namespace Sawmill.Codegen;

internal static class XmlDocGenerator
{
    public static void Go()
    {
        var repoRoot = Path.GetDirectoryName(Path.GetDirectoryName(ThisFile()))!;
        var sawmillDoc = GetSourceXmlDocFile(repoRoot);

        var sourceFiles = Directory
            .EnumerateFiles(repoRoot, "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Contains("Sawmill.Codegen", StringComparison.InvariantCulture));
        foreach (var sourceFile in sourceFiles)
        {
            PasteXmlDocs(sourceFile, sawmillDoc);
        }
    }

    private static readonly Regex _pasteDocRegex = new(@"^(\s*)//!pastedoc\s+(.+)$", RegexOptions.Compiled);
    private static void PasteXmlDocs(string sourceFile, XDocument sawmillDoc)
    {
        var lines = File.ReadAllLines(sourceFile);
        var newLines = new List<string>();

        var skippingDocs = false;
        foreach (var line in lines)
        {
            if (skippingDocs)
            {
                if (line.TrimStart().StartsWith("///", StringComparison.InvariantCulture))
                {
                    continue;
                }
                skippingDocs = false;
            }

            newLines.Add(line);

            var match = _pasteDocRegex.Match(line);
            if (match.Success)
            {
                skippingDocs = true;

                var indentation = match.Groups[1].Value;
                var cref = match.Groups[2].Value;
                var docContent = LookupCref(sawmillDoc, cref);
                if (docContent == null)
                {
                    throw new InvalidOperationException($"Couldn't find cref {cref}");
                }
                docContent.Add("<seealso cref=\"" + cref + "\"/>");
                var docIndentation = docContent[0].Length - docContent[0].TrimStart().Length;
                var newXmlDoc = docContent
                    .Select(l => l.StartsWith(indentation, StringComparison.InvariantCulture) ? l.Remove(0, indentation.Length) : l)
                    .Select(l => indentation + "/// " + l);
                newLines.AddRange(newXmlDoc);
            }
        }

        File.WriteAllLines(sourceFile, newLines);
    }

    private static List<string>? LookupCref(XDocument sawmillDoc, string cref)
        => sawmillDoc
            .Element("doc")
            .SelfAndDescendants()
            .FirstOrDefault(el => el.Name == "member" && el.Attribute("name")?.Value == cref)
            ?.Elements()
            .Where(e => !(e.Name == "param" && e.Attribute("name")?.Value == "rewriter") && e.Name != "typeparam")
            .SelectMany(e => e.ToString().Split(Environment.NewLine))
            .ToList();

    private static XDocument GetSourceXmlDocFile(string repoRoot)
        => XDocument.Load(
            new[] { "Debug", "Release" }
                .Select(f => Path.Combine(repoRoot, "Sawmill", "bin", f, "netstandard2.1", "Sawmill.xml"))
                .Where(File.Exists)
                .OrderByDescending(File.GetLastWriteTime)
                .First()
        );

    private static string ThisFile([CallerFilePath] string? path = null) => path!;
}
