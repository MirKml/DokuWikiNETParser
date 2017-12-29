namespace DokuWiki
{
    public static class Converter
    {
        public static string Convert(string wikiText)
        {
            return new NodeListRenderer().Render(new Parser().Parse(wikiText));
        }

        public static string Convert(string wikiText, ConverterConfiguration configuration)
        {
            var nodesRenderer = new NodeListRenderer();
            nodesRenderer.ImagesBaseUrl = configuration.ImagesBaseUrl;
            nodesRenderer.ImageLeftAlignCssClass = configuration.ImageLeftAlignCssClass;
            nodesRenderer.ImageRightAlignCssClass = configuration.ImageRightAlignCssClass;
            nodesRenderer.ImageCenterAlignCssClass = configuration.ImageCenterAlignCssClass;

            return nodesRenderer.Render(new Parser().Parse(wikiText));
        }
    }

    public class ConverterConfiguration
    {
        public string ImagesBaseUrl { get; set; }
        public string ImageLeftAlignCssClass { get; set; }
        public string ImageRightAlignCssClass { get; set; }
        public string ImageCenterAlignCssClass { get; set; }
    }
}