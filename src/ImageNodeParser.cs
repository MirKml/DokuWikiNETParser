namespace DokuWiki
{
    class ImageNodeParser : RegExpParser
    {
        private const string regExp = "\\{\\{([\\w|\\? ]+)\\}\\}";

        public ImageNodeParser(): base(regExp, NodeType.UrlNode)
        {
        }
    }
}