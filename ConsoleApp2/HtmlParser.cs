using HtmlAgilityPack;

namespace ConsoleApp2
{
    public abstract class HtmlParser<T>
    {
        protected readonly string Html;

        public HtmlParser(string html)
        {
            Html = html;
        }

        protected HtmlDocument LoadHtml()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Html);
            return htmlDoc;
        }

        public abstract T Parse();
    }
}
