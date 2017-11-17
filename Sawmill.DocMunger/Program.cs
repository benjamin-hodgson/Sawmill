using System;
using System.Linq;
using System.Xml.Linq;

namespace Sawmill.DocMunger
{
    using System.Collections.Generic;
    using Sawmill.Xml;

    class Program
    {
        static void Main(string[] args)
        {
            var sawmillDoc = XDocument.Load(args[0]);
            var docToMunge = XDocument.Load(args[1]);

            var newDoc = docToMunge
                .Element("doc")
                .Rewrite(
                    el =>
                    {
                        var cref = GetSeeAlsoCrefFromMember(el);
                        if (cref == null)
                        {
                            return el;
                        }
                        var replacement = new XElement(el);
                        replacement.Element("summary").AddFirst(LookupCref(sawmillDoc, cref));
                        return replacement;
                    }
                );
            
            docToMunge.Element("doc").ReplaceWith(newDoc);
            docToMunge.Save(args[1]);
        }

        static string GetSeeAlsoCrefFromMember(XElement el)
        {
            if (el.Name != "member" || el.Elements().Count() != 1)
            {
                return null;
            }

            var summary = el.Element("summary");
            if (summary == null || summary.Elements().Count() != 1)
            {
                return null;
            }

            return summary.Element("seealso")?.Attribute("cref")?.Value;
        }

        static IEnumerable<XElement> LookupCref(XDocument sawmillDoc, string cref)
            => sawmillDoc
                .Element("doc")
                .SelfAndDescendants()
                .FirstOrDefault(el => el.Name == "member" && el.Attribute("name")?.Value == cref)
                .Elements();
    }
}
