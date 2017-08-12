using System.Linq;

namespace DokuWiki
{
    class UrlNodeParser : RegExpParser
    {
        /// <summary>
        /// Regexp for parsing url nodes from wiki text
        /// e.g. [[http://www.mirin.cz|this is my homepage]]
        /// </summary>
        private const string regExp = "\\[\\[(http.+?)\\]\\]";

        public UrlNodeParser(): base(regExp, NodeType.UrlNode)
        {
        }

        internal override Node[] GetNodes(string wikiText)
        {
            var nodes = base.GetNodes(wikiText);
            if (!nodes.Any()) return nodes;

            // set additional node properties
            foreach (UrlNode node in nodes)
            {
                var urlItems = node.Content.Split('|');
                node.Url = urlItems[0];
                node.Title = urlItems.Length == 2 ? urlItems[1] : node.Url;
            }

            return nodes;
        }
    }
}