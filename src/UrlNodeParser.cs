namespace DokuWiki
{
    class UrlNodeParser : RegExpParser
    {
        private const string regExp = "\\[\\[([\\w|> ]+)\\]\\]";

        public UrlNodeParser(): base(regExp, NodeType.UrlNode)
        {
        }
    }
}