using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertUrlTests
    {
        [TestMethod]
        public void ConvertPlainWithUrl()
        {
           var wikiText = "This is url of my homepage: [[http://www.mirin.cz]]";
           Assert.AreEqual("This is url of my homepage: <a href=\"http://www.mirin.cz\">http://www.mirin.cz</a>", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertUrlWithTitle()
        {
           var wikiText = "[[http://www.mirin.cz|my homepage]]";
           Assert.AreEqual("<a href=\"http://www.mirin.cz\">my homepage</a>", Converter.Convert(wikiText));
        }
    }
}
