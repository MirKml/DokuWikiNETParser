using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuWiki
{
    /// Parses content of one block, mostly paragraph. Mostly it creates list of
    /// inline nodes e.g. emphasis text nodes, bold text nodes etc.
    class BlockContentParser : NodeParser
    {
        internal override Node[] GetNodes(string wikiText)
        {
            var allNodes = new List<Node>();

            var noFormattingParser = new NoFormattingParser();
            var noFormattedNodes = noFormattingParser.GetNodes(wikiText);
            allNodes.AddRange(noFormattedNodes);

            var urlLinkParser = new UrlNodeParser();
            var urlLinkNodes = urlLinkParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToArray();
            allNodes.AddRange(urlLinkNodes);

            var imageNodeParser = new ImageNodeParser();
            var imageNodes = imageNodeParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToArray();
            allNodes.AddRange(imageNodes);

            var boldTextParser = new BoldTextParser();
            var boldTextNodes = boldTextParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList();
            allNodes.AddRange(boldTextNodes);

            var italicTextParser = new ItalicTextParser();
            var italicTextNodes = italicTextParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList(); ;
            allNodes.AddRange(italicTextNodes);

            var plainTextParser = new PlainTextParser(allNodes);
            var plainTextNodes = plainTextParser.GetNodes(wikiText);
            allNodes.AddRange(plainTextNodes);
            
            allNodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            return allNodes.ToArray();
        }

        internal static void ParseNestedContent(Node node)
        {
            var boldTextParser = new BoldTextParser();
            var boldTextNodes = boldTextParser.GetNodes(node.Content);
            if (boldTextNodes.Any())
            {
                node.Nodes.AddRange(boldTextNodes);
            }

            var italicTextParser = new ItalicTextParser();
            var italicTextNodes = italicTextParser.GetNodes(node.Content);
            if (boldTextNodes.Any())
            {
                node.Nodes.AddRange(boldTextNodes);
            }
        }

    }
}