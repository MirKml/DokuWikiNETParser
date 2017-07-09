namespace DokuWiki
{
    public class Converter
    {
        public static string Convert(string wikiText)
        {
            return new NodeListRenderer().Render(new Parser().Parse(wikiText));
        }
    }
}