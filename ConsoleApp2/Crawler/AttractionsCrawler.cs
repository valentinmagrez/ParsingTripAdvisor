using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp2.Helpers;
using ConsoleApp2.Parsers;
using ConsoleApp2.Parsers.Attractions;
using PuppeteerSharp;

namespace ConsoleApp2.Crawler
{
    public class AttractionsCrawler
    {
        private Browser _browser;

        public AttractionsCrawler(Browser browser)
        {
            _browser = browser;
        }

        public async Task<List<AttractionDto>> Start(Dictionary<string, string> cityAndUrl)
        {
            var a = new PerformanceAnalyser();
            var attractions = new List<AttractionDto>();
            foreach (var (cityName, url) in cityAndUrl)
            {
                var attractionsHtml = a.Measure(async()=> await GetAllAttractionsToParse(url), "find attractions page");
                foreach (var html in await attractionsHtml)
                {
                    var attractionParser = new AttractionParser(html, _browser);
                    var attractioDto = a.Measure(attractionParser.Parse, " parse attraction ");
                    attractioDto.City = cityName;
                    Console.WriteLine("Parse attraction: " + attractioDto.Place);
                    attractions.Add(attractioDto);
                }
            }

            return attractions;
        }

        private async Task<List<string>> GetAllAttractionsToParse(string url)
        {
            var explorer = new PageExplorer(_browser);
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
            var explorer = new PageExplorer(_browser);
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
                var explorer = new PageExplorer(_browser);
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
