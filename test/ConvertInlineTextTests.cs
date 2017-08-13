using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertInlineTextTests
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
        public void ConvertPlainWithItalic()
        {
           var wikiText = "Some plain text and //some italic text//";
           Assert.AreEqual("Some plain text and <em>some italic text</em>", Converter.Convert(wikiText));
        }
    }
}
