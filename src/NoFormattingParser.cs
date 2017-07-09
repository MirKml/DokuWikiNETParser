using System.Text.RegularExpressions;

namespace DokuWiki
{
    class NoFormattingParser: RegExpParser
    {
        private const string regExp = "%%(?s:(.*?))%%";

        internal NoFormattingParser() : base(regExp, NodeType.InlineNoFormat)
        { }
    }
}