using ConsoleApp2.Parsers.Attractions;
using NFluent;
using NUnit.Framework;

namespace TripAdvisor.UnitTest
{
    public class ReviewCardParserTest
    {
        private const string ReviewCardHtml = @"
<div class=""_c"" data-automation=""reviewCard"">
    <div class=""mwPje f M k"">
        <div class=""XExLl f u o"">
            <div class=""hzzSG"">
                <div class=""MLvbw f u"">
                    <div class=""AYlYS"" style=""z-index:0"">
                        <div class="""">
                            <div class=""tknvo ccudK Rb I o"">
                                <div class=""""><a target=""_self"" tabindex=""-1"" aria-hidden=""true""
                                        href=""/Profile/tikicici2"" class=""BMQDV _F G- wSSLS SwZTJ"">
                                        <div class=""FGwzt PaRlG"">
                                            <picture class=""NhWcC _R mdkdE"" style=""width:32px;height:32px""><img
                                                    src=""https://dynamic-media-cdn.tripadvisor.com/media/photo-o/09/35/39/94/travel-imr.jpg?w=100&amp;h=-1&amp;s=1""
                                                    width=""100"" height=""100"" alt=""tikicici2"" loading=""lazy""></picture>
                                        </div>
                                    </a></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""zpDvc""><span class=""biGQs _P fiohW fOtGX""><a target=""_self"" href=""/Profile/tikicici2""
                        class=""BMQDV _F G- wSSLS SwZTJ FGwzt ukgoS"">tikicici2</a></span>
                <div class=""JINyA"">
                    <div class=""biGQs _P pZUbB osNWb""><span>Old Bridge, NJ</span><span class=""IugUm"">208
                            contributions</span></div>
                </div>
            </div>
        </div>
        <div class=""f""><button class=""BrOJk u j z _F wSSLS HuPlH Vonfv"" type=""button""
                aria-label=""Click to add helpful vote""><svg viewBox=""0 0 24 24"" width=""20px"" height=""20px""
                    class=""d Vb UmNoP"">
                    <path
                        d=""M3.025 9.963c-.566 0-1.025.459-1.025 1.025v9.732h2.051v-9.732c0-.566-.459-1.025-1.026-1.025zM21.493 9.111a2.05 2.05 0 00-1.546-.703h-4.306l.541-2.67c.122-.606-.032-1.228-.424-1.706s-.97-.753-1.588-.753h-2.348l-5.72 7.358V20.72h12.59a2.038 2.038 0 002.027-1.74l1.261-8.241a2.045 2.045 0 00-.487-1.628zm-2.799 9.557H8.154v-7.326l4.672-6.01h1.345l-1.037 5.128 6.816-.015-1.256 8.223z"">
                    </path>
                </svg><span class=""kLqdM""><span class=""biGQs _P FwFXZ"">0</span></span></button>
            <div class=""_T"">
                <div class=""w _T""><button class=""BrOJk u j z _F wSSLS HuPlH Vonfv"" type=""button"" aria-haspopup=""menu""
                        aria-label=""Open Options Menu""><svg viewBox=""0 0 24 24"" width=""20px"" height=""20px""
                            class=""d Vb UmNoP"" x=""0"" y=""0"">
                            <circle cx=""12"" cy=""19.4"" r=""2.5""></circle>
                            <circle cx=""12"" cy=""4.4"" r=""2.5""></circle>
                            <circle cx=""12"" cy=""11.9"" r=""2.5""></circle>
                        </svg></button></div>
            </div>
        </div>
    </div>
    <div><svg class=""UctUV d H0"" viewBox=""0 0 128 24"" width=""88"" height=""16"" aria-label=""5.0 of 5 bubbles"">
            <path d=""M 12 0C5.388 0 0 5.388 0 12s5.388 12 12 12 12-5.38 12-12c0-6.612-5.38-12-12-12z"" transform="""">
            </path>
            <path d=""M 12 0C5.388 0 0 5.388 0 12s5.388 12 12 12 12-5.38 12-12c0-6.612-5.38-12-12-12z""
                transform=""translate(26 0)""></path>
            <path d=""M 12 0C5.388 0 0 5.388 0 12s5.388 12 12 12 12-5.38 12-12c0-6.612-5.38-12-12-12z""
                transform=""translate(52 0)""></path>
            <path d=""M 12 0C5.388 0 0 5.388 0 12s5.388 12 12 12 12-5.38 12-12c0-6.612-5.38-12-12-12z""
                transform=""translate(78 0)""></path>
            <path d=""M 12 0C5.388 0 0 5.388 0 12s5.388 12 12 12 12-5.38 12-12c0-6.612-5.38-12-12-12z""
                transform=""translate(104 0)""></path>
        </svg></div>
    <div class=""biGQs _P fiohW qWPrE ncFvv fOtGX""><a target=""_blank"" tabindex=""0""
            href=""/ShowUserReviews-g295401-d3653239-r844416061-Sousse_Archaeological_Museum-Sousse_Sousse_Governorate.html""
            class=""BMQDV _F G- wSSLS SwZTJ FGwzt ukgoS""><span class=""yCeTE"">Museum closed on monday - its a must see</span></a></div>
    <div class=""RpeCd"">Jun 2022</div>
    <div class=""_T FKffI"">
        <div class=""fIrGe _T bgMZj"" style=""line-break: normal; cursor: auto;"">
            <div class=""biGQs _P pZUbB KxBGd""><span class=""yCeTE"">the museum is close on Mondays. this is the only place that you can see mosaics on the original color and texture. its one of the most small and impressive museums.</span></div>
        </div>
        <div class=""lszDU"" style=""line-height: 22px;""><button class=""UikNM _G B- _S _T c G_ P0 wSSLS wnNQG""
                type=""button""><span class=""biGQs _P XWJSj Wb"">Read more</span>
                <div class=""CECjK""><svg viewBox=""0 0 24 24"" width=""20px"" height=""20px"" class=""d Vb UmNoP"">
                        <path d=""M18.4 7.4L12 13.7 5.6 7.4 4.2 8.8l7.8 7.8 7.8-7.8z""></path>
                    </svg></div>
            </button></div>
    </div>
    <div></div>
    <div class=""TreSq"">
        <div class=""biGQs _P pZUbB ncFvv osNWb"">Written June 24, 2022</div>
        <div class=""biGQs _P pZUbB mowmC osNWb"">This review is the subjective opinion of a Tripadvisor member and not of
            Tripadvisor LLC. Tripadvisor performs checks on reviews.</div>
    </div>
</div>
";

        private const string ReviewOldCardHtml = @"<div class=""lgfjP Gi z pBVnE MD bZHZM"">
    <div class=""sCZGP""><a href=""/Profile/JoshuaG1571"" target=""_self"" class=""kjIqZ I ui_social_avatar inline""><img alt=""""
                src=""https://media-cdn.tripadvisor.com/media/photo-t/1b/bb/84/44/joshuag1571.jpg""></a>
        <div class=""IHLTq _Z o"">
            <div class=""jXhbq f _Y _m"">
                <div class=""overflow ""><span class=""GXoUa NF S8 _S MVUuZ""><svg viewBox=""0 0 24 24"" width=""1em""
                            height=""1em"" class=""d Vb UmNoP"" x=""0"" y=""0"">
                            <circle cx=""4.5"" cy=""11.9"" r=""2.5""></circle>
                            <circle cx=""19.5"" cy=""11.9"" r=""2.5""></circle>
                            <circle cx=""12"" cy=""11.9"" r=""2.5""></circle>
                        </svg></span></div><span class=""_m""></span>
            </div>
            <div class=""cRVSd""><span><a class=""ui_header_link uyyBf"" href=""/Profile/JoshuaG1571"">Joshua G</a> wrote a
                    review Aug 2020</span></div>
            <div class=""MziKN""><span class=""RdTWF""><span class=""default LXUOn small""><span
                            class=""ui_icon map-pin-fill fXexN""></span>Tunisia</span></span><span
                    class=""phMBo""><span><span class=""yRNgz"">26</span> contributions</span></span><span
                    class=""phMBo""><span><span class=""yRNgz"">13</span> helpful votes</span></span></div>
        </div>
    </div>
    <div class=""pDrIj f z"">
        <div class=""MtByY _T PNjsP"">
            <div class=""BSBvb GA"" style=""height: 128px; width: 100%; position: relative;""><img class=""_Q t _U s l KSVvt""
                    src=""https://dynamic-media-cdn.tripadvisor.com/media/photo-o/1b/bb/80/01/tunisia-dicoveries.jpg?w=800&amp;h=200&amp;s=1""
                    alt="""" style=""height: 100%; width: 100%; object-fit: cover;""></div>
        </div>
    </div>
    <div class=""WAllg _T"" data-reviewid=""763938620"">
        <div class=""IkECb f O"">
            <div class=""Hlmiy F1"" data-test-target=""review-rating""><span class=""ui_bubble_rating bubble_10""></span>
            </div>
        </div>
        <div class=""KgQgP MC _S b S6 H5 _a"" dir=""ltr"" data-test-target=""review-title""><a
                href=""/ShowUserReviews-g295401-d16777379-r763938620-Tunisia_Dicoveries-Sousse_Sousse_Governorate.html""
                class=""Qwuub""><span><span>Don’t visit Tunisia if your British Now</span></span></a></div>
        <div class=""vTVDc"">
            <div class=""_T FKffI bmUTE"">
                <div class=""fIrGe _T"" style=""max-height: initial; line-break: normal; cursor: auto;""><q
                        class=""QewHA H4 _a""><span>Did 4 dives with them (August 5 &amp; 6). Hosni (owner and diving
instructor) and the rest of the staff are very nice, friendly and they make sure that
everybody enjoys the experience (scuba diving or snorkeling). The second day I went back
with my kids. For the first time they had  the chance to breath
under water. They loved it. Will definitively dive with them again. Thank you
Hosni.</span>…</q></div>
                <div class=""lszDU"" style=""line-height: 20px;"">
                    <div class=""TnInx"" data-test-target=""expand-review""><span class=""Ignyf _S Z"">Read more</span><span
                            class=""ui_icon caret-down Lvqmo""></span></div>
                </div>
            </div>
            <div class=""JGazz""><span class=""teHYY _R Me S4 H3""><span class=""usajM"">Date of experience:</span> August 2020</span>
                <div class=""XCJRC S2 Me MD i"">This review is the subjective opinion of a Tripadvisor member and not of
                    Tripadvisor LLC. Tripadvisor performs checks on reviews.</div>
            </div>
            <div class=""YnxVY""><span class=""wpsrP f _T Xd Y""></span>
                <div class=""qIICj f _T Me z""><span class=""zMxkM f _T M _S""><span
                            class=""ui_icon thumbs-up bohqm""></span><span
                            class=""ui_link RAPNy"">Helpful</span></span><span class=""CXSid f"">
                        <div><span>
                                <div class=""mGZLW f _T M _S qNoXj""><span><span class=""ui_icon share AWqKr""></span><span
                                            class=""ui_link duDPx socialCTA"">Share
                                            <!-- -->
                                        </span></span></div>
                            </span></div>
                    </span></div>
            </div>
        </div>
    </div>
</div>";

        private ReviewCardParser Parser { get; set; }

        [SetUp]
        public void Setup()
        {
            Parser = new ReviewCardParser(ReviewCardHtml);
        }

        [Test]
        [TestCase(ReviewCardHtml, "the museum is close on Mondays. this is the only place that you can see mosaics on the original color and texture. its one of the most small and impressive museums.")]
        [TestCase(ReviewOldCardHtml, "Did 4 dives with them (August 5 &amp; 6). Hosni (owner and diving\r\ninstructor) and the rest of the staff are very nice, friendly and they make sure that\r\neverybody enjoys the experience (scuba diving or snorkeling). The second day I went back\r\nwith my kids. For the first time they had  the chance to breath\r\nunder water. They loved it. Will definitively dive with them again. Thank you\r\nHosni.")]
        public void Parse_Content(string html, string expectedResult)
        {
            var result = new ReviewCardParser(html).Parse();

            Check.That(result.Content).IsEqualTo(expectedResult);
        }

        [Test]
        [TestCase(ReviewCardHtml ,5.0)]
        [TestCase(ReviewOldCardHtml ,1.0)]
        public void Parse_Rate(string html, double expected)
        {
            var result = new ReviewCardParser(html).Parse();

            Check.That(result.Rate).IsEqualTo(expected);
        }
        [Test]
        [TestCase(ReviewCardHtml, "June 24, 2022")]
        [TestCase(ReviewOldCardHtml, "Aug 2020")]
        public void Parse_PublicationDate(string html, string expected)
        {
            var result = new ReviewCardParser(html).Parse();

            Check.That(result.PublicationDate).IsEqualTo(expected);
        }
        [Test]
        [TestCase(ReviewCardHtml, "Jun 2022")]
        [TestCase(ReviewOldCardHtml, "August 2020")]
        public void Parse_TripDate(string html, string expected)
        {
            var result = new ReviewCardParser(html).Parse();

            Check.That(result.TripDate).IsEqualTo(expected);
        }
        [Test]
        [TestCase(ReviewCardHtml, "Museum closed on monday - its a must see")]
        [TestCase(ReviewOldCardHtml, "Don’t visit Tunisia if your British Now")]
        public void Parse_Title(string html, string expected)
        {
            var result = new ReviewCardParser(html).Parse();

            Check.That(result.Title).IsEqualTo(expected);
        }

        [Test]
        [TestCase(ReviewCardHtml, "tikicici2", "Old Bridge, NJ", 208 )]
        //[TestCase(ReviewOldCardHtml, "Eric L", "Canton of Vaud, Switzerland", 5 )]
        public void Parse_User(string html, string name, string origin, int rateNumber)
        {
            var result = new ReviewCardParser(html).Parse();

            var expected = new UserDto { Name = name, Origin = origin, RateNumber = rateNumber };
            Check.That(result.User).IsEqualTo(expected);
        }
    }
}