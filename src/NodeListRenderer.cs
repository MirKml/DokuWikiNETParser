using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DokuWiki
{
    interface IRenderer
    {
        string Render(Node node);
    }

    internal class NodeListRenderer
    {
        private Dictionary<string, IRenderer> renderers;

        internal NodeListRenderer()
        {
            renderers = new Dictionary<string, IRenderer>();
        }

        internal string Render(Node[] nodes)
        {
            //nodes.PrintNodes(true);
            var stringBuilder = new StringBuilder();
            foreach (var node in nodes)
            {
                stringBuilder.Append(GetRenderer(node.Type).Render(node));
            }
            return stringBuilder.ToString();
        }

        private IRenderer GetRenderer(NodeType type)
        {
            IRenderer renderer;
            switch (type)
            {
                case NodeType.BoldText:
                    if (!renderers.TryGetValue("boldText", out renderer))
                    {
                        renderer = renderers["boldText"] = new Renderer.BoldText();
                    }
                    break;

                case NodeType.ItalicText:
                    if (!renderers.TryGetValue("italicText", out renderer))
                    {
                        renderer = renderers["italicText"] = new Renderer.ItalicText();
                    }
                    break;

                case NodeType.PlainText:
                    if (!renderers.TryGetValue("plainText", out renderer))
                    {
                        renderer = renderers["plainText"] = new Renderer.PlainText();
                    }
                    break;

                case NodeType.UrlNode:
                    if (!renderers.TryGetValue("url", out renderer))
                    {
                        renderer = renderers["url"] = new Renderer.Url();
                    }
                    break;

                default:
                    throw new NotSupportedException("rendering for the node type " + type + " isn't supported");
            }
            return renderer;
        }

        internal static string Sanitize(string text)
        {
            return WebUtility.HtmlEncode(text);
        }
    }
}