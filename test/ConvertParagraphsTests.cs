using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertParagraphTests
    {

        [TestMethod]
        public void ConvertCodeParagraph()
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
            Assert.AreEqual("<pre><code class=\"php\">" + codeText + "</code></pre>", Converter.Convert(wikiText));
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
            System.Console.WriteLine(wikiText);
            Assert.AreEqual("", Converter.Convert(wikiText));
        }
    }
}