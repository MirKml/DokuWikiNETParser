
using System;
using System.Collections.Generic;

namespace DokuWiki
{
    class PlainTextParser : NodeParser
    {
        private readonly IList<Node> currentNodes;

        internal PlainTextParser(IList<Node> currentNodes)
        {
            this.currentNodes = currentNodes;
        }

        internal override Node[] GetNodes(string wikiText)
        {
            var plainTextNodes = new List<Node>();

            var textPosition = 0;
            foreach (var node in currentNodes)
            {
                if (node.StartPosition > textPosition)
                {
                    var plainTextNode = new Node(NodeType.PlainText);
                    plainTextNode.StartPosition = textPosition;
                    plainTextNode.EndPosition = node.StartPosition - 1;
                    plainTextNode.Content = wikiText.Substring(plainTextNode.StartPosition, plainTextNode.EndPosition - plainTextNode.StartPosition + 1);
                    plainTextNodes.Add(plainTextNode);
                }
                if (node.StartPosition < textPosition)
                {
                    throw new IndexOutOfRangeException("plain text parsing: start index of current node is smaller then current position");
                }
                textPosition = node.EndPosition + 1;
            }

            if (textPosition < wikiText.Length - 1)
            {
                var plainTextNode = new Node(NodeType.PlainText);
                plainTextNode.StartPosition = textPosition;
                plainTextNode.EndPosition = wikiText.Length - 1;
                plainTextNode.Content = wikiText.Substring(plainTextNode.StartPosition);
            }

            return plainTextNodes.ToArray();
        }
    }
}