using System;
using System.Linq;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using OfficeOpenXml.Drawing.Style.Fill;

namespace ConsoleApp2
{
    public class ReviewCardParser : HtmlParser<ReviewDto>
    {
        public ReviewCardParser(string html) : base(html)
        {
        }

        public override ReviewDto Parse()
        {
            try
            {
                var htmlDoc = LoadHtml();

                var content = ParseContent(htmlDoc);
                var rate = ParseRate(htmlDoc);
                var title = ParseTitle(htmlDoc);
                var publication = ParsePublicationDate(htmlDoc);
                var tripDate = ParseTripDate(htmlDoc);
                var user = ParseUser(htmlDoc);

                return new ReviewDto
                {
                    User = user,
                    Title = title,
                    PublicationDate = publication,
                    TripDate = tripDate,
                    Content = content,
                    Rate = rate
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private UserDto ParseUser(HtmlDocument htmlDoc)
        {
            try
            {
                var userHtml = htmlDoc.QuerySelector("div.XExLl");
                if (userHtml is null)
                    userHtml = htmlDoc.QuerySelector("div.xMxrO");
                var userParser = new ReviewUserParser(userHtml.InnerHtml);

                return userParser.Parse();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string ParseTripDate(HtmlDocument htmlDoc)
        {
            try
            {
                var tripInfoHtml = htmlDoc.QuerySelector("div.RpeCd");
                if (tripInfoHtml is null)
                    return OldParseTripDate(htmlDoc);
                var tripInfo = tripInfoHtml.InnerHtml.Split(' ');
                return tripInfo[0] + " " + tripInfo[1];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string OldParseTripDate(HtmlDocument htmlDoc)
        {
            var dateHtml = htmlDoc.QuerySelector("span.teHYY._R.Me.S4.H3");
            if (dateHtml is null)
                return null;
            var dateText = dateHtml.InnerHtml.Split(' ').Reverse().ToList();
            return dateText[1] + " " + dateText[0];
        }

        private string ParsePublicationDate(HtmlDocument htmlDoc)
        {
            try
            {
                var publicationDateHtml = htmlDoc.QuerySelector("div.ncFvv.osNWb");
                if (publicationDateHtml is null)
                    return OldParsePublicationDate(htmlDoc);
                var content = publicationDateHtml.InnerHtml;
                if(content.Contains("Written"))
                {
                    return content.Split("Written ")[1];
                }

                return content.Split("Écrit le ")[1];
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private string OldParsePublicationDate(HtmlDocument htmlDoc)
        {
            var publicationDateHtml = htmlDoc.QuerySelector("div.cRVSd span");
            var texts = publicationDateHtml.InnerText.Split(' ').Reverse().ToList();
            return texts[1] + " " + texts[0];
        }

        private string ParseTitle(HtmlDocument htmlDoc)
        {
            var titleHtml = htmlDoc.QuerySelector("a span.yCeTE");
            if (titleHtml is not null)
                return titleHtml.InnerHtml;
            return htmlDoc.QuerySelector("div.KgQgP.MC._S.b.S6.H5._a").InnerText;
        }

        private string ParseContent(HtmlDocument html)
        {
            var contentValue = html.QuerySelector("div.KxBGd span.yCeTE");
            if (contentValue is not null)
                return contentValue.InnerHtml;
            var tmp = html.QuerySelectorAll("q.XllAv.H4._a span");
            var result = string.Join(' ', html.QuerySelectorAll("q.QewHA.H4._a span").Select(_=>_.InnerHtml));
            return result;
        }

        public double ParseRate(HtmlDocument html)
        {
            try
            {
                var rateHtml = html.QuerySelector("svg.UctUV");
                var rateValue = rateHtml.Attributes.First(_ => _.Name == "aria-label")
                    .DeEntitizeValue;
                var extractedRate = rateValue.Split(' ')[0].Replace('.', ',');

                return Convert.ToDouble(extractedRate);
            }
            catch (Exception e)
            {
                return OldParseRate(html);
            }
        }

        private double OldParseRate(HtmlDocument html)
        {
            var rateHtml = html.QuerySelector("span.ui_bubble_rating").GetClasses().First(_ => _.StartsWith("bubble_"));
            return Convert.ToInt32(rateHtml.Split('_')[1]) / 10;
        }
    }

    public class ReviewDto
    {
        public UserDto User { get; set; }
        public string PublicationDate { get; set; }
        public string TripDate { get; set; }
        public double Rate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
