using System.Text.RegularExpressions;

namespace DokuWiki
{
    class EmphasisTextParser: RegExpParser
    {
        private const string regExp = "__(.*?)__";

        internal EmphasisTextParser() : base(regExp, NodeType.BoldText)
        { }
    }
}