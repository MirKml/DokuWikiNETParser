using System;
using System.Linq;

namespace DokuWiki.Renderer
{
    class ListBlock : IRenderer
    {
        private NodeListRenderer nodeListRenderer;

        internal ListBlock(NodeListRenderer nodeListRenderer)
        {
            this.nodeListRenderer = nodeListRenderer;
        }

        public string Render(Node node)
        {
            if (node.Nodes == null || !node.Nodes.Any())
            {
                return string.Empty;
            }

            var content = "<ul>\n";
            var currentLevel = 1;
            var spaceIndent = string.Empty;
            foreach (ListNode listNode in node.Nodes)
            {
                if (listNode.Level > currentLevel)
                {
                    content += spaceIndent + " <ul>\n";
                    spaceIndent += " ";
                }
                else if (listNode.Level < currentLevel)
                {
                    spaceIndent = DecrementSpaceIndent(spaceIndent);
                    content += spaceIndent + " </ul>\n";
                }

                var nodeContent = listNode.Nodes.Any()
                    ? nodeListRenderer.Render(listNode.Nodes)
                    : NodeListRenderer.Sanitize(listNode.Content);

                content += spaceIndent + " <li>" + nodeContent + "</li>\n";
                currentLevel = listNode.Level > 1 ? listNode.Level : 1;
            }

            var unclosedLevels = spaceIndent.Length;
            if (unclosedLevels > 0)
            {
                for (int i = 0; i < unclosedLevels; i++)
                {
                    content += spaceIndent + "</ul>\n";
                    spaceIndent = DecrementSpaceIndent(spaceIndent);
                }
            }

            content += "</ul>\n";
            return content;
        }

        private static string DecrementSpaceIndent(string spaceIndent)
        {
            return spaceIndent.Length > 2
                ? spaceIndent.Substring(0, spaceIndent.Length - 1)
                : string.Empty;
        }
    }
}