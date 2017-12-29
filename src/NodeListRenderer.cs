using System;
using System.Collections.Generic;
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

        public string ImagesBaseUrl { private get; set; }
        public string ImageLeftAlignCssClass { private get; set; }
        public string ImageRightAlignCssClass { private get; set; }
        public string ImageCenterAlignCssClass { private get; set; }

        internal string Render(Node[] nodes)
        {
            // for debug purposes:
            // nodes.PrintNodes(true);
            var stringBuilder = new StringBuilder();
            foreach (var node in nodes)
            {
                stringBuilder.Append(GetRenderer(node.Type).Render(node));
            }
            return stringBuilder.ToString();
        }

        internal string Render(List<Node> nodes)
        {
            return Render(nodes.ToArray());
        }

        private IRenderer GetRenderer(NodeType type)
        {
            IRenderer renderer;
            switch (type)
            {
                case NodeType.BoldText:
                    if (!renderers.TryGetValue("boldText", out renderer))
                    {
                        renderer = renderers["boldText"] = new Renderer.BoldText(this);
                    }
                    break;

                case NodeType.ItalicText:
                    if (!renderers.TryGetValue("italicText", out renderer))
                    {
                        renderer = renderers["italicText"] = new Renderer.ItalicText(this);
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

                case NodeType.Code:
                    if (!renderers.TryGetValue("code", out renderer))
                    {
                        renderer = renderers["code"] = new Renderer.Code();
                    }
                    break;

                case NodeType.Heading:
                    if (!renderers.TryGetValue("heading", out renderer))
                    {
                        renderer = renderers["heading"] = new Renderer.Heading();
                    }
                    break;

                case NodeType.ParagraphNode:
                    if (!renderers.TryGetValue("paragraph", out renderer))
                    {
                        renderer = renderers["paragraph"] = new Renderer.Paragraph(this);
                    }
                    break;

                case NodeType.ListBlock:
                    if (!renderers.TryGetValue("listBlock", out renderer))
                    {
                        renderer = renderers["listBlock"] = new Renderer.ListBlock(this);
                    }
                    break;

                case NodeType.ImageNode:
                    if (!renderers.TryGetValue("image", out renderer))
                    {
                        var imageRenderer = new Renderer.Image(ImagesBaseUrl);
                        imageRenderer.LeftAlignCssClass = ImageLeftAlignCssClass;
                        imageRenderer.RightAlignCssClass = ImageRightAlignCssClass;
                        imageRenderer.CenterAlignCssClass = ImageCenterAlignCssClass;
                        renderer = renderers["image"] = (IRenderer)imageRenderer;
                    }
                    break;

                default:
                    throw new NotSupportedException("rendering for the node type " + type + " isn't supported");
            }
            return renderer;
        }

        internal static string Sanitize(string text)
        {
            var stringBuilder = new StringBuilder(text);
            return stringBuilder
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .ToString();
        }
    }
}