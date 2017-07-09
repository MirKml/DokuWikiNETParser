using System;
using System.Collections.Generic;
using System.Linq;

namespace DokuWiki
{
    internal class ParagraphParser: NodeParser
    {
        internal override Node[] GetNodes(string wikiText)
        {
            if (!AreAnyParagraphs(wikiText))
            {
                return new Node[0];
            }

            int firstIndex = 0, nextIndex = 0;
            var textLength = wikiText.Length; 
            var blockNodes = new List<Node>();
            while (firstIndex < textLength && (nextIndex = wikiText.IndexOf("\n\n", firstIndex)) != -1)
            {
                // it means more LF then 2 
                if (firstIndex == nextIndex)
                {
                    firstIndex++;
                    continue;
                }
                var node = new Node(NodeType.ParagraphNode);
                blockNodes.Add(CreateNode(firstIndex, nextIndex, wikiText));
                firstIndex = nextIndex + 1;
            }

            // last block
            if (firstIndex < textLength)
            {
                var lastBlock = CreateNode(firstIndex, textLength, wikiText);
                blockNodes.Add(lastBlock);
            }
            return blockNodes.Where(n => !string.IsNullOrWhiteSpace(n.Content)).ToArray();
        }

        /// <summary>
        /// Are any paragraphs in whole wiki text?
        /// </summary>
        private static bool AreAnyParagraphs(string wikiText)
        {
            return wikiText.IndexOf("\n\n") != -1;
        }

        private static Node CreateNode(int firstIndex, int nextIndex, string wikiText)
        {
            var node = new Node(NodeType.ParagraphNode);
            node.StartPosition = firstIndex == 0 ? 0 : firstIndex + 1;
            node.EndPosition = nextIndex;
            node.Content = wikiText.Substring(node.StartPosition, node.EndPosition - node.StartPosition);
            return node;
        }
    }
}