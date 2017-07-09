using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuWiki
{
    class BlockNodesParser: NodeParser
    {
        internal override Node[] GetNodes(string wikiText)
        {
            var blockNodes = new List<Node>();

            // code block nodes first, because these blocks are final, these mustn't have
            // any other inner node inside
            var codeBlockParser = new CodeBlockParser();
            var codeNodes = codeBlockParser.GetNodes(wikiText);
            blockNodes.AddRange(codeNodes);

            var headingParser = new HeadingParser();
            var headingNodes = headingParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(codeNodes, n)).ToArray();
            blockNodes.AddRange(headingNodes);

            var listNodeParser = new ListBlockParser();
            var listItemNodes = listNodeParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(codeNodes, n)).ToArray();
            var listBlocks = ListBlockParser.JoinListItems(listItemNodes, wikiText);
            blockNodes.AddRange(listBlocks);

            var paragraphParser = new ParagraphParser();
            var paragraphNodes = paragraphParser.GetNodes(wikiText);
            paragraphNodes = paragraphNodes.Where(n => !Node.IsInsideNodes(codeNodes, n)).ToArray(); 
            paragraphNodes = FilterListParagrahs(paragraphNodes, listBlocks); 
            RemoveHeadings(paragraphNodes, headingNodes, wikiText);
            blockNodes.AddRange(paragraphNodes);

            blockNodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            return blockNodes.ToArray();
        }

        private static void RemoveHeadings(Node[] paragraphsNodes, Node[] headingNodes, string wikiText)
        {
            foreach (var paragraphNode in paragraphsNodes)
            {
                foreach (var headingNode in headingNodes)
                {
                    if (headingNode.EndPosition > paragraphNode.StartPosition && headingNode.EndPosition < paragraphNode.EndPosition)
                    {
                        // add '2', because there is one LF after the end of heading and next character is start of the paragraph
                        paragraphNode.StartPosition = headingNode.EndPosition + 2;
                        paragraphNode.Content = wikiText.Substring(paragraphNode.StartPosition, paragraphNode.EndPosition - paragraphNode.StartPosition);
                    }
                }
            }
        }

        private static Node[] FilterListParagrahs(Node[] paragraphNodes, List<Node> listBlocks)
        {
            var filteredParagraphNodes = paragraphNodes.Where( pn => 
            {
                return !listBlocks.Any(ln => ln.StartPosition >= pn.StartPosition && ln.EndPosition <= pn.EndPosition);
            }).ToArray();
            return filteredParagraphNodes;
        }
    }
}