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
            Node node;
            switch (nodeType)
            {
                case NodeType.Code:
                    node = new CodeNode();
                    break;
                case NodeType.ListNode:
                    node = new ListNode();
                    break;
                case NodeType.UrlNode:
                    node = new UrlNode();
                    break;
                case NodeType.Heading:
                    node = new HeadingNode();
                    break;
                default:
                    node = new Node(nodeType);
                    break;
            }

            node.Content = regExpMatch.Groups[1].Value;
            node.StartPosition = regExpMatch.Index;
            node.EndPosition = node.StartPosition + regExpMatch.Value.Length;
            return node;
        }

    }

    /// <summary>
    /// Parses wiki heading elements:
    ///  == Level 1 header ===
    ///  === Level 2 header ===
    ///  ==== Level 3 header ====
    /// </summary>
    class HeadingParser : RegExpParser
    {
        // only two == are checked
        private const string regExp = "(?:\n|^)(={2,})[ ]+([^=]+)[ ]+={2,}";

        internal HeadingParser() : base(regExp, NodeType.Heading)
        { }

        protected override Node CreateNode(Match regExpMatch)
        {
            var node = (HeadingNode)base.CreateNode(regExpMatch);
            node.Content = regExpMatch.Groups[2].Value;
            // heading level is number of '=' characters - 1
            node.Level = regExpMatch.Groups[1].Value.Length - 1;
            return node;
        }
    }

    /// <summary>
    /// Code block element parser for code wiki elements. E.g.
    /// &lt;code php&gt;
    /// if (empty($this->transaction['transactionProducts'])) {
    ///    $this->transaction['transactionProducts'] = array();
    /// }
    /// &amp;lt;/code&gt;
    /// </summary>
    class CodeBlockParser : RegExpParser
    {
        private const string regExp = "(?:\n|^)<code([^>]*)>(?s:(.*?))<\\/code>";

        internal CodeBlockParser() : base(regExp, NodeType.Code)
        { }

        protected override Node CreateNode(Match regExpMatch)
        {
            var node = (CodeNode)base.CreateNode(regExpMatch);
            node.Content = regExpMatch.Groups[2].Value;
            if (regExpMatch.Groups[1].Success)
            {
                node.LanguageIdentifier = regExpMatch.Groups[1].Value.Trim();
            }
            return node;
        }
    }

}