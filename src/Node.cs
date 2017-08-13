using System.Collections.Generic;

namespace DokuWiki
{
    class Node
    {
        public Node(NodeType type)
        {
            Type = type;
            Nodes = new List<Node>();
        }

        /// <summary>
        /// zero-based start position of the node in parent node content
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// zero-based end position of the node in parent node content
        /// </summary>
        public int EndPosition { get; set; }

        /// <summary>
        /// Content of the node without boundary wiki marks.
        /// It can be wiki markup, which can be parsed or final text, which will be rendered
        /// during conversion phase.
        /// </summary>
        public string Content { get; set; }

        public NodeType Type { get; private set; }

        /// <summary>
        /// list of nested nodes
        /// </summary>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Checks if particular node is "inside" any of node list
        /// It's in sense of position in text.
        /// </summary>
        internal static bool IsInsideNodes(Node[] nodes, Node node)
        {
            foreach (var itemNode in nodes)
            {
                if (itemNode != node
                    && (node.StartPosition >= itemNode.StartPosition && node.EndPosition <= itemNode.EndPosition))
                {
                    return true;
                }
            }
            return false;
        }
    }

    class CodeNode : Node
    {
        public CodeNode() : base(NodeType.Code)
        { }

        /// <summary>
        /// Determines the code language e.g. PHP, C++, etc.
        /// </summary>
        public string LanguageIdentifier { get; set; }
    }

    class ListNode : Node
    {
        public ListNode() : base(NodeType.ListNode)
        { }

        public bool IsOrdered { get; set; }
        public int Level { get; set; }
    }

    class UrlNode : Node
    {
        public UrlNode() : base(NodeType.UrlNode)
        { }

        public string Url { get; set; }
        public string Title { get; set; }
    }

    class HeadingNode : Node
    {
        public HeadingNode() : base(NodeType.Heading)
        { }

        public int Level { get; set;}
    }

    enum NodeType
    {
        Heading,
        PlainText,
        Code,
        /// <summary>one item in <see cref="NodeType.ListBlock"/> block</summary>
        ListNode,
        /// <summary>List of <see cref="NodeType.ListNode"/> items</summary>
        ListBlock,
        ParagraphNode,
        BoldText,
        ItalicText,
        UnderlineText,
        InlineNoFormat,
        UrlNode,
        ImageNode
    }
}