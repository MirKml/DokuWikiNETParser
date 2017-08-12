using System.Text.RegularExpressions;

namespace DokuWiki
{
    class UnderlineTextParser: RegExpParser
    {
        private const string regExp = "__(.*?)__";

        internal UnderlineTextParser() : base(regExp, NodeType.UnderlineText)
        { }
    }
}