using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertImageTests
    {
        [TestMethod]
        public void ConvertWikiImageSimple()
        {
            var configuration = new ConverterConfiguration();
            configuration.ImagesBaseUrl = "http://www.testserver.test/images";
            var wikiText = "{{wiki:dokuwiki-128.png}}";
            Assert.AreEqual("<img src=\"http://www.testserver.test/images/dokuwiki-128.png\">", Converter.Convert(wikiText, configuration));
        }

        [TestMethod]
        public void ConvertExternalImageWithResize()
        {
            var wikiText = "{{https://www.example-test.com/images/bla11/dokuwiki-128.png?200x29}}";
            Assert.AreEqual("<img src=\"https://www.example-test.com/images/bla11/dokuwiki-128.png\" width=\"200\" height=\"29\">", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertExternalImageWithRightAlign()
        {
            var wikiText = "{{ https://www.example-test.com/images/bla11/dokuwiki-128.png}}";
            var configuration = new ConverterConfiguration();
            configuration.ImageRightAlignCssClass = "media-right";
            Assert.AreEqual("<img src=\"https://www.example-test.com/images/bla11/dokuwiki-128.png\" class=\"media-right\">", Converter.Convert(wikiText, configuration));
        }

        public void ConvertExternalImageWithLeftAlign()
        {
            var wikiText = "{{https://www.example-test.com/images/bla11/dokuwiki-128.png }}";
            var configuration = new ConverterConfiguration();
            configuration.ImageLeftAlignCssClass = "media-left";
            Assert.AreEqual("<img src=\"https://www.example-test.com/images/bla11/dokuwiki-128.png\" class=\"media-left\">", Converter.Convert(wikiText, configuration));
        }

        public void ConvertExternalImageWithCenterAlign()
        {
            var wikiText = "{{ https://www.example-test.com/images/bla11/dokuwiki-128.png }}";
            var configuration = new ConverterConfiguration();
            configuration.ImageCenterAlignCssClass = "media-center";
            Assert.AreEqual("<img src=\"https://www.example-test.com/images/bla11/dokuwiki-128.png\" class=\"media-center\">", Converter.Convert(wikiText, configuration));
        }

    }
}
