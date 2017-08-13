using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuWiki
{
    /// <summary>
    /// Parses content of one block, mostly paragraph. Mostly it creates list of
    /// inline nodes e.g. emphasis text nodes, bold text nodes etc.
    /// </summary>
    class BlockContentParser : NodeParser
    {
        internal override Node[] GetNodes(string wikiText)
        {
            var allNodes = new List<Node>();

            var noFormattingParser = new NoFormattingParser();
            var noFormattedNodes = noFormattingParser.GetNodes(wikiText);
            allNodes.AddRange(noFormattedNodes);

            var urlLinkParser = new UrlNodeParser();
            var urlLinkNodes = urlLinkParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList();
            allNodes.AddRange(urlLinkNodes);

            var imageNodeParser = new ImageNodeParser();
            var imageNodes = imageNodeParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList();
            allNodes.AddRange(imageNodes);

            var boldTextParser = new BoldTextParser();
            var boldTextNodes = boldTextParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList();
            allNodes.AddRange(boldTextNodes);

            var italicTextParser = new ItalicTextParser();
            var italicTextNodes = italicTextParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(allNodes.ToArray(), n)).ToList(); ;
            allNodes.AddRange(italicTextNodes);

            // there are no other nodes, it means whole text is just one plain text node, which is the Content
            // property of current paragraph node, so we can return empty array
            if (!allNodes.Any())
            {
                return allNodes.ToArray();
            }

            // any text which isn't in any other node is text in plain text nodes
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