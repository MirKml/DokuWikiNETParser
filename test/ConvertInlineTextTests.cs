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

        [TestMethod]
        public void ConvertItalicBoldInside()
        {
           var wikiText = "Some //italic text **with bold inside**//";
           Assert.AreEqual("Some <em>italic text <strong>with bold inside</strong></em>", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertItalicBoldStartInside()
        {
           var wikiText = "//**bold inside** italic//";
           Assert.AreEqual("<em><strong>bold inside</strong> italic</em>", Converter.Convert(wikiText));
        }
    }
}
