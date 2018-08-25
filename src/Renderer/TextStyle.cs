using System;
using System.Linq;

namespace DokuWiki.Renderer
{
    class BoldText : IRenderer
    {
        private NodeListRenderer nodeListRenderer;

        internal BoldText(NodeListRenderer nodeListRenderer)
        {
            this.nodeListRenderer = nodeListRenderer;
        }

        public string Render(Node node)
        {
            string content = node.Nodes.Any()
                ? nodeListRenderer.Render(node.Nodes)
                : NodeListRenderer.Sanitize(node.Content);

            return $"<strong>{content}</strong>";
        }
    }

    class ItalicText : IRenderer
    {
        private NodeListRenderer nodeListRenderer;

        internal ItalicText(NodeListRenderer nodeListRenderer)
        {
            this.nodeListRenderer = nodeListRenderer;
        }

        public string Render(Node node)
        {
            string content = node.Nodes.Any()
                ? nodeListRenderer.Render(node.Nodes)
                : NodeListRenderer.Sanitize(node.Content);

            return $"<em>{content}</em>";
        }
    }

    class PlainText : IRenderer
    {
        public string Render(Node node)
        {
            return NodeListRenderer.Sanitize(node.Content);
        }
    }

    /// <summary>
    /// Renders inline text without any formatting. For backward compatibility, it renders
    /// the text inside "code" element.
    /// </summary>
    class InlineNoFormat : IRenderer
    {
        public string Render(Node node)
        {
            return $"<code>{NodeListRenderer.Sanitize(node.Content)}</code>";
        }
    }

    class Url : IRenderer
    {
        public string Render(Node node)
        {
            var urlNode = (UrlNode)node;
            return $"<a href=\"{urlNode.Url}\">"
                + (string.IsNullOrEmpty(urlNode.Title) ? urlNode.Url : urlNode.Title)
                + "</a>";
        }
    }
}