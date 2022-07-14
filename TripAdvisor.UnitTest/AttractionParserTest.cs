using ConsoleApp2;
using NFluent;
using NUnit.Framework;

namespace TripAdvisor.UnitTest
{
    public class AttractionParserTest
    {
        private const string ReviewCardHtml = @"
            <div class=""cjhIj""><span class=""WlYyy cPsXC dTqpp""><a target=""_self"" href=""/Profile/Q502BHsarahb""
                        class=""iPqaD _F G- ddFHE eKwUx btBEK fUpii"">Sarah</a></span>
                <div class=""ddOtn"">
                    <div class=""WlYyy diXIH bQCoY""><span>Paris, France</span><span class=""fhriQ"">24 contributions</span>
                    </div>
                </div>
            </div>
";

        private ReviewUserParser Parser { get; set; }
        [SetUp]
        public void Setup()
        {
            Parser = new ReviewUserParser(ReviewCardHtml);
        }
        
        [Test]
        public void Parse_Rate()
        {
            var result = Parser.Parse();

            Check.That(result.RateNumber).IsEqualTo(24);
        }
        [Test]
        public void Parse_Origin()
        {
            var result = Parser.Parse();

            Check.That(result.Origin).IsEqualTo("Paris, France");
        }

        [Test]
        public void Parse_Name()
        {
            var result = Parser.Parse();

            Check.That(result.Name).IsEqualTo("Sarah");
        }
    }
}