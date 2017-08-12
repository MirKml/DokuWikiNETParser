using System.Text.RegularExpressions;

namespace DokuWiki
{
    class ItalicTextParser: RegExpParser
    {
        /// <summary>
        /// Regexp for matching italic text.
        /// Italic text doesn't have to start with http:// or https://.
        /// </summary>
        private const string regExp = "(?<!http[s]?:)//(?s:(.*?))//";

        internal ItalicTextParser() : base(regExp, NodeType.ItalicText)
        { }
    }
}