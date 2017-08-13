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
        public string Render(Node node)
        {
            if (node.Nodes.Any())
            {
                var nodeListRenderer = new NodeListRenderer();
                var nodeListContent = nodeListRenderer.Render(node.Nodes.ToArray());
                if (!nodeListContent.EndsWith("\n"))
                {
                    nodeListContent += "\n";
                }
                return "<p>\n" + nodeListContent + "</p>\n";
            }

            var nodeContent = node.Content;
            if (!nodeContent.EndsWith("\n"))
            {
                nodeContent += "\n";
            }
            return "<p>\n" + nodeContent + "</p>\n";
        }
    }

}