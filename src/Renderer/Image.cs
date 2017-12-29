namespace DokuWiki.Renderer
{
    class Image : IRenderer
    {
        private string internalBaseImagesUrl;

        internal Image(string internalBaseImagesUrl)
        {
            this.internalBaseImagesUrl = internalBaseImagesUrl;
        }

        public string RightAlignCssClass { private get; set; }
        public string LeftAlignCssClass { private get; set; }
        public string CenterAlignCssClass { private get; set; }

        public string Render(Node node)
        {
            var imageNode = (ImageNode)node;
            var url = imageNode.IsInternal
                ? internalBaseImagesUrl + "/" + imageNode.ImageName
                : imageNode.ImageName;

            var cssClass = string.Empty;
            if (imageNode.HasLeftAlign && imageNode.HasRightAlign)
            {
                cssClass = CenterAlignCssClass;
            }
            else if (imageNode.HasRightAlign)
            {
                cssClass = RightAlignCssClass;
            }
            else if (imageNode.HasLeftAlign)
            {
                cssClass = LeftAlignCssClass;
            }

            return $"<img src=\"{url}\""
                + (!string.IsNullOrWhiteSpace(cssClass) ? $" class=\"{cssClass}\"" : "")
                + (imageNode.Width > 0 ? $" width=\"{imageNode.Width}\"" : "")
                + (imageNode.Height > 0 ? $" height=\"{imageNode.Height}\"" : "")
                + (!string.IsNullOrEmpty(imageNode.Title) ? $" title=\"{imageNode.Title.Trim()} alt=\"{imageNode.Title.Trim()}\"" : "")
                + ">";
        }
    }
}
