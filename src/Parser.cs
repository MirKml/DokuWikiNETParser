using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DokuWiki
{
    class Parser
    {
        internal Node[] Parse(string wikiText)
        {
            var blockNodesParser = new BlockNodesParser();
            var blockNodes = blockNodesParser.GetNodes(wikiText);
            
            // there aren't any blocks, consider whole text as one block and return it's nodes
            if (!blockNodes.Any() && !string.IsNullOrEmpty(wikiText))
            {
                var blockContentParser = new BlockContentParser();
                return blockContentParser.GetNodes(wikiText);
            }

            foreach (var paragraphNode in blockNodes.Where(n => n.Type == NodeType.ParagraphNode))
            {
                var blockContentParser = new BlockContentParser();
                var nodes = blockContentParser.GetNodes(paragraphNode.Content);
                paragraphNode.Nodes.AddRange(nodes);
            }

            return blockNodes; 
        }
    }

    abstract class NodeParser
    {
        internal abstract Node[] GetNodes(string wikiText);
    }

    abstract class RegExpParser : NodeParser
    {
        private string regExp;
        private NodeType nodeType;

        public RegExpParser(string regExp, NodeType type)
        {
            this.regExp = regExp;
            this.nodeType = type;
        }

        internal override Node[] GetNodes(string wikiText)
        {
            var matches = Regex.Matches(wikiText, regExp);
            if (matches.Count == 0) return new Node[0];

            var nodes = new Node[matches.Count];
            int index = 0;
            foreach (Match match in matches)
            {
                nodes[index++] = CreateNode(match);
            }

            return nodes;
        }

        protected virtual Node CreateNode(Match regExpMatch)
        {
            var node = new Node(nodeType);
            node.Content = regExpMatch.Groups[1].Value;
            node.StartPosition = regExpMatch.Index;
            node.EndPosition = node.StartPosition + regExpMatch.Value.Length - 1;
            return node;
        }
    }

    /// Heading element
    /// == Leve 1 header ===
    /// === Lever 2 header ===
    /// ==== Level 3 header ==== 
    class HeadingParser : RegExpParser
    {
        // only two == are checked
        private const string regExp = "\n={2,}[ ]+([^=]+)[ ]+={2,}";

        internal HeadingParser() : base(regExp, NodeType.Heading)
        { }
    }

    ///
    /// Code block element
    /// <code php>
    /// if (empty($this->transaction['transactionProducts'])) {
    ///    $this->transaction['transactionProducts'] = array();
    /// }
    /// </code>
    class CodeBlockParser : RegExpParser
    {
        private const string regExp = "\n<code([^>]*)>(?s:(.*?))<\\/code>";

        internal CodeBlockParser() : base(regExp, NodeType.Code)
        { }

        protected override Node CreateNode(Match regExpMatch)
        {
            var node = base.CreateNode(regExpMatch);
            node.Content = regExpMatch.Groups[2].Value;
            return node;
        }
    }

}