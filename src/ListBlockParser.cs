using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DokuWiki
{
    class ListBlockParser : RegExpParser
    {
        private const string regExp = "(?:\n|^)([ ]+)[*-] ([^\n]*)";

        internal ListBlockParser() : base(regExp, NodeType.ListNode)
        { }

        protected override Node CreateNode(Match regExpMatch)
        {
            var node = (ListNode)base.CreateNode(regExpMatch);
            node.Content = regExpMatch.Groups[2].Value;
            node.Level = regExpMatch.Groups[1].Value.Length / 2;
            return node;
        }

        /// <summary>
        /// Joins the wiki list item nodes which belong to same block into new one node - item list block.
        /// <returns>List of blocks, each block contains list of items.</returns>
        /// </summary>
        internal static List<Node> JoinListItems(Node[] nodes, string wikiText)
        {
            var nodeList = new List<Node>(nodes);
            nodeList.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));

            var listBlocks = new List<Node>();
            Node currentBlock = new Node(NodeType.ListBlock);
            for (int i = 0; i < nodeList.Count; i++)
            {
                var nodeItem = nodeList[i];
                if (i == 0)
                {
                    // start position is line ending, so add one character
                    currentBlock.StartPosition = nodeItem.StartPosition + 1;
                    currentBlock.EndPosition = nodeItem.EndPosition;
                    currentBlock.Nodes.Add(nodeItem);
                    listBlocks.Add(currentBlock);
                    continue;
                }
                if (i > 0)
                {
                    var previousNodeItem = nodeList[i - 1];
                    if (nodeItem.StartPosition == previousNodeItem.EndPosition)
                    {
                        currentBlock.EndPosition = nodeItem.EndPosition;
                        currentBlock.Nodes.Add(nodeItem);
                    }
                    else
                    {
                        currentBlock = new Node(NodeType.ListBlock);
                        // start position is line ending, so add one character
                        currentBlock.StartPosition = nodeItem.StartPosition + 1;
                        currentBlock.EndPosition = nodeItem.StartPosition;
                        currentBlock.Nodes.Add(nodeItem);
                        listBlocks.Add(currentBlock);
                    }
                }
            }

            // fill the content from main wiki text
            if (listBlocks.Any())
            {
                foreach (var listBlock in listBlocks)
                {

                    listBlock.Content = wikiText.Substring(listBlock.StartPosition, listBlock.EndPosition - listBlock.StartPosition);
                }
            }
            return listBlocks;
        }
    }
}