using ScrapySharp.Core;
using ScrapySharp.Html.Parsing;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Crawler.Infrastructure
{
    public class Core4
    {
        public async Task Crawl(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            var page = doc.DocumentNode.SelectSingleNode("//body");


            var documents = doc.DocumentNode.CssSelect(".SVuzf .e").Single().InnerText;//.Attributes["href"].Value;
        }
    }
}
