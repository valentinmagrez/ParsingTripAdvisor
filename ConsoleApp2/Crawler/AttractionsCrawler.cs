using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp2.Helpers;
using ConsoleApp2.Parsers;
using ConsoleApp2.Parsers.Attractions;
using PuppeteerSharp;

namespace ConsoleApp2.Crawler
{
    public abstract class Crawler<T> where T : ILocationDto
    {
        protected Browser Browser;

        protected Crawler(Browser browser)
        {
            Browser = browser;
        }

        protected abstract string Name { get; }

        public virtual async Task<List<T>> Start(Dictionary<string, string> cityAndUrl)
        {
            var a = new PerformanceAnalyser();
            var attractions = new List<T>();
            foreach (var (cityName, url) in cityAndUrl)
            {
                var attractionsHtml = a.Measure(async () => await GetAllItemsToParse(url), $"find {Name}s page");
                foreach (var html in await attractionsHtml)
                {
                    var attractionParser = GetParser(html);
                    var attractioDto = a.Measure(attractionParser.Parse, $" parse {Name} ");
                    attractioDto.SetLocation(cityName);
                    Console.WriteLine($"Parse {Name}: {attractioDto}");
                    attractions.Add(attractioDto);
                }
            }

            return attractions;
        }

        protected abstract Task<List<string>> GetAllItemsToParse(string url);

        protected abstract HtmlParser<T> GetParser(string html);
    }

    public class AttractionsCrawler : Crawler<AttractionDto>
    {
        public AttractionsCrawler(Browser browser) : base(browser)
        {
        }

        protected override string Name => "Attraction";

        protected override Task<List<string>> GetAllItemsToParse(string url)
        {
            return GetAllAttractionsToParse(url);
        }

        protected override HtmlParser<AttractionDto> GetParser(string html)
        {
            return new AttractionParser(html, Browser);
        }

        private async Task<List<string>> GetAllAttractionsToParse(string url)
        {
            var explorer = new PageExplorer(Browser);
            var mainPage = await explorer.LoadPage(url);
            var pages = await GetPages(mainPage);

            var loadAttractions = pages.Select(async page =>
            {
                var attraction = new List<string>();
                try
                {
                    attraction = await GetAttractionsFromPage(page);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await page.DisposeAsync();
                return attraction;
            });

            return (await Task.WhenAll(loadAttractions)).SelectMany(_ => _).ToList();
        }

        private async Task<List<Page>> GetPages(Page mainPage)
        {
            var pages = new List<Page>();
            var currentPage = mainPage;
            while (currentPage is not null)
            {
                pages.Add(currentPage);
                currentPage = await GetNextPage(currentPage);
            }

            return pages;
        }

        private async Task<Page> GetNextPage(Page currentPage)
        {
            var nextPageLink =
                @"Array.from(document.querySelectorAll('a[aria-label=""Page suivante""]')).map(a => a.href);";
            var links = await currentPage.EvaluateExpressionAsync<string[]>(nextPageLink);
            if (links.Length == 0)
                return null;
            Console.WriteLine("Found: " + links[0]);
            var explorer = new PageExplorer(Browser);
            return await explorer.LoadPage(links[0]);
        }

        private async Task<List<string>> GetAttractionsFromPage(Page page)
        {
            var href = @"Array.from(document.querySelectorAll('article a')).map(a => a.href);";
            var links = await page.EvaluateExpressionAsync<string[]>(href);
            var timeout = 0;
            while (links.Length == 0 && timeout < 60)
            {
                links = await page.EvaluateExpressionAsync<string[]>(href);
                Console.WriteLine("Cant find any attraction");
                Thread.Sleep(1000);
                timeout++;
            }

            var programmerLinks = new HashSet<string>();
            foreach (var link in links)
            {
                if (link.EndsWith(".html") && link.Contains("/Attraction"))
                {
                    var engLink = link.Replace(".fr", ".com");
                    programmerLinks.Add(engLink);
                }
            }

            var attractionsHtml = new List<string>();
            foreach (var link in programmerLinks)
            {
                Console.WriteLine("Navigate to : " + link);
                var explorer = new PageExplorer(Browser);
                var attractionPage = await explorer.LoadPage(link);
                attractionsHtml.Add(await attractionPage.GetContentAsync());
                try
                {
                    await attractionPage.DisposeAsync();
                }
                catch (Exception e)
                {
                }

                ;
            }

            return attractionsHtml;
        }
    }
}
