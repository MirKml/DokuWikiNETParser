using System.Text.RegularExpressions;

namespace DokuWiki
{
    /// <summary>
    /// Parses the image wiki content e.g. {{wiki:dokuwiki-128.png}}
    //
    //  with center align and resize
    //  {{ wiki:dokuwiki-128.png?200x50 }}
    //
    //  with external url, rezise, right align
    //  {{https://www.server.com/images/dokuwiki-128.png?200x50 }}
    //
    //  with external url, rezise, right align, title
    //  {{https://www.server.com/images/dokuwiki-128.png?200x50 |Some image title with dash -}}
    /// </summary>
    class ImageNodeParser : RegExpParser
    {
        private const string regExp = "\\{\\{( ?(wiki:|https?://)([\\w\\./-]+)(\\?[0-9]+(?:x[0-9]+)?)? ?(|[\\w -]+)?)\\}\\}";

        public ImageNodeParser() : base(regExp, NodeType.ImageNode)
        { }

        protected override Node CreateNode(Match regExpMatch)
        {
            var node = (ImageNode)base.CreateNode(regExpMatch);
            var wikiOrHttpPart = regExpMatch.Groups[2].Value;
            var imageNamePart = regExpMatch.Groups[3].Value;
            var sizePart = regExpMatch.Groups[4].Value;
            var titlePart = regExpMatch.Groups[5].Value;

            node.IsInternal = wikiOrHttpPart.StartsWith("wiki:");

            node.ImageName = !node.IsInternal
                ? wikiOrHttpPart + imageNamePart
                : imageNamePart;

            if (sizePart.Contains("x"))
            {
                var sizeMatch = Regex.Match(sizePart, "\\?([0-9]+)x([0-9]+)");
                node.Width = int.Parse(sizeMatch.Groups[1].Value);
                node.Height = int.Parse(sizeMatch.Groups[2].Value);
            }
            else if (!string.IsNullOrEmpty(sizePart))
            {
                node.Width = int.Parse(sizePart.Substring(1));
            }

            node.HasRightAlign = node.Content.StartsWith(" ");
            node.HasLeftAlign = node.Content.EndsWith(" ");
            node.Title = titlePart;
            return node;
        }
    }
}