using System;
using System.Linq;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace ConsoleApp2.Parsers.Attractions
{
    public class ReviewUserParser : HtmlParser<UserDto>
    {
        public ReviewUserParser(string html):base(html)
        {
        }

        public override UserDto Parse()
        {
            var html = LoadHtml();
            var name = ParseName(html);
            var originAndRateNumber = ParseOriginAndRateNumber(html);

            return new UserDto
            {
                Name = name,
                RateNumber = Convert.ToInt32(String.Join("", originAndRateNumber.Item2.Where(char.IsDigit))),
                Origin = originAndRateNumber.Item1
            };
        }

        private Tuple<string, string> ParseOriginAndRateNumber(HtmlDocument html)
        {
            try
            {
                var divContent = html.QuerySelectorAll("div.biGQs._P.pZUbB.osNWb span");
                if (divContent.Count == 0)
                    return OldParseOriginAndRateNumber(html);
                if (divContent.Count > 1)
                    return new Tuple<string, string>(divContent[0].InnerHtml, divContent[1].InnerHtml);

                return new Tuple<string, string>(string.Empty, divContent[0].InnerHtml);
                ;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        private Tuple<string, string> OldParseOriginAndRateNumber(HtmlDocument html)
        {
            var originHtml = html.QuerySelector("span.RdTWF span.default.LXUOn.small");
            var contributionHtml = html.QuerySelector("span.phMBo span.yRNgz");

            return new Tuple<string, string>(originHtml.InnerText, contributionHtml.InnerText);
        }

        private string ParseName(HtmlDocument html)
        {
            try
            {
                var contentValue = html.QuerySelector("div.zpDvc a");
                if (contentValue is null)
                    contentValue = html.QuerySelector("a");
                if (contentValue is null)
                    contentValue = html.QuerySelector("span.WlYyy.cPsXC.dTqpp");
                return contentValue.InnerHtml;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }


    public class UserDto
    {
        public string Name { get; set; }
        public int RateNumber { get; set; }
        public string Origin { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not UserDto user)
                return false;

            return Name == user.Name
                   && Origin == user.Origin
                   && RateNumber == user.RateNumber;
        }
    }
}
