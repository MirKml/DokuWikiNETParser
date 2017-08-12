using System;
using System.Linq;

namespace DokuWiki.Renderer
{
    class BoldText : IRenderer
    {
        public string Render(Node node)
        {
            string content = node.Nodes.Any()
                ? new NodeListRenderer().Render(node.Nodes.ToArray())
                : node.Content;

            return $"<strong>{NodeListRenderer.Sanitize(content)}</strong>";
        }
    }

    class ItalicText : IRenderer
    {
        public string Render(Node node)
        {
            return $"<em>{NodeListRenderer.Sanitize(node.Content)}</em>";
        }
    }

    class PlainText : IRenderer
    {
        public string Render(Node node)
        {
            return NodeListRenderer.Sanitize(node.Content);
        }
    }

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
                + (string.IsNullOrEmpty(urlNode.Title) ? urlNode.Title : urlNode.Url)
                + "</a>";
        }
    }
}