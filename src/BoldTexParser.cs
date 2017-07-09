using System.Text.RegularExpressions;

namespace DokuWiki
{
    class BoldTextParser: RegExpParser
    {
        private const string regExp = "\\*\\*(?s:(.*?))\\*\\*";

        internal BoldTextParser() : base(regExp, NodeType.BoldText)
        { }
    }
}