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
            var parsers = new NodeParser[]
            {
                new NoFormattingParser(),
                new UrlNodeParser(),
                new ImageNodeParser(),
                new BoldTextParser(),
                new ItalicTextParser()
            };

            var allNodes = new List<Node>();
            foreach (var parser in parsers)
            {
                allNodes.AddRange(parser.GetNodes(wikiText));
            }

            // there are no other nodes, it means whole text is just one plain text node, which is the Content
            // property of current paragraph node, so we can return empty array
            if (!allNodes.Any())
            {
                return allNodes.ToArray();
            }

            FixNesting(allNodes);

            // any text which isn't in any other node is text in plain text nodes
            var plainTextParser = new PlainTextParser(allNodes);
            var plainTextNodes = plainTextParser.GetNodes(wikiText);
            allNodes.AddRange(plainTextNodes);

            allNodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            return allNodes.ToArray();
        }

        private static void FixNesting(List<Node> nodes)
        {
            // adjust nesting of nodes depends on sorting asc according start position
            nodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            Node previousNode = null;
            foreach (var node in nodes.ToList())
            {
                if (previousNode == null)
                {
                    previousNode = node;
                    continue;
                }
                if (node.StartPosition < previousNode.EndPosition)
                {
                    nodes.Remove(node);
                    ParseNestedContent(previousNode);
                }
            }
        }

        private static void ParseNestedContent(Node node)
        {
            var parsers = new NodeParser[]
            {
                new NoFormattingParser(),
                new BoldTextParser(),
                new ItalicTextParser()
            };

            var nodes = new List<Node>();
            foreach (var parser in parsers)
            {
                nodes.AddRange(parser.GetNodes(node.Content));
            }

            if (!nodes.Any())
            {
                throw new InvalidOperationException("there is no nested nodes, don't call ParseNestedContent method");
            }

            FixNesting(nodes);

            // any text which isn't in any other node is text in plain text nodes
            var plainTextParser = new PlainTextParser(nodes);
            var plainTextNodes = plainTextParser.GetNodes(node.Content);
            nodes.AddRange(plainTextNodes);

            nodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            node.Nodes = nodes;
        }
    }
}
