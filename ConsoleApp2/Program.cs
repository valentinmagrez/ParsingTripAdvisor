using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace ConsoleApp2
{
    public class Program
    {
        private static readonly Dictionary<string, string> CityAndTripLink = new()
        {
            //        {
            //            "Hammamet",
            //            "https://www.tripadvisor.com/Attractions-g297943-Activities-a_allAttractions.true-Hammamet_Nabeul_Governorate.html"
            //        },
            //        {
            //            "Sousse",
            //            "https://www.tripadvisor.fr/Attractions-g295401-Activities-a_allAttractions.true-Sousse_Sousse_Governorate.html"
            //        },
            {
                "Tozeur",
                "https://www.tripadvisor.com/Attractions-g293757-Activities-a_allAttractions.true-Tozeur_Tozeur_Governorate.html"
            },
            // {
            //            "Douz",
            //            "https://www.tripadvisor.com/Attractions-g477972-Activities-a_allAttractions.true-Douz_Kebili_Governorate.html"
            //        },            {
            //"Nefta",
            //            "https://www.tripadvisor.com/Attractions-g297951-Activities-Nefta_Tozeur_Governorate.html"
            //        },            {
            //"Kebili",
            //            "https://www.tripadvisor.com/Attractions-g1115268-Activities-Kebili_Kebili_Governorate.html"
            //        },            {
            //"Tabarka",
            //            "https://www.tripadvisor.com/Attractions-g297953-Activities-Tabarka_Jendouba_Governorate.html"
            //        },            {
            //"Ain Drahem",
            //            "https://www.tripadvisor.com/Attractions-g1791611-Activities-Ain_Draham_Jendouba_Governorate.html"
            //        },
            //    {
            //    "Kairouan",
            //    "https://www.tripadvisor.com/Attractions-g303925-Activities-a_allAttractions.true-Kairouan_Kairouan_Governorate.html"
            //}
            //{ "Marrakech" , "https://www.tripadvisor.com/Attractions-g293734-Activities-a_allAttractions.true-Marrakech_Marrakech_Safi.html" },
            //{ "Agadir", "https://www.tripadvisor.com/Attractions-g293731-Activities-a_allAttractions.true-Agadir_Souss_Massa.html" },
            //{ "Essaouira", "https://www.tripadvisor.com/Attractions-g298349-Activities-a_allAttractions.true-Essaouira_Marrakech_Safi.html" },
            //{ "Fès", "https://www.tripadvisor.com/Attractions-g293733-Activities-a_allAttractions.true-Fes_Fes_Meknes.html" },
            //{ "Chefchaouen", "https://www.tripadvisor.com/Attractions-g304013-Activities-a_allAttractions.true-Chefchaouen_Tanger_Tetouan_Al_Hoceima.html" },

        };

        static void Main(string[] args)
        {
            try
            {
                var a = new PerformanceAnalyser();
                var parser = TripAdvisorParser.Create().GetAwaiter().GetResult();

                var attractions = new List<AttractionDto>();
                foreach (KeyValuePair<string, string> cityLink in CityAndTripLink)
                {
                    var attractionsHtml = a.Measure(parser.GetAllAttractions(cityLink.Value).GetAwaiter().GetResult, "find attractions page");
                    foreach (var html in attractionsHtml)
                    {
                        var attractionParser = new AttractionParser(html, parser.Browser);
                        var attractioDto = a.Measure(attractionParser.Parse, " parse attraction ");
                        attractioDto.City = cityLink.Key;
                        Console.WriteLine("Parse attraction: " + attractioDto.Place);
                        attractions.Add(attractioDto);
                    }
                }

                new ExcelBuilder().Create(attractions);
            }
            catch (Exception e)
            {
                var a = e;
            }
        }


        public class TripAdvisorParser
        {
            public Browser Browser { get; }

            public static async Task<TripAdvisorParser> Create()
            {
                var options = new LaunchOptions()
                {
                    Headless = true,
                    ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
                };

                var browser = await Puppeteer.LaunchAsync(options);
                return new TripAdvisorParser(browser);
            }

            public TripAdvisorParser(Browser browser)
            {
                Browser = browser;
            }

            private async Task<Page> LoadPage(string url)
            {
                try
                {
                    var page = await Browser.NewPageAsync();
                    await page.GoToAsync(url);
                    return page;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }

            public async Task<List<string>> GetAllAttractions(string url)
            {
                var mainPage = await LoadPage(url);
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
                    var test = await Browser.PagesAsync();
                    return attraction;
                });

                return (await Task.WhenAll(loadAttractions)).SelectMany(_=>_).ToList();
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
                var nextPageLink = @"Array.from(document.querySelectorAll('a[aria-label=""Page suivante""]')).map(a => a.href);";
                var links = await currentPage.EvaluateExpressionAsync<string[]>(nextPageLink);
                if (links.Length == 0)
                    return null;
                Console.WriteLine("Found: "+links[0]);
                return await LoadPage(links[0]);
            }

            private async Task<List<string>> GetAttractionsFromPage(Page page)
            {
                var href = @"Array.from(document.querySelectorAll('article a')).map(a => a.href);";
                var links = await page.EvaluateExpressionAsync<string[]>(href);
                var timeout = 0;
                while (links.Length == 0 && timeout<60)
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
                    Console.WriteLine("Navigate to : "+link);
                    var attractionPage = await LoadPage(link);
                    attractionsHtml.Add(await attractionPage.GetContentAsync());
                    try
                    {
                        await attractionPage.DisposeAsync();
                    }catch(Exception e){}

                    ;
                }

                return attractionsHtml;
            }
        }
    }
}
