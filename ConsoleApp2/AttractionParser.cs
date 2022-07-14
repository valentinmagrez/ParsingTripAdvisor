using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using PuppeteerSharp;
using Supremes.Nodes;

namespace ConsoleApp2
{
    public class AttractionParser : HtmlParser<AttractionDto>
    {
        private Browser _browser;
        public AttractionParser(string html, Browser browser) : base(html)
        {
            _browser = browser;
        }

        public override AttractionDto Parse()
        {
            var html = LoadHtml();
            var place = ParsePlace(html);
            Console.WriteLine("Start Parsing: " + place);
            var reviews = ParseReviews(html).GetAwaiter().GetResult();

            return new AttractionDto
            {
                Place = place,
                Reviews = reviews
            };
        }

        private async Task<List<ReviewDto>> ParseReviews(HtmlDocument html)
        {
            var reviews = new List<ReviewDto>();
            var pages = GetReviewsPages(html);

            Parallel.ForEach(await pages, page =>
            {
                reviews.AddRange(ParseReviewsFromPage(page));
            });
            Console.WriteLine($"Parse {reviews.Count} reviews");
            return reviews;
        }

        private List<ReviewDto> ParseReviewsFromPage(HtmlDocument html)
        {
            try
            {
                var reviewsHtml = html.QuerySelectorAll("div#tab-data-qa-reviews-0 div._c");
                if (reviewsHtml is null || reviewsHtml.Count == 0)
                    reviewsHtml = html.QuerySelectorAll("div.eVykL.Gi.z.cPeBe.MD.cwpFC");
                
                var reviews = new List<ReviewDto>();
                Parallel.ForEach(reviewsHtml, reviewHtml =>
                {
                    var parser = new ReviewCardParser(reviewHtml.InnerHtml);
                    var review = parser.Parse();
                    reviews.Add(review);
                    Console.WriteLine("Parse review: " + review.Title);
                });

                return reviews;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<List<HtmlDocument>> GetReviewsPages(HtmlDocument html)
        {
            var pages = new List<HtmlDocument>();
            var currentPage = html;
            while (currentPage is not null)
            {
                pages.Add(currentPage);
                currentPage = await GetNextReviewsPage(currentPage);
            }

            return pages;
        }

        private string ParsePlace(HtmlDocument html)
        {
            try
            {
                var contentValue = html.QuerySelectorAll("header h1")
                    .FirstOrDefault(_ => _.Attributes.Contains("data-automation"));
                if (contentValue is null)
                    contentValue = html.QuerySelector("div.header.heading.masthead.masthead_h1");
                if (contentValue is null)
                    contentValue = html.QuerySelector("h1#HEADING");
                return contentValue.InnerHtml;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        private async Task<HtmlDocument> GetNextReviewsPage(HtmlDocument html)
        {
            var pagesLinkHtml = html.QuerySelectorAll("div.UCacc a");
            var nextPageLinkHtml = pagesLinkHtml.FirstOrDefault(_ => _.Attributes.Contains("data-smoke-attr"));
            if (nextPageLinkHtml is null)
                return null;
            var nextPageLink = "https://www.tripadvisor.com"+nextPageLinkHtml.Attributes.First(_=>_.Name=="href").Value;


            var page = await _browser.NewPageAsync();
            await page.GoToAsync(nextPageLink);
            var nextPageHtml = await page.GetContentAsync();
            await page.DisposeAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(nextPageHtml);

            return htmlDoc;
        }
    }
    
    public class AttractionDto
    {
        public string City { get; set; }
        public string Place { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}