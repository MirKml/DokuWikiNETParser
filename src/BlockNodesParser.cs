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
            // there are two parsers for code nodes
            // - one for "defined" code nodes
            var codeBlockParser = new CodeBlockParser();
            var definedCodeNodes = codeBlockParser.GetNodes(wikiText);
            blockNodes.AddRange(definedCodeNodes);

            // - one for "one line" code nodes
            var oneLineCodeBlockParser = new OneLineCodeBlockParser();
            var oneLineCodeNodes = oneLineCodeBlockParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(definedCodeNodes, n)).ToArray();
            blockNodes.AddRange(oneLineCodeNodes);

            // all code nodes
            var codeNodes = definedCodeNodes.Concat(oneLineCodeNodes).ToArray();

            var headingParser = new HeadingParser();
            var headingNodes = headingParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(codeNodes, n)).ToArray();
            blockNodes.AddRange(headingNodes);

            var listNodeParser = new ListBlockParser();
            var listItemNodes = listNodeParser.GetNodes(wikiText).Where(n => !Node.IsInsideNodes(codeNodes, n)).ToArray();
            var listBlocks = ListBlockParser.JoinListItems(listItemNodes, wikiText);
            blockNodes.AddRange(listBlocks);

            var paragraphParser = new ParagraphParser();
            var paragraphNodes = paragraphParser.GetNodes(wikiText).ToList();
            paragraphNodes = paragraphNodes.Where(n => !Node.IsInsideNodes(codeNodes, n)).ToList();
            paragraphNodes = FilterListParagrahs(paragraphNodes, listBlocks);
            RemoveHeadings(paragraphNodes, headingNodes, wikiText);
            paragraphNodes.AddRange(SplitOnInnerOneLineCodeBlocks(paragraphNodes, oneLineCodeNodes, wikiText));
            blockNodes.AddRange(paragraphNodes);

            blockNodes.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
            return blockNodes.ToArray();
        }

        private static void RemoveHeadings(List<Node> paragraphsNodes, Node[] headingNodes, string wikiText)
        {
            foreach (var paragraphNode in paragraphsNodes)
            {
                foreach (var headingNode in headingNodes)
                {
                    if (headingNode.EndPosition > paragraphNode.StartPosition && headingNode.EndPosition < paragraphNode.EndPosition)
                    {
                        // add '1', because there is one LF after the end of heading and next character is start of the paragraph
                        paragraphNode.StartPosition = headingNode.EndPosition + 1;
                        paragraphNode.Content = wikiText.Substring(paragraphNode.StartPosition, paragraphNode.EndPosition - paragraphNode.StartPosition);
                    }
                }
            }
        }

        private List<Node> SplitOnInnerOneLineCodeBlocks(List<Node> paragraphsNodes, Node[] oneLineCodeBlocks, string wikiText)
        {
            List<Node> newParagrahs = new List<Node>();

            foreach (var paragraphNode in paragraphsNodes)
            {
                foreach (var codeBlock in oneLineCodeBlocks)
                {
                    if (codeBlock.StartPosition > paragraphNode.StartPosition && codeBlock.EndPosition <= paragraphNode.EndPosition)
                    {
                        // When code block is "whole inside" in the paragraph - there is some paragraph content after the code block -
                        // new paragraph node is created.
                        if (codeBlock.EndPosition < paragraphNode.EndPosition)
                        {
                            var newParagraph = new Node(NodeType.ParagraphNode);
                            newParagraph.StartPosition = codeBlock.EndPosition + 1;
                            newParagraph.EndPosition = paragraphNode.EndPosition;
                            newParagraph.Content = wikiText.Substring(newParagraph.StartPosition, newParagraph.EndPosition - newParagraph.StartPosition);
                            newParagrahs.Add(newParagraph);
                        }

                        // current paragraph is shortened before the current code block
                        paragraphNode.EndPosition = codeBlock.StartPosition - 1;
                        paragraphNode.Content = wikiText.Substring(paragraphNode.StartPosition, paragraphNode.EndPosition - paragraphNode.StartPosition);
                    }
                }
            }

            return newParagrahs;
        }

        private static List<Node> FilterListParagrahs(List<Node> paragraphNodes, List<Node> listBlocks)
        {
            var filteredParagraphNodes = paragraphNodes.Where( pn =>
            {
                return !listBlocks.Any(ln => ln.StartPosition >= pn.StartPosition && ln.EndPosition <= pn.EndPosition);
            }).ToList();
            return filteredParagraphNodes;
        }
    }
}