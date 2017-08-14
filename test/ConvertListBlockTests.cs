using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertListBlockTests
    {

        [TestMethod]
        public void ConvertSimpleListBlock()
        {
            var wikiText = @"
  * This is a 1st item 1st level
  * This is a 2nd item 1st level";

            Assert.AreEqual(@"<ul>
 <li>This is a 1st item 1st level</li>
 <li>This is a 2nd item 1st level</li>
</ul>
"
                , Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertLevel2ListBlock()
        {
            var wikiText = @"
  * This is a 1st item 1st level
  * This is a 2nd item 1st level
    * This is a 3rd item 2nd level
    * This is a 4th item 2nd level
  * This is a 5th item 1st level";

            Assert.AreEqual(@"<ul>
 <li>This is a 1st item 1st level</li>
 <li>This is a 2nd item 1st level</li>
 <ul>
  <li>This is a 3rd item 2nd level</li>
  <li>This is a 4th item 2nd level</li>
 </ul>
 <li>This is a 5th item 1st level</li>
</ul>
"
                , Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertLevel2OpenedListBlock()
        {
            var wikiText = @"
  * This is a 1st item 1st level
  * This is a 2nd item 1st level
    * This is a 3rd item 2nd level
    * This is a 4th item 2nd level";

            Assert.AreEqual(@"<ul>
 <li>This is a 1st item 1st level</li>
 <li>This is a 2nd item 1st level</li>
 <ul>
  <li>This is a 3rd item 2nd level</li>
  <li>This is a 4th item 2nd level</li>
 </ul>
</ul>
"
                , Converter.Convert(wikiText));
        }
    }
}