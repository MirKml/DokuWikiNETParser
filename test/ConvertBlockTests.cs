using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertBlockTests
    {

        [TestMethod]
        public void ConvertHeadingBlock()
        {
            var headingLevel1Block = "== Level 1 heading ==";
            var headingLevel2Block = "=== Level 2 heading ===";
            Assert.AreEqual("<h1>Level 1 heading</h1>\n", Converter.Convert(headingLevel1Block));
            Assert.AreEqual("<h2>Level 2 heading</h2>\n", Converter.Convert(headingLevel2Block));
        }

        [TestMethod]
        public void ConvertCodeBlock()
        {
            var codeText = @"
public function addItem($sku, $name, $price, $category = null, $quantity = 1) {
 $product = array();
 foreach (array('sku', 'name', 'category', 'price', 'quantity') as $arg) {
  if ($$arg !== null) {
   $product[$arg] = $$arg;
  }
 }

 if (empty($this->transaction['transactionProducts'])) {
  $this->transaction['transactionProducts'] = array();
 }
 $this->transaction['transactionProducts'][] = $product;
 return $this;
}
";
            var wikiText = "<code php>" + codeText + "</code>";
            Assert.AreEqual("<pre><code class=\"php\">" + codeText + "</code></pre>\n", Converter.Convert(wikiText));
        }

        [TestMethod]
        public void ConvertOneParagraph()
        {
            var wikiText = @"

Tohle je začátek prvního odstavce, první věta.
A odřádkovaná druhá věta ale pořád první odstavec.";

            Assert.AreEqual(@"<p>
Tohle je začátek prvního odstavce, první věta.
A odřádkovaná druhá věta ale pořád první odstavec.
</p>
"
                , Converter.Convert(wikiText));
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
            Assert.AreEqual(@"<p>
První věta prního odstavce, není až tak dlouhá, ale už jí skončím.
Tohle je druhá věta, &quot;odřádkovaná&quot; jedním EOL, ale stále je v prvním odstavci.
</p>
<h3>Nadpis druhého odstavce</h3>
<p>
Tohle je začátek druhého odstavce, první věta.
A odřádkovaná druhá věta ale pořád druhý odstavec.
</p>
<p>
Tohle je začátek třetího odstavce, první věta. Odřádkováno po více než dvou řádcích
A odřádkovaná druhá věta ale pořád třetí odstavec.
</p>
<h3>Nadpis čtvrtého odstavce</h3>
<p>
Tohle je začátek čtvrtého odstavce, první věta.
A odřádkovaná druhá věta ale pořád čtvrtý odstavec.
</p>
"
                , Converter.Convert(wikiText));
        }
    }
}