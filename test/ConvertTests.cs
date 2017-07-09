using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertTests
    {
        [TestMethod]
        public void ConvertBold()
        {
           var wikiText = "**some bold text**";
           Assert.AreEqual("<strong>some bold text</strong>", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertPlainWithBold()
        {
           var wikiText = "Some plain text and **some bold text**";
           Assert.AreEqual("Some plain text and <strong>some bold text</strong>", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertParagraphs()
        {
            var wikiText = @"První věta prního odstavce, není až tak dlouhá, ale už jí skončím.
Tohle je druhá věta, ""odřádkovaná"" jedním EOL, ale stále je v prvním odstavci.

==== Nadpis druhého odstavce ====
Tohle je začátek druhého odstavce, první věta.
A odřádkovaná druhá věta ale pořád druhý odstavec.


Tohle je začátek třetího odstavce, první věta. Odřádkováno po více než dvou řádcích
A odřádkovaná druhá věta ale pořád třetí odstavec.

==== Nadpis čtvrtého odstavce ====
Tohle je začátek čtvrtého odstavce, první věta.
A odřádkovaná druhá věta ale pořád čtvrtý odstavec.";

           Assert.AreEqual("", Converter.Convert(wikiText));
        }
    }
}
