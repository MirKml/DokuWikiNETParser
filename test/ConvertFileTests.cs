using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DokuWiki.Test
{
    [TestClass]
    public class ConvertFileTest
    {
        [TestMethod]
        public void ConvertText1()
        {
            var testDataDir = Path.Combine("../../..", "testData");
            var htmlOutput = Converter.Convert(File.ReadAllText($"{testDataDir}/404text.wiki"));
            Assert.AreEqual(File.ReadAllText($"{testDataDir}/404text.html"), htmlOutput);
        }
    }
}