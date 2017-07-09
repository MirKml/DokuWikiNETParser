using System.Text.RegularExpressions;

namespace DokuWiki
{
    class ItalicTextParser: RegExpParser
    {
        // regexp for matching italic text
        // italic text doesn't have to start with http:// or https:// 
        private const string regExp = "(?<!http[s]?:)//(?s:(.*?))//";

        internal ItalicTextParser() : base(regExp, NodeType.ItalicText)
        { }
    }
}