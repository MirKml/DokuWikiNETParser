using System;
using System.IO;

namespace DokuWiki
{
    class Program
    {
        static void Main(string[] args)
        {
            var wikiTextsDir = "/home/kubelik/MirinWeb/DokuWiki/wikitexts/";
            var wikiText = File.ReadAllText($"{wikiTextsDir}/404text.wiki");
            var parser = new Parser();
            parser.Parse(wikiText);
        }
    }
}
