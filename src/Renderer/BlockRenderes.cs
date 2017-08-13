using System;
using System.Linq;

namespace DokuWiki.Renderer
{
    /// <summary>
    /// Renders the HTML markup for programing code blocks. It supposes usage of https://highlightjs.org
    /// Javascript highlighter.
    /// </summary>
    class Code : IRenderer
    {
        public string Render(Node node)
        {
            var codeNode = (CodeNode)node;
            return "<pre><code"
                + (!string.IsNullOrEmpty(codeNode.LanguageIdentifier) ? $" class=\"{codeNode.LanguageIdentifier}\">" : ">")
                + codeNode.Content
                + "</code></pre>\n";
        }
    }

    class Heading : IRenderer
    {
        public string Render(Node node)
        {
            var headingNode = (HeadingNode)node;
            return "<h" + headingNode.Level + ">" + headingNode.Content
                + "</h" + headingNode.Level + ">\n";
        }
    }

    /// <summary>
    /// Renders the paragraph block. Paragraph can be composed from other node blocks,
    /// mostly inline text blocks. It renders these blocks recursively.
    /// </summary>
    class Paragraph : IRenderer
    {
        private NodeListRenderer nodeListRenderer;

        internal Paragraph(NodeListRenderer nodeListRenderer)
        {
            this.nodeListRenderer = nodeListRenderer;
        }

        public string Render(Node node)
        {
            string content = node.Nodes.Any()
                ? nodeListRenderer.Render(node.Nodes)
                : NodeListRenderer.Sanitize(node.Content);

            if (!content.EndsWith("\n"))
            {
                content += "\n";
            }
            return "<p>\n" + content + "</p>\n";
        }
    }

}