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
            Console.WriteLine("Munging " + args[1]);
            var sawmillDoc = XDocument.Load(args[0]);
            var docToMunge = XDocument.Load(args[1]);

            var newDoc = docToMunge
                .Element("doc")
                .Rewrite(
                    el =>
                    {
                        var seeAlso = GetSeeAlsoFromMember(el);
                        if (seeAlso == null)
                        {
                            return el;
                        }
                        var replacement = new XElement(el);
                        var newChildren = LookupCref(sawmillDoc, seeAlso.Attribute("cref").Value).ToList();
                        newChildren.Add(seeAlso);  // put the seealso element back
                        replacement.Element("summary").ReplaceWith(newChildren);
                        return replacement;
                    }
                );
            
            docToMunge.Element("doc").ReplaceWith(newDoc);
            docToMunge.Save(args[1]);
        }

        static XElement GetSeeAlsoFromMember(XElement el)
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

            return summary.Element("seealso");
        }

        static IEnumerable<XElement> LookupCref(XDocument sawmillDoc, string cref)
            => sawmillDoc
                .Element("doc")
                .SelfAndDescendants()
                .FirstOrDefault(el => el.Name == "member" && el.Attribute("name")?.Value == cref)
                .Elements()
                .Select(x => new XElement(x));  // clone
    }
}
