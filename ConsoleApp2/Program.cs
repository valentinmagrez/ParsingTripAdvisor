using System;
using System.Collections.Generic;
using ConsoleApp2.Crawler;
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
            //{
            //    "Tozeur",
            //    "https://www.tripadvisor.com/Attractions-g293757-Activities-a_allAttractions.true-Tozeur_Tozeur_Governorate.html"
            //},
            // {
            //            "Douz",
            //            "https://www.tripadvisor.com/Attractions-g477972-Activities-a_allAttractions.true-Douz_Kebili_Governorate.html"
            //        },            {
            //"Nefta",
            //            "https://www.tripadvisor.com/Attractions-g297951-Activities-Nefta_Tozeur_Governorate.html"
            //        },
            {
            "Kebili",
            "https://www.tripadvisor.com/Attractions-g1115268-Activities-Kebili_Kebili_Governorate.html"
        },
            // {
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
                var browser = CreateBrowser();
                var attractions = new AttractionsCrawler(browser).Start(CityAndTripLink).GetAwaiter().GetResult();

                new ExcelBuilder().Create(attractions);
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        private static Browser CreateBrowser()
        {
            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            return Puppeteer.LaunchAsync(options).GetAwaiter().GetResult();
        }
    }
}
