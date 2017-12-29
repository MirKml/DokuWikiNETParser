using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertImageTests
    {
        [TestMethod]
        public void ConvertWikiImageSimple()
        {
           var wikiText = "{{wiki:dokuwiki-128.png}}";
           Assert.AreEqual("<img src=\"http://internal.loc/images/dokuwiki-128.png\">", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertExternalImage()
        {
           var wikiText = "{{https://www.example-test.com/images/bla11/dokuwiki-128.png?200x29}}";
           Assert.AreEqual("<img src=\"http://internal.loc/images/dokuwiki-128.png\">", Converter.Convert(wikiText));
        }
    }
}
